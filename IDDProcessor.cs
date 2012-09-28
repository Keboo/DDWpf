//Written by Kevin Bost, http://dotnetgeek.tumblr.com/, kitokeboo@yahoo.com
using System.Windows;
using System.Windows.Input;

namespace DDLibWPF
{
    public interface IDDProcessor
    {
        /// <summary>
        /// Gets the drag-drop data object, or <c>null</c> if no drag-drop operation should be performed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        /// <returns>The object to be dragged or <c>null</c> if no drag drop should be performed.</returns>
        object GetDragDropObject(object sender, MouseEventArgs e);

        /// <summary>
        /// Gets the drag drop feedback. Return <c>null</c> to use default
        /// cursors.
        /// </summary>
        /// <param name="e">The 
        /// <see cref="System.Windows.GiveFeedbackEventArgs"/> instance
        /// containing the event data.</param>
        /// <param name="dragDropObject">The object being dragged.</param>
        /// <returns>The drag drop feedback object used to build a custom cursor
        /// or null if no custom cursor should be used.</returns>
        DDFeedback GetDragDropFeedback(GiveFeedbackEventArgs e, object dragDropObject);
    }
}
