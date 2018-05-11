using System.IO;
using Urho;
using Urho.Forms;
using Xamarin.Forms;

namespace Plugin.Swank._360Controls
{
    public class PanoramaController
    {
        PanoramaImageSource _imageSource;
        float _fieldOfView, _yaw, _pitch;
        UrhoSurface _urhoSurface;
        PanoramaUrhoApp _urhoApp;

        public Xamarin.Forms.View GetView()
        {
            if (_urhoSurface == null)
            {
                _urhoSurface = CreateView() as UrhoSurface;
            }

            return _urhoSurface;
        }

        public void SetImage(PanoramaImageSource imageSource)
        {
            if (_imageSource != imageSource)
            {
                _imageSource = imageSource;
                UpdateImage();
            }
        }

        public void SetFieldOfView(float fieldOfView)
        {
            if (_fieldOfView != fieldOfView)
            {
                _fieldOfView = fieldOfView;
                UpdateFieldOfView();
            }
        }

        public void SetYaw(float yaw)
        {
            if (_yaw != yaw)
            {
                _yaw = yaw;
                UpdateYaw();
            }
        }

        public void SetPitch(float pitch)
        {
            if (_pitch != pitch)
            {
                _pitch = pitch;
                UpdatePitch();
            }
        }

        public void Initialize()
        {
            // Enqueue the creation such that UrhoSharp renderers are initialized
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (_urhoApp == null)
                {
                    _urhoApp = await _urhoSurface.Show<PanoramaUrhoApp>(new Urho.ApplicationOptions(assetsFolder: null) // "Data"
                    {
                        Orientation = ApplicationOptions.OrientationType.LandscapeAndPortrait
                    });

                    UpdateImage();
                    UpdateFieldOfView();
                    UpdatePitch();
                    UpdateYaw();
                }
            });
        }

        Xamarin.Forms.View CreateView()
        {
            var urhoSurface = new UrhoSurface()
            {
                VerticalOptions = LayoutOptions.FillAndExpand
                //InputTransparent = true
            };

            return urhoSurface;
        }

        async void UpdateImage()
        {
            if (_imageSource == null || _urhoApp == null)
            {
                return;
            }

            Stream imageStream = await _imageSource.GetStreamAsync();

            if (imageStream == null)
            {
                return;
            }

            _urhoApp.SetImage(new MemoryBuffer(GetMemoryStream(imageStream)));
        }

        void UpdateFieldOfView()
        {
            if (_urhoApp != null)
            {
                _urhoApp.SetFieldOfView(_fieldOfView);
            }
        }

        void UpdateYaw()
        {
            if (_urhoApp != null)
            {
                _urhoApp.SetYaw(_yaw);
            }
        }

        void UpdatePitch()
        {
            if (_urhoApp != null)
            {
                _urhoApp.SetPitch(_pitch);
            }
        }

        public void SetGesture(IGestureRecognizer gesture)
        {
            if (_urhoSurface != null)
            {
                _urhoSurface.GestureRecognizers.Add(gesture);
            }
        }

        static MemoryStream GetMemoryStream(Stream stream)
        {
            if (stream is MemoryStream)
            {
                return stream as MemoryStream;
            }
            else
            {
                // Depending on stream size, this can be expensive operation
                // Therefore it is preferred that the stream is actually a MemoryStream
                var ms = new MemoryStream();

                stream.CopyTo(ms);
                ms.Position = 0;

                return ms;
            }
        }
    }
}