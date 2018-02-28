using CarouselView.FormsPlugin.iOS;

namespace Plugin.Swank
{
    public class SwankImplementation : ISwank
    {
        public static void Init()
        {
            CarouselViewRenderer.Init();
        }
    }
}