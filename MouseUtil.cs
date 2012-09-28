//Written by Kevin Bost, http://dotnetgeek.tumblr.com/, kitokeboo@yahoo.com
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace DDLibWPF
{
    /// <summary>
    /// Class used ot get the current location of the mouse.
    /// See http://www.pinvoke.net/default.aspx/user32.getcursorpos
    /// </summary>
    internal class MouseUtil
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct MousePoint
        {
            public Int32 X;
            public Int32 Y;
        };

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref MousePoint pt);

        /// <summary>
        /// Get the current location of the mouse relative to the screen.
        /// </summary>
        /// <returns></returns>
        public static Point CurrentMousePoint()
        {
            var mouse = new MousePoint();
            GetCursorPos(ref mouse);
            return new Point(mouse.X, mouse.Y);
        }
    }
}
