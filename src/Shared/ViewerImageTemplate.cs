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
            Aspect = Aspect.AspectFit,
            InputTransparent = true
        };

        private readonly Layout<View> _stackLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand,
            Orientation = StackOrientation.Vertical,
            BackgroundColor = Color.Black
        };

        public ViewerImageTemplate()
        {
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
                Children = { immersionSwitchText, immersionSwitch }
            });
        }

        private void IsBlockedSwitchOnToggled(object sender, ToggledEventArgs toggledEventArgs)
        {
            Viewer.ToggleIsSwipeEnabled((BindingContext as ViewerImage).FilePath);
        }
    }
}