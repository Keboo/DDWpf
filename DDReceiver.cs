//Written by Kevin Bost, http://dotnetgeek.tumblr.com/, kitokeboo@yahoo.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DDLibWPF
{
    public class DDReceiver
    {
        //Item dropped delegates used for callbacks when an item is dropped
        public delegate void ItemDropped(object data, DDSourceArgs args);
        public delegate void ItemDropped<in T>(T data, DDSourceArgs args);

        //A list of UIElement that this DDReceiver listens to for events
        private readonly List<UIElement> _uiElements;
        //The the callback delegate that should be invoked when a given type is dropped onto the UIElement
        private readonly IDictionary<Type, Delegate> _allowedTypes;

        //A flag indicating if this DDReceiver has been started
        private bool _started;

        /// <summary>
        /// Occurs on DragEnter over a control. This will also occur on DragOver
        /// events if the <see cref="DDReceiver.UseDragOverEvents"/> is set to <c>true</c>.
        /// Register for this event and set the <see cref="AllowDropEventArgs.AllowDrop"/> 
        /// property to false if the drag drop operation should be canceled.
        /// </summary>
        public event EventHandler<AllowDropEventArgs> AllowDrop;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="UIElement.AllowDrop"/> property
        /// will be set to true when this <see cref="DDReceiver"/> is started.
        /// </summary>
        public bool ForceAllowDrop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the AllowDrop event will be raised
        /// when <see cref="UIElement.DragOver"/> events occur on the <see cref="UIElement"/>s.
        /// </summary>
        public bool UseDragOverEvents { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DDReceiver" /> class.
        /// </summary>
        /// <param name="uiElements">The UI elements.</param>
        public DDReceiver(params UIElement[] uiElements)
        {
            _uiElements = new List<UIElement>();

            if (uiElements != null)
                AddElements(uiElements);

            _allowedTypes = new Dictionary<Type, Delegate>();

            ForceAllowDrop = DDGlobalDefaults.ReceiverForceAllowDrop;
            UseDragOverEvents = DDGlobalDefaults.ReceiverUseDragOverEvents;
        }

        /// <summary>
        /// Starts this <see cref="DDReceiver"/> and registers for all required events.
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
        /// Stops this receiver and un-registers for all required events.
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
        /// Add additional <see cref="UIElement"/>s to this <see cref="DDReceiver"/>.
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
        /// Removes the <see cref="UIElement"/>s from this <see cref="DDReceiver"/>.
        /// </summary>
        /// <param name="uiElements">The UI elements.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
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

        /// <summary>
        /// Allows a given type, and its children, to be dropped on the <see cref="UIElement"/>s.
        /// </summary>
        /// <typeparam name="T">The type to allow</typeparam>
        /// <param name="method">The callback method to invoke on a drop</param>
        public void AllowType<T>(ItemDropped<T> method)
        {
            _allowedTypes.Add(typeof(T), method);
        }

        /// <summary>
        /// Allow a given type, and its children, to be dropped on the <see cref="UIElement"/>s.
        /// </summary>
        /// <param name="type">The type to allow</param>
        /// <param name="method">The callback method to invoke on a drop</param>
        public void AllowType(Type type, ItemDropped method)
        {
            _allowedTypes.Add(type, method);
        }

        /// <summary>
        /// No longer allow a given type to be dropped on the <see cref="UIElement"/>s.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnAllowType<T>()
        {
            _allowedTypes.Remove(typeof(T));
        }

        /// <summary>
        /// Clears all of the allowed types from this <see cref="DDReceiver"/>.
        /// </summary>
        public void ClearAllAllowedTypes()
        {
            _allowedTypes.Clear();
        }

        private void UIElementDragDrop(object sender, DragEventArgs e)
        {
            var allowedType = FindAllowedType(e);
            if (allowedType != null)
            {
                object data = e.Data.GetData(allowedType);
                if (data != null) //This "should" always be true since the type is being derived from the DragEventArgs
                {
                    var method = _allowedTypes[allowedType];
                    var args = new DDSourceArgs(sender as UIElement, e);
                    method.DynamicInvoke(data, args);
                }
            }
        }

        private void UIElementDragEnter(object sender, DragEventArgs e)
        {
            HandleDraggedData(e);
        }

        private void UIElementDragOver(object sender, DragEventArgs e)
        {
            if (UseDragOverEvents)
                HandleDraggedData(e);
        }

        private void HandleDraggedData(DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.None; //disables the drag drop operation

            var allowedType = FindAllowedType(e);
            if (allowedType != null)
            {
                object data = e.Data.GetData(allowedType);
                if (data != null) //This "should" always be true
                {
                    if (AllowDrop != null)
                    {
                        var args = new AllowDropEventArgs(data, e);
                        AllowDrop(this, args);
                        if (args.AllowDrop == false)
                            return;
                    }
                    e.Effects = e.AllowedEffects;
                }
            }
        }

        private Type FindAllowedType(DragEventArgs e)
        {
            var typeStrings = e.Data.GetFormats();
            return (from typeString in typeStrings
                    from pair in _allowedTypes
                    let acceptableType = Type.GetType(typeString, false)
                            ?? Type.GetType(string.Format("{0}, {1}", typeString, pair.Key.Assembly.FullName))
                    where acceptableType != null && pair.Key.IsAssignableFrom(acceptableType)
                    select acceptableType).FirstOrDefault();
        }

        private void RegisterEventHandlers(UIElement element)
        {
            if (ForceAllowDrop)
                element.AllowDrop = true;
            if (element.AllowDrop == false)
                throw new InvalidOperationException("DDReceiver controls must set AllowDrop to true");
            element.DragEnter += UIElementDragEnter;
            element.Drop += UIElementDragDrop;
            element.DragOver += UIElementDragOver;
        }

        private void UnRegisterEventHandlers(UIElement element)
        {
            element.DragEnter -= UIElementDragEnter;
            element.Drop -= UIElementDragDrop;
            element.DragOver -= UIElementDragOver;
        }
    }
}
