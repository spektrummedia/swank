using Plugin.Swank.Panorama.ImageSources;
using Xamarin.Forms;

namespace Plugin.Swank.Panorama
{
    public class PanoramaView : ContentView
    {
        public PanoramaImageSource Image
        {
            get => (PanoramaImageSource) GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public float FieldOfView
        {
            get => (float) GetValue(FieldOfViewProperty);
            set => SetValue(FieldOfViewProperty, value);
        }

        public float Yaw
        {
            get => (float) GetValue(YawProperty);
            set => SetValue(YawProperty, value);
        }

        public float Pitch
        {
            get => (float) GetValue(PitchProperty);
            set => SetValue(PitchProperty, value);
        }

        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(PanoramaImageSource), typeof(PanoramaView), null,
                BindingMode.OneWay, null, (s, oldValue, newValue) => (s as PanoramaView).ImagePropertyChanged());

        public static readonly BindableProperty FieldOfViewProperty =
            BindableProperty.Create(nameof(FieldOfView), typeof(float), typeof(PanoramaView), 0.0f, BindingMode.OneWay,
                null, (s, oldValue, newValue) => (s as PanoramaView).FieldOfViewPropertyChanged());

        public static readonly BindableProperty YawProperty =
            BindableProperty.Create(nameof(Yaw), typeof(float), typeof(PanoramaView), 0.0f, BindingMode.OneWay, null,
                (s, oldValue, newValue) => (s as PanoramaView).YawPropertyChanged());

        public static readonly BindableProperty PitchProperty =
            BindableProperty.Create(nameof(Pitch), typeof(float), typeof(PanoramaView), 0.0f, BindingMode.OneWay, null,
                (s, oldValue, newValue) => (s as PanoramaView).PitchPropertyChanged());

        private readonly PanoramaController _panoramaControl;

        public PanoramaView()
        {
            _panoramaControl = new PanoramaController();
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            Content = _panoramaControl.GetView();

            _panoramaControl.Initialize();
        }

        private void ImagePropertyChanged()
        {
            _panoramaControl.SetImage(Image);
        }

        private void FieldOfViewPropertyChanged()
        {
            _panoramaControl.SetFieldOfView(FieldOfView);
        }

        private void YawPropertyChanged()
        {
            _panoramaControl.SetYaw(Yaw);
        }

        private void PitchPropertyChanged()
        {
            _panoramaControl.SetPitch(Pitch);
        }
    }
}