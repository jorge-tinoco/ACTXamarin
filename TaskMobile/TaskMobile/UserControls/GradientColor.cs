using System;
using Xamarin.Forms;

namespace TaskMobile.UserControls
{
    /// <summary>
    /// User control for set gradient in stack layout container.
    /// </summary>
    public class GradientColor : StackLayout
    {
        /// <summary>
        /// Gradient start color.
        /// </summary>
        public Color StartColor { get; set; }

        /// <summary>
        /// Gradient end color.
        /// </summary>
        public Color EndColor { get; set; }
    }
}
