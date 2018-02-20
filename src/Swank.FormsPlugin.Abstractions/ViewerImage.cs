using Xamarin.Forms;

namespace Swank.FormsPlugin.Abstractions
{
    public class ViewerImage : ObservableObject
    {
        public ImageSource Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public bool Is360
        {
            get => _is360;
            set => SetProperty(ref _is360, value);
        }

        private bool _is360;

        private ImageSource _source;
    }
}