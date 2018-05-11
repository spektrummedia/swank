using System.IO;
using System.Threading.Tasks;

namespace Plugin.Swank._360Controls
{
    public abstract class PanoramaImageSource
    {
        public abstract Task<Stream> GetStreamAsync();
    }
}