using System;
using System.Linq;
using Xamarin.Forms;

namespace Plugin.Swank
{
    public class ViewerImageTemplate : ContentView
    {
        private Viewer Viewer => Parent as Viewer;
        public int width => 4000;
        public int height => 2000;

        private readonly Image _image = new Image
        {
            Aspect = Aspect.AspectFill,
            VerticalOptions = LayoutOptions.CenterAndExpand
        };

        private readonly Layout<View> _stackLayout = new AbsoluteLayout()
        {
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.CenterAndExpand
        };

        private Size ImageSize;

        private double x, y;

        public ViewerImageTemplate()
        {
            var currentImage = BindingContext as ViewerImage;
            _image.SetBinding(Image.SourceProperty, nameof(ViewerImage.Source));

            AbsoluteLayout.SetLayoutBounds(_image, new Rectangle(.5, .5, 1, 1));
            AbsoluteLayout.SetLayoutFlags(_image, AbsoluteLayoutFlags.All);

            _stackLayout.Children.Add(_image);
            Content = _stackLayout;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var image = BindingContext as ViewerImage;
            //if (image != null && image.Is360)
            //{
            //    Create360Controls(image);
            //}
        }

        private void Create360Controls(ViewerImage image)
        {
            // Lock-in/lock-out
            var isBlockedSwitch = new Switch();
            isBlockedSwitch.IsToggled = false;
            isBlockedSwitch.Toggled += IsBlockedSwitchOnToggled;

            // Add components to stacklayout
            _stackLayout.Children.Add(isBlockedSwitch);
        }

        private void OnDrag(object sender, PanUpdatedEventArgs e)
        {
            Console.WriteLine($"OnDrag X:{e.TotalX} Y:{e.TotalY}");

            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    Content.TranslationX =
                        Math.Max(Math.Min(0, x + e.TotalX), -Math.Abs(Content.Width - width));
                    Content.TranslationY =
                        Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs(Content.Height - height));
                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    x = Content.TranslationX;
                    y = Content.TranslationY;
                    break;
            }
        }

        private void IsBlockedSwitchOnToggled(object sender, ToggledEventArgs toggledEventArgs)
        {
            Viewer.SetIsSwipeEnabled(toggledEventArgs.Value);
            if (toggledEventArgs.Value)
            {
                var panGesture = new PanGestureRecognizer();
                panGesture.PanUpdated += OnDrag;
                _stackLayout.GestureRecognizers.Add(panGesture);
                _image.Aspect = Aspect.Fill;
                _stackLayout.Scale = 2;
                Content.TranslationX = width / 2;
                Content.TranslationY = height / 2;
            }
            else if (_stackLayout.GestureRecognizers.Any())
            {
                foreach (var gesture in _stackLayout.GestureRecognizers.ToArray())
                {
                    _stackLayout.GestureRecognizers.Remove(gesture);
                }
            }
        }
    }
}