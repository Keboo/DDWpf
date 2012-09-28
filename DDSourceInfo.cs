//Written by Kevin Bost, http://dotnetgeek.tumblr.com/, kitokeboo@yahoo.com

using System.Windows;
namespace DDLibWPF
{
    public sealed class DDSourceArgs
    {
        private readonly UIElement _element;
        private readonly DragEventArgs _dragEventArgs;

        public UIElement UIElement
        {
            get { return _element; }
        }

        public DragEventArgs DragEventArgs
        {
            get { return _dragEventArgs; }
        }

        internal DDSourceArgs(UIElement element, DragEventArgs dragEventArgs)
        {
            _element = element;
            _dragEventArgs = dragEventArgs;
        }
    }
}
