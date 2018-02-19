using Xamarin.Forms;

namespace Swank.FormsPlugin.Abstractions
{
    public class ViewerImageTemplate : ContentView
    {
        public ViewerImageTemplate()
        {
            var layout = new StackLayout()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            var image = new Image
            {
                Aspect = Aspect.AspectFit
            };
            image.SetBinding(Image.SourceProperty, nameof(ViewerImage.Source));
            layout.Children.Add(image);
            Content = layout;
        }
    }
}