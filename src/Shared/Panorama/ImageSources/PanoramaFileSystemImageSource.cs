using System;
using System.IO;
using System.Threading.Tasks;

namespace Plugin.Swank.Panorama.ImageSources
{
    public class PanoramaFileSystemImageSource : PanoramaImageSource
    {
        private readonly string _imageFilePath;

        private Stream _imageStream;

        public PanoramaFileSystemImageSource(string imageFilePath)
        {
            _imageFilePath = imageFilePath;
        }

        public override async Task<Stream> GetStreamAsync()
        {
            if (_imageStream == null)
            {
                if (!File.Exists(_imageFilePath))
                {
                    throw new Exception($"File {_imageFilePath} do not exists!");
                }

                _imageStream = File.Open(_imageFilePath, FileMode.Open, FileAccess.Read);
            }

            return _imageStream;
        }
    }
}