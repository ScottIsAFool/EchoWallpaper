using System.IO;
using System.Threading.Tasks;
using EchoWallpaper.Core.Interfaces;
using Microsoft.Xna.Framework.Media;

namespace EchoWallpaper.WindowsPhone.Silverlight.Services
{
    public class MediaLibraryService : IMediaLibraryService
    {
        private readonly MediaLibrary _library = new MediaLibrary();

        public async Task SavePicture(string file, Stream stream)
        {
            _library.SavePicture(file, stream);
        }
    }
}
