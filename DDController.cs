//Written by Kevin Bost, http://dotnetgeek.tumblr.com/, kitokeboo@yahoo.com

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace DDLibWPF
{
    public class DDController
    {
        /// <summary>
        /// Occurs when this controller starts a drag drop operation.
        /// </summary>
        public event EventHandler DDStarted;
        /// <summary>
        /// Occurs when this controller starts a drag drop operation.
        /// </summary>
        public event EventHandler DDEnded;

        //A list of UIElement that this DDController listens to for events
        private readonly List<UIElement> _uiElements;

        private readonly IDDProcessor _processor;

        //The object returned from the IDDProcessor as the data for the drag drop operation
        private object _dragDropObject;
        //A flag used to indicate that the mouse was pressed, and move events should check to see if 
        //a drag drop operation should be started
        private bool _doDragDrop;
        //The object used to display UI feedback to the user during a drag drop operation
        private DDFeedback _feedbackObject;
        //A flag indicating if this DDController has been started
        private bool _started;
        //The point where the mouse was pressed on one of the UIElements
        private Point _mouseDownPoint;

        /// <summary>
        /// Gets or sets the drag drop effects.
        /// </summary>
        /// <value>The drag drop effects.</value>
        public DragDropEffects DragDropEffects { get; set; }

        /// <summary>
        /// Gets or sets the minimum distance that an item must be dragged 
        /// horizontally before the drag drop operation is started.
        /// </summary>
        /// <value>The minimum horizontal drag distance.</value>
        public double MinimumHorizontalDragDistance { get; set; }
        /// <summary>
        /// Gets or sets the minimum distance that an item must be dragged 
        /// vertically before the drag drop operation is started.
        /// </summary>
        /// <value>The minimum vertical drag distance.</value>
        public double MinimumVerticalDragDistance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the controller cache 
        /// the result from <see cref="IDDProcessor"/>.GetDragDropFeedback(). 
        /// This means that for each drag drop operation 
        /// <see cref="IDDProcessor"/>.GetDragDropFeedback will only
        /// be called once per drag drop operation. Default is <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if the controller should use static feedback;
        /// otherwise, <c>false</c>.</value>
        public bool CacheFeedback { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DDController" /> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="elements">The elements.</param>
        public DDController(IDDProcessor processor, params UIElement[] elements)
        {
            _uiElements = new List<UIElement>();
            if (elements != null)
                AddElements(elements);

            _processor = processor;

            DragDropEffects = DDGlobalDefaults.ControllerDragDropEffects;
            MinimumHorizontalDragDistance = DDGlobalDefaults.ControllerMinimumHorizontalDragDistance;
            MinimumVerticalDragDistance = DDGlobalDefaults.ControllerMinimumVerticalDragDistance;
            CacheFeedback = DDGlobalDefaults.ControllerCacheFeedback;
        }

        /// <summary>
        /// Starts this <see cref="DDController"/> listening for events.
        /// </summary>
        public void Start()
        {
            if (_started == false)
            {
                _uiElements.ForEach(RegisterEventHandlers);
                _started = true;
            }
        }

        /// <summary>
        /// Stops this <see cref="DDController"/> from listening for events.
        /// </summary>
        public void Stop()
        {
            if (_started)
            {
                _uiElements.ForEach(UnRegisterEventHandlers);
                _started = false;
            }
        }

        /// <summary>
        /// Add additional UIElements to this <see cref="DDController"/>.
        /// </summary>
        /// <param name="uiElements"></param>
        public void AddElements(params UIElement[] uiElements)
        {
            if (uiElements == null) throw new ArgumentNullException("uiElements");
            foreach (var element in uiElements)
            {
                if (_started)
                    RegisterEventHandlers(element);
                _uiElements.Add(element);
            }
        }

        /// <summary>
        /// Removes UIElements from this <see cref="DDController"/>
        /// </summary>
        /// <param name="uiElements"></param>
        public void RemoveElements(params UIElement[] uiElements)
        {
            if (uiElements == null) throw new ArgumentNullException("uiElements");
            foreach (var element in uiElements)
            {
                if (_uiElements.Remove(element))
                {
                    if (_started)
                        UnRegisterEventHandlers(element);
                }
            }
        }

        private void UIElementMouseLeave(object sender, EventArgs e)
        {
            _doDragDrop = false;
        }

        private void UIElementMouseMove(object sender, MouseEventArgs e)
        {
            if (_doDragDrop)
            {
                //If the mouse has previously been pressed, determine if the mouse has moved far enough to start a drag drop operation
                var inputElement = sender as IInputElement;
                if (inputElement == null) return;

                var location = e.GetPosition(inputElement);

                //Calculate the distance the mouse has moved since it was originally pressed
                var yDelta = Math.Abs(location.Y - _mouseDownPoint.Y);
                var xDelta = Math.Abs(location.X - _mouseDownPoint.X);

                if (xDelta < MinimumHorizontalDragDistance && yDelta < MinimumVerticalDragDistance)
                    return;

                //The distance dragged is far enough, invoke the processor to see if we should start the drag drop operation
                _doDragDrop = false;
                _dragDropObject = _processor.GetDragDropObject(sender, e);
                var dependancyObject = sender as DependencyObject;
                if (_dragDropObject != null && dependancyObject != null)
                {
                    //Raise the appropriate events and start the drag drop operation
                    var @event = DDStarted;
                    if (@event != null)
                        @event(this, EventArgs.Empty);

                    DragDrop.DoDragDrop(dependancyObject, _dragDropObject, DragDropEffects); //this call blocks execution

                    @event = DDEnded;
                    if (@event != null)
                        @event(this, EventArgs.Empty);

                    _dragDropObject = null;
                    //Cleanup any UI feedback that was presented
                    if (_feedbackObject != null)
                    {
                        _feedbackObject.HideFeedback();
                        _feedbackObject = null;
                    }
                }
            }
        }

        private void UIElementMouseUp(object sender, MouseEventArgs e)
        {
            _doDragDrop = false;
        }

        private void UIElementMouseDown(object sender, MouseEventArgs e)
        {
            //If the user presses down the left mouse button, set the flags indicating 
            //that a drag drop operation may be starting
            _doDragDrop = e.LeftButton == MouseButtonState.Pressed;
            if (_doDragDrop)
            {
                var inputElement = sender as IInputElement;
                if (inputElement != null)
                    _mouseDownPoint = e.GetPosition(inputElement);
            }
        }

        private void UIElementGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            var oldFeedback = _feedbackObject;
            //Get any UI feedback from the processor if necessary.
            if (CacheFeedback == false || _feedbackObject == null)
                _feedbackObject = _processor.GetDragDropFeedback(e, _dragDropObject);
            if (_feedbackObject != null)
            {
                //If the feedback has changed we want to hide the old one
                if (oldFeedback != null && ReferenceEquals(oldFeedback, _feedbackObject) == false)
                    oldFeedback.HideFeedback();
                
                var currentLoc = MouseUtil.CurrentMousePoint();
                _feedbackObject.ShowFeedback(currentLoc);

                //If we want to hide the default cursors, mark this event as handled
                if (_feedbackObject.ShowDefaultCursors == false)
                    e.Handled = true;
            }
        }

        private void RegisterEventHandlers(UIElement element)
        {
            //Register for the UI events
            element.PreviewMouseDown += UIElementMouseDown;
            element.PreviewMouseUp += UIElementMouseUp;
            element.PreviewMouseMove += UIElementMouseMove;
            element.MouseLeave += UIElementMouseLeave;
            element.PreviewGiveFeedback += UIElementGiveFeedback;
        }

        private void UnRegisterEventHandlers(UIElement element)
        {
            //Un-register for the UI events
            element.PreviewMouseDown -= UIElementMouseDown;
            element.PreviewMouseUp -= UIElementMouseUp;
            element.PreviewMouseMove -= UIElementMouseMove;
            element.MouseLeave -= UIElementMouseLeave;
            element.PreviewGiveFeedback -= UIElementGiveFeedback;
        }
    }
}
