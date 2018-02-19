using Xamarin.Forms;

namespace Swank.FormsPlugin.Abstractions
{
    public class ViewerImageTemplate : ContentView
    {
        public ViewerImageTemplate()
        {
            var layout = new StackLayout();
            var image = new Image
            {
                Aspect = Aspect.AspectFill
            };
            image.SetBinding(Image.SourceProperty, nameof(ViewerImage.Source));
            layout.Children.Add(image);
            Content = layout;
        }
    }
}