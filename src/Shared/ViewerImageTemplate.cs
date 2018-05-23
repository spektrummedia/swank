using System;
using System.Linq;
using Plugin.Swank.Panorama;
using Plugin.Swank.Panorama.ImageSources;
using Xamarin.Forms;

namespace Plugin.Swank
{
    public class ViewerImageTemplate : ContentView
    {
        private Viewer Viewer => Parent as Viewer;

        private readonly Image _image = new Image
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand,
            Aspect = Aspect.AspectFit
        };

        private readonly PanGestureRecognizer _pan = new PanGestureRecognizer();

        private readonly Layout<View> _stackLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand,
            Orientation = StackOrientation.Vertical,
            BackgroundColor = Color.Black
        };

        private PanoramaView _panorama;
        private StackLayout _panoramaLayout;

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

        private void Create360Controls(ViewerImage image)
        {
            _panorama = new PanoramaView
            {
                FieldOfView = 75.0f,
                Yaw = 0,
                Pitch = 0,
                HeightRequest = _panoramaLayout.HeightRequest,
                WidthRequest = _panoramaLayout.WidthRequest,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                InputTransparent = true
            };

            // Lock-in/lock-out
            var immersionSwitch = new Switch
            {
                IsToggled = false,
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(0, -3, 0, 0)
            };
            immersionSwitch.Toggled += IsBlockedSwitchOnToggled;

            // Label
            var immersionSwitchText = new Label
            {
                Text = (BindingContext as ViewerImage).Toggle360ModeText,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontSize = 22,
                Margin = new Thickness(0, 0, 5, 0)
            };

            // Add components to stacklayout
            _stackLayout.Children.Insert(1, new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {immersionSwitchText, immersionSwitch}
            });
        }

        private void IsBlockedSwitchOnToggled(object sender, ToggledEventArgs toggledEventArgs)
        {
            if (toggledEventArgs.Value)
            {
                _pan.PanUpdated += OnPanUpdated;
                _panoramaLayout = new StackLayout
                {
                    HeightRequest = _image.Width / 2,
                    WidthRequest = _image.Width,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    GestureRecognizers = {_pan},
                    BackgroundColor = Color.Black
                };

                var filePath = (BindingContext as ViewerImage).FilePath;

                _panorama.Image = filePath.Contains("http")
                    ? new PanoramaUriImageSource(new Uri(filePath))
                    : (PanoramaImageSource) new PanoramaFileSystemImageSource(filePath);

                _panoramaLayout.Children.Add(_panorama);
                _stackLayout.Children.Insert(0, _panoramaLayout);
                _stackLayout.Children.Remove(_image);
            }
            else
            {
                if (_panoramaLayout.GestureRecognizers.Any())
                {
                    _pan.PanUpdated -= OnPanUpdated;
                    foreach (var gesture in _panoramaLayout.GestureRecognizers.ToArray())
                    {
                        _panoramaLayout.GestureRecognizers.Remove(gesture);
                    }
                }

                _panoramaLayout.Children.Remove(_panorama);
                _stackLayout.Children.Remove(_panoramaLayout);
                _stackLayout.Children.Insert(0, _image);
            }

            Viewer.ToggleIsSwipeEnabled();
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    if (_panorama != null)
                    {
                        ComputeYaw(e.TotalX);
                        ComputePitch(e.TotalY);
                    }

                    break;
            }
        }

        private void ComputeYaw(double totalX)
        {
            if (totalX == 0)
            {
                return;
            }

            _panorama.Yaw += (float) (totalX / 100);
        }

        private void ComputePitch(double totalY)
        {
            if (totalY == 0)
            {
                return;
            }

            var newPitch = _panorama.Pitch + (float) (totalY / 100);
            if (newPitch <= 40 && newPitch >= -40)
            {
                _panorama.Pitch = newPitch;
            }
        }
    }
}