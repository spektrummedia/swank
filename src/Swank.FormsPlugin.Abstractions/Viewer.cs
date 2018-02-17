using Xamarin.Forms;

namespace Swank.FormsPlugin.Abstractions
{
    public class Viewer : ScrollView
    {
        public Viewer()
        {
            Content = new StackLayout();
        }
    }
}