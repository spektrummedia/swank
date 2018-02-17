using Xamarin.Forms;

namespace Swank.FormsPlugin.iOS.Tests
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            MainPage = new NavigationPage(new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(32, 32, 32, 32),
                    Children =
                    {
                        new Label {Text = "Swank - iOS"}
                    }
                }
            });
        }
    }
}