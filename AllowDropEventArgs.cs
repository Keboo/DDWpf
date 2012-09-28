//Written by Kevin Bost, http://dotnetgeek.tumblr.com/, kitokeboo@yahoo.com

using System;
using System.Windows;

namespace DDLibWPF
{
    /// <summary>
    /// The event arguments passed for the <see cref="DDReceiver"/>.AllowDrop event.
    /// </summary>
    public sealed class AllowDropEventArgs : EventArgs
    {
        private readonly object _draggedData;
        private readonly DragEventArgs _args;

        /// <summary>
        /// The data that was passed to the drag drop operation
        /// </summary>
        public object DraggedData
        {
            get { return _draggedData; }
        }

        /// <summary>
        /// Returns a drop point that is relative to a specified <see cref="System.Windows.IInputElement"/>.
        /// </summary>
        /// <param name="relativeTo">An <see cref="System.Windows.IInputElement"/> object for which to get a relative drop point.</param>
        /// <returns></returns>
        public Point GetPosition(IInputElement relativeTo)
        {
            return _args.GetPosition(relativeTo);
        }

        /// <summary>
        /// Set this to false to prevent a drop action from occurring.
        /// </summary>
        public bool AllowDrop { get; set; }

        internal AllowDropEventArgs(object draggedData, DragEventArgs args)
        {
            AllowDrop = true;
            _draggedData = draggedData;
            _args = args;
        }
    }
}
