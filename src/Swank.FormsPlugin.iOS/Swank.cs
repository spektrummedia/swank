using System;
using CarouselView.FormsPlugin.iOS;

namespace Swank.FormsPlugin.iOS
{
    /// <summary>
    /// Instance registration class
    /// </summary>
    public static class Swank
    {
        /// <summary>
        /// Init this instance
        /// </summary>
        public static void Init()
        {
            CarouselViewRenderer.Init();
        }
    }
}