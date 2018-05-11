using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Plugin.Swank.Panorama.ImageSources
{
    public class PanoramaUriImageSource : PanoramaImageSource
    {
        readonly Uri _imageUri;

        Stream _imageStream;

        public PanoramaUriImageSource(Uri imageUri)
        {
            _imageUri = imageUri;
        }

        public async override Task<Stream> GetStreamAsync()
        {
            if (_imageStream == null)
            {
                _imageStream = await new HttpClient().GetStreamAsync(_imageUri);
            }

            return _imageStream;
        }
    }
}