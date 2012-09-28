//Written by Kevin Bost, http://dotnetgeek.tumblr.com/, kitokeboo@yahoo.com

using System;
using System.Windows;
using System.Windows.Markup;

namespace DDLibWPF.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(DDSource))]
    public sealed class DDSource : MarkupExtension, IDDProcessor
    {
        private DDController _controller;
        private UIElement _element;
        public object DragObject { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        internal void StartOn(UIElement element)
        {
            Stop();
            _element = element;
            _controller = new DDController(this, element);
            _controller.Start();
        }

        internal void Stop()
        {
            _element = null;
            if (_controller != null)
                _controller.Stop();
        }

        public object GetDragDropObject(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_element != null)
            {
                var value = AttachedProperties.GetDragObject(_element);
                var accessor = value as Func<object>;
                if (accessor != null)
                    return accessor();
                return value;
            }
            return null;
        }

        public DDFeedback GetDragDropFeedback(GiveFeedbackEventArgs e, object dragDropObject)
        {
            return null;
        }
    }
}
