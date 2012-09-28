//Written by Kevin Bost, http://dotnetgeek.tumblr.com/, kitokeboo@yahoo.com

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace DDLibWPF
{
    public sealed class DDFeedback
    {
        /// <summary>
        /// Creates a new <see cref="DDFeedback"/> object with the specified text 
        /// and optional foreground and background colors
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="background">The background color</param>
        /// <param name="forground">The foreground color</param>
        /// <returns>A new <see cref="DDFeedback"/> object displaying text</returns>
        public static DDFeedback CreateFromText(string text, Color? background = null, Color? forground = null)
        {
            var rv = new DDFeedback();
            var toolTip = rv._toolTip;
            toolTip.Content = text;
            if (background != null)
                toolTip.Background = new SolidColorBrush(background.Value);
            if (forground != null)
                toolTip.Foreground = new SolidColorBrush(forground.Value);
            return rv;
        }

        /// <summary>
        /// Creates a new <see cref="DDFeedback"/> object displaying a 
        /// <see cref="System.Windows.Media.Visual"/> in its current state.
        /// </summary>
        /// <param name="visual">The visual object to take a snapshot of</param>
        /// <param name="size">The size to use for the displayed feedback</param>
        /// <returns>A new <see cref="DDFeedback"/> object displaying a snapshot of a 
        /// <see cref="System.Windows.Media.Visual"/></returns>
        public static DDFeedback CreateFromVisual(Visual visual, Size size)
        {
            var rv = new DDFeedback();

            var rtb = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);

            var dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                var brush = new VisualBrush(visual);
                dc.DrawRectangle(brush, null, new Rect(new Point(), size));
            }

            rtb.Render(dv);

            var toolTip = rv._toolTip;
            toolTip.Padding = new Thickness(0);
            toolTip.Content = new Image { Source = rtb };

            return rv;
        }

        private readonly ToolTip _toolTip;

        /// <summary>
        /// Gets or sets a value indicating if if the default drag and drop cursors should be shown
        /// while displaying this <see cref="DDFeedback"/>.
        /// </summary>
        public bool ShowDefaultCursors { get; set; }

        /// <summary>
        /// Gets or sets the horizontal offset of the mouse cursor over this <see cref="DDFeedback"/>.
        /// </summary>
        public double HorizontalCursorOffset { get; set; }

        /// <summary>
        /// Gets or sets the vertical offset of the mouse cursor over this <see cref="DDFeedback"/>.
        /// </summary>
        public double VerticalCursorOffset { get; set; }

        #region Tooltip Properties

        /// <summary>
        /// Gets or sets a brush that describes the background of this <see cref="DDFeedback"/>.
        /// </summary>
        public Brush Background
        {
            get { return _toolTip.Background; }
            set { _toolTip.Background = value; }
        }

        /// <summary>
        /// Gets or sets a brush that describes the border background of this <see cref="DDFeedback"/>. 
        /// </summary>
        public Brush BorderBrush
        {
            get { return _toolTip.BorderBrush; }
            set { _toolTip.BorderBrush = value; }
        }

        /// <summary>
        /// Gets or sets the border thickness of this <see cref="DDFeedback"/>.
        /// </summary>
        public Thickness BorderThickness
        {
            get { return _toolTip.BorderThickness; }
            set { _toolTip.BorderThickness = value; }
        }

        /// <summary>
        /// Gets or sets the font family of this <see cref="DDFeedback"/>. 
        /// </summary>
        public FontFamily FontFamily
        {
            get { return _toolTip.FontFamily; }
            set { _toolTip.FontFamily = value; }
        }

        /// <summary>
        /// Gets or sets the font of this <see cref="DDFeedback"/>.
        /// </summary>
        public double FontSize
        {
            get { return _toolTip.FontSize; }
            set { _toolTip.FontSize = value; }
        }

        /// <summary>
        /// Gets or sets the degree to which a font is condensed or expanded on the screen. 
        /// </summary>
        public FontStretch FontStretch
        {
            get { return _toolTip.FontStretch; }
            set { _toolTip.FontStretch = value; }
        }

        /// <summary>
        /// Gets or sets the font style of this <see cref="DDFeedback"/>. 
        /// </summary>
        public FontStyle FontStyle
        {
            get { return _toolTip.FontStyle; }
            set { _toolTip.FontStyle = value; }
        }

        /// <summary>
        /// Gets or sets the weight or thickness of the specified font. 
        /// </summary>
        public FontWeight FontWeight
        {
            get { return _toolTip.FontWeight; }
            set { _toolTip.FontWeight = value; }
        }

        /// <summary>
        /// Gets or sets a brush that describes the foreground color of this <see cref="DDFeedback"/>.
        /// </summary>
        public Brush Foreground
        {
            get { return _toolTip.Foreground; }
            set { _toolTip.Foreground = value; }
        }

        /// <summary>
        /// Gets or sets the padding inside this <see cref="DDFeedback"/>.
        /// </summary>
        public Thickness Padding
        {
            get { return _toolTip.Padding; }
            set { _toolTip.Padding = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether this <see cref="DDFeedback"/> has a drop shadow.
        /// </summary>
        public bool ShowDropShadow
        {
            get { return _toolTip.HasDropShadow; }
            set { _toolTip.HasDropShadow = value; }
        }

        #endregion Tooltip Properties

        private DDFeedback()
        {
            _toolTip = new ToolTip { Placement = PlacementMode.Relative };

            HorizontalCursorOffset = DDGlobalDefaults.FeedbackHorizontalCursorOffset;
            VerticalCursorOffset = DDGlobalDefaults.FeedbackVerticalCursorOffset;

            ShowDefaultCursors = DDGlobalDefaults.FeedbackShowDefaultCursors;
        }

        internal void ShowFeedback(Point screenPoint)
        {
            _toolTip.IsOpen = true;
            _toolTip.HorizontalOffset = HorizontalCursorOffset + screenPoint.X;
            _toolTip.VerticalOffset = VerticalCursorOffset + screenPoint.Y;
        }

        internal void HideFeedback()
        {
            _toolTip.IsOpen = false;
        }
    }
}
