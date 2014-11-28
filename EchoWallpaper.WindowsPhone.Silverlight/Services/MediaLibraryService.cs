using System.IO;
using EchoWallpaper.Core.Interfaces;
using Microsoft.Xna.Framework.Media;

namespace EchoWallpaper.WindowsPhone.Silverlight.Services
{
    public class MediaLibraryService : IMediaLibraryService
    {
        private readonly MediaLibrary _library = new MediaLibrary();

        public void SavePicture(string file, Stream stream)
        {
            _library.SavePicture(file, stream);
        }
    }
}
