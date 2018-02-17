using Xamarin.Forms;

namespace Swank.FormsPlugin.Abstractions
{
    public class Viewer : ScrollView
    {
        public Viewer()
        {
            BackgroundColor = Color.Red;

            // Size
            Orientation = ScrollOrientation.Horizontal;
            Content = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(0),
                Margin = new Thickness(0),
                Spacing = 0
            };
        }
    }
}