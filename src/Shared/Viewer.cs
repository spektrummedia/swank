using System.Collections;
using CarouselView.FormsPlugin.Abstractions;
using Xamarin.Forms;

namespace Plugin.Swank
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
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            Orientation = CarouselViewOrientation.Horizontal;
            BackgroundColor = Color.Black;
            ShowIndicators = true;
            ItemTemplate = new DataTemplate(typeof(ViewerImageTemplate));
        }

        public void SetIsSwipeEnabled(bool isDisabled)
        {
            IsSwipeEnabled = isDisabled;
        }
    }
}