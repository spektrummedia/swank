using Xamarin.Forms;

namespace Plugin.Swank
{
    public class ViewerImage : ObservableObject
    {
        private ImageSource _source;
        public ImageSource Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        private bool _is360;
        public bool Is360
        {
            get => _is360;
            set => SetProperty(ref _is360, value);
        }

        private string _toggle360ModeText;
        public string Toggle360ModeText
        {
            get => _toggle360ModeText;
            set => SetProperty(ref _toggle360ModeText, value);
        }
    }
}