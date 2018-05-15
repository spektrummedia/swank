using System;
using Plugin.Swank.Panorama.ImageSources;
using Xamarin.Forms;

namespace Plugin.Swank.Panorama
{
    public class PanoramaView : ContentView, IDisposable
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

        private PanoramaController _paranoramaController { get; set; }

        public PanoramaView()
        {
            _paranoramaController = new PanoramaController();
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (_paranoramaController != null)
            {
                Content = _paranoramaController.GetView();
                _paranoramaController.Initialize();
            }
        }

        private void ImagePropertyChanged()
        {
            if (_paranoramaController != null)
            {
                _paranoramaController.SetImage(Image);
            }
        }

        private void FieldOfViewPropertyChanged()
        {
            if (_paranoramaController != null)
            {
                _paranoramaController.SetFieldOfView(FieldOfView);
            }
        }

        private void YawPropertyChanged()
        {
            if (_paranoramaController != null)
            {
                _paranoramaController.SetYaw(Yaw);
            }
        }

        private void PitchPropertyChanged()
        {
            if (_paranoramaController != null)
            {
                _paranoramaController.SetPitch(Pitch);
            }
        }

        public void Dispose()
        {
            _paranoramaController?.Dispose();
        }
    }
}