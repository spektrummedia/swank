using System.Collections;
using CarouselView.FormsPlugin.Abstractions;
using Xamarin.Forms;

namespace Swank.FormsPlugin.Abstractions
{
    public class Viewer : CarouselViewControl
    {
        public new IEnumerable ItemsSource
        {
            get => (IEnumerable) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public Viewer()
        {
            Margin = 0;
            Position = 0;
            InterPageSpacing = 10;
            Orientation = CarouselViewOrientation.Horizontal;
            BackgroundColor = Color.Black;
            WidthRequest = 600;
            HeightRequest = 200;
            ShowArrows = true;
            ShowIndicators = true;
            ItemTemplate = new DataTemplate(typeof(ViewerImageTemplate));
        }
    }
}