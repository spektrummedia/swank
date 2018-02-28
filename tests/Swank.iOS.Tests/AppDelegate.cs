using Foundation;
using Plugin.Swank;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Swank.iOS.Tests
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            SwankImplementation.Init();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
    }
}


