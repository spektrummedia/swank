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

        private WeakReference<PanoramaController> _panoramaControl;
        public PanoramaController PanoramaControl
        {
            get
            {
                PanoramaController control = null;
                _panoramaControl.TryGetTarget(out control);
                return control;
            }
            set => _panoramaControl = new WeakReference<PanoramaController>(value);
        }

        public PanoramaView()
        {
            PanoramaControl = new PanoramaController();
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (PanoramaControl != null)
            {
                Content = PanoramaControl.GetView();
                PanoramaControl.Initialize();
            }
        }

        private void ImagePropertyChanged()
        {
            PanoramaControl.SetImage(Image);
        }

        private void FieldOfViewPropertyChanged()
        {
            PanoramaControl.SetFieldOfView(FieldOfView);
        }

        private void YawPropertyChanged()
        {
            PanoramaControl.SetYaw(Yaw);
        }

        private void PitchPropertyChanged()
        {
            PanoramaControl.SetPitch(Pitch);
        }

        public void Dispose()
        {
            PanoramaControl?.Dispose();
        }
    }
}