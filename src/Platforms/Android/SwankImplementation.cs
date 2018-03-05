using CarouselView.FormsPlugin.Android;

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