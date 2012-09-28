//Written by Kevin Bost, http://dotnetgeek.tumblr.com/, kitokeboo@yahoo.com

using System.Windows;

namespace DDLibWPF
{
    /// <summary>
    /// This class contains many default values for the drag drop classes in this library. 
    /// When a new class is instantiated it will uses the values stored here as its defaults.
    /// Set the values to make global changes to the drag drop behavior.
    /// </summary>
    public static class DDGlobalDefaults
    {
        static DDGlobalDefaults()
        {
            Reset();
        }

        /// <summary>
        /// Resets all settings to their default values.
        /// </summary>
        public static void Reset()
        {
            //Explicitly setting default values
            ReceiverUseDragOverEvents = false;
            ReceiverForceAllowDrop = false;

            ControllerDragDropEffects = DragDropEffects.All;
            ControllerMinimumVerticalDragDistance = SystemParameters.MinimumVerticalDragDistance;
            ControllerMinimumHorizontalDragDistance = SystemParameters.MinimumHorizontalDragDistance;
            ControllerCacheFeedback = true;

            FeedbackShowDefaultCursors = true;
            FeedbackHorizontalCursorOffset = 0;
            FeedbackVerticalCursorOffset = 0;
        }

        /// <summary>
        /// The initial value for the <see cref="DDReceiver"/>.UseDragOverEvents
        /// property. Default is <c>false</c>.
        /// </summary>
        public static bool ReceiverUseDragOverEvents { get; set; }

        /// <summary>
        /// The initial value for the <see cref="DDReceiver"/>.ForceAllowDrop
        /// property. Default is <c>false</c>.
        /// </summary>
        public static bool ReceiverForceAllowDrop { get; set; }

        /// <summary>
        /// The initial value for the <see cref="DDController"/>.
        /// <see cref="DragDropEffects"/> property. Default is 
        /// <see cref="DragDropEffects"/>.All.
        /// </summary>
        public static DragDropEffects ControllerDragDropEffects { get; set; }

        /// <summary>
        /// The initial value for the <see cref="DDController"/>
        /// .MinimumHorizontalDragDistance property. Default is 
        /// <see cref="SystemParameters"/>.MinimumHorizontalDragDistance.
        /// </summary>
        public static double ControllerMinimumHorizontalDragDistance { get; set; }

        /// <summary>
        /// The initial value for the <see cref="DDController"/>
        /// .MinimumVerticalDragDistance property. Default is 
        /// <see cref="SystemParameters"/>.MinimumVerticalDragDistance.
        /// </summary>
        public static double ControllerMinimumVerticalDragDistance { get; set; }

        /// <summary>
        /// The initial value for the <see cref="DDController"/>
        /// .CacheFeedback property. Default is <c>true</c>.
        /// </summary>
        public static bool ControllerCacheFeedback { get; set; }

        /// <summary>
        /// The initial value for the <see cref="DDFeedback"/>
        /// .ShowDefaultCursors property. Default is <c>true</c>.
        /// </summary>
        public static bool FeedbackShowDefaultCursors { get; set; }

        /// <summary>
        /// The initial value for the <see cref="DDFeedback"/>
        /// .HorizontalCursorOffset property. Default is <c>0.0</c>.
        /// </summary>
        public static double FeedbackHorizontalCursorOffset { get; set; }

        /// <summary>
        /// The initial value for the <see cref="DDFeedback"/>
        /// .VerticalCursorOffset property. Default is <c>0.0</c>.
        /// </summary>
        public static double FeedbackVerticalCursorOffset { get; set; }
    }
}
