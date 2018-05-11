using System.Collections.Generic;
using Xamarin.Forms;

namespace Plugin.Swank._360Controls
{
    public class PanoramaView : ContentView
    {
        readonly PanoramaController _panoramaControl;

        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(PanoramaImageSource), typeof(PanoramaView), null, BindingMode.OneWay, null, propertyChanged: (s, oldValue, newValue) => (s as PanoramaView).ImagePropertyChanged());

        public static readonly BindableProperty FieldOfViewProperty =
            BindableProperty.Create(nameof(FieldOfView), typeof(float), typeof(PanoramaView), 0.0f, BindingMode.OneWay, null, propertyChanged: (s, oldValue, newValue) => (s as PanoramaView).FieldOfViewPropertyChanged());

        public static readonly BindableProperty YawProperty =
            BindableProperty.Create(nameof(Yaw), typeof(float), typeof(PanoramaView), 0.0f, BindingMode.OneWay, null, propertyChanged: (s, oldValue, newValue) => (s as PanoramaView).YawPropertyChanged());

        public static readonly BindableProperty PitchProperty =
            BindableProperty.Create(nameof(Pitch), typeof(float), typeof(PanoramaView), 0.0f, BindingMode.OneWay, null, propertyChanged: (s, oldValue, newValue) => (s as PanoramaView).PitchPropertyChanged());

        public PanoramaImageSource Image
        {
            get { return (PanoramaImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public float FieldOfView
        {
            get { return (float)GetValue(FieldOfViewProperty); }
            set { SetValue(FieldOfViewProperty, value); }
        }

        public float Yaw
        {
            get { return (float)GetValue(YawProperty); }
            set { SetValue(YawProperty, value); }
        }

        public float Pitch
        {
            get { return (float)GetValue(PitchProperty); }
            set { SetValue(PitchProperty, value); }
        }

        public PanoramaView()
        {
            _panoramaControl = new PanoramaController();
        }

        public void SetGestures(IList<IGestureRecognizer> gestures)
        {
            foreach (var gesture in gestures)
            {
                this.GestureRecognizers.Add(gesture);
            }
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            Content = _panoramaControl.GetView();

            _panoramaControl.Initialize();

            foreach (var gesture in GestureRecognizers)
            {
                _panoramaControl.SetGesture(gesture);
            }
        }

        void ImagePropertyChanged()
        {
            _panoramaControl.SetImage(Image);
        }

        void FieldOfViewPropertyChanged()
        {
            _panoramaControl.SetFieldOfView(FieldOfView);
        }

        void YawPropertyChanged()
        {
            _panoramaControl.SetYaw(Yaw);
        }

        void PitchPropertyChanged()
        {
            _panoramaControl.SetPitch(Pitch);
        }
    }
}