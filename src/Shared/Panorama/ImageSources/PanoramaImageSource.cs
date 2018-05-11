using System.IO;
using System.Threading.Tasks;

namespace Plugin.Swank.Panorama.ImageSources
{
    public abstract class PanoramaImageSource
    {
        public abstract Task<Stream> GetStreamAsync();
    }
}