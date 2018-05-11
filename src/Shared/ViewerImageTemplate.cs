using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Plugin.Swank._360Controls;
using Xamarin.Forms;
using Switch = Xamarin.Forms.Switch;

namespace Plugin.Swank
{
    public class ViewerImageTemplate : ContentView
    {
        private Viewer Viewer => Parent as Viewer;
        private PanGestureRecognizer _pan = new PanGestureRecognizer();
        public int width => 4096;
        public int height => 2048;

        private PanoramaView _panorama;

        private readonly Image _image = new Image
        {
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        private readonly Layout<View> _stackLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Orientation = StackOrientation.Vertical
        };

        private double x, y = 0;
        private int scale = 2;

        public ViewerImageTemplate()
        {
            var currentImage = BindingContext as ViewerImage;
            _image.SetBinding(Image.SourceProperty, nameof(ViewerImage.Source));
            _stackLayout.Children.Add(_image);
            Content = _stackLayout;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var image = BindingContext as ViewerImage;
            if (image != null && image.Is360)
            {
                Create360Controls(image);
            }
        }

        private void CreatePanorama(ViewerImage image)
        {
        }

        private void Create360Controls(ViewerImage image)
        {
            // Lock-in/lock-out
            var isBlockedSwitch = new Switch();
            isBlockedSwitch.IsToggled = false;
            isBlockedSwitch.Toggled += IsBlockedSwitchOnToggled;

            // Add components to stacklayout
            _stackLayout.Children.Insert(0, isBlockedSwitch);
        }

        private void IsBlockedSwitchOnToggled(object sender, ToggledEventArgs toggledEventArgs)
        {
            Viewer.SetIsSwipeEnabled(toggledEventArgs.Value);
            if (toggledEventArgs.Value)
            {
                _image.IsVisible = false;
                _pan.PanUpdated += OnPanUpdated;

                _panorama = new PanoramaView()
                {
                    FieldOfView = 75.0f,
                    Image = new PanoramaUriImageSource(new Uri(
                        "https://d36tnp772eyphs.cloudfront.net/blogs/1/2006/11/360-panorama-matador-seo.jpg")),
                    Yaw = 0,
                    Pitch = 0,
                    BackgroundColor = Color.Aquamarine,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                _panorama.SetGestures(new[] { _pan });
                _stackLayout.Children.Add(_panorama);
            }
            else
            {
                _panorama.IsVisible = false;
                _image.IsVisible = true;

                if (_panorama.GestureRecognizers.Any())
                {
                    _pan.PanUpdated -= OnPanUpdated;
                    foreach (var gesture in _panorama.GestureRecognizers.ToArray())
                    {
                        _panorama.GestureRecognizers.Remove(gesture);
                    }
                }

                _stackLayout.Children.Remove(_panorama);
            }
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    var absWidth = -Math.Abs(Content.Width - (_image.Width * scale));
                    var minX = Math.Min(300, x + e.TotalX);
                    Content.TranslationX = Math.Max(minX, absWidth);
                    Console.WriteLine($"absHeight: {absWidth} minY: {minX} translationY: {Content.TranslationX}");

                    var absHeight = -Math.Abs(Content.Height - (_image.Height * scale));
                    var minY = Math.Min(300, y + e.TotalY);
                    Content.TranslationY = Math.Max(minY, absHeight);
                    Console.WriteLine($"absHeight: {absHeight} minY: {minY} translationY: {Content.TranslationY}");
                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    x = Content.TranslationX;
                    y = Content.TranslationY;
                    break;
            }
        }
    }
}