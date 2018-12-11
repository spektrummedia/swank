using System;
using System.Collections;
using CarouselView.FormsPlugin.Abstractions;
using Plugin.Swank.Panorama.ImageSources;
using Swank.FormsPlugin;
using Xamarin.Forms;

namespace Plugin.Swank
{
    public class Viewer : CarouselViewControl
    {
        private Gallery Gallery => Parent?.Parent as Gallery;

        public new IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
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

        public void ToggleIsSwipeEnabled(string filePath)
        {
            IsSwipeEnabled = !IsSwipeEnabled;
            Gallery?.TogglePanoramaVisibility();

            if (!string.IsNullOrEmpty(filePath))
            {
                var image = filePath.Contains("http")
                    ? new PanoramaUriImageSource(new Uri(filePath))
                    : (PanoramaImageSource)new PanoramaFileSystemImageSource(filePath);
                Gallery?.SetPanoramaImage(image);
            }
        }
    }
}