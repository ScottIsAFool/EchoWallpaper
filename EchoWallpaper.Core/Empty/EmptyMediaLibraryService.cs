using System.IO;
using EchoWallpaper.Core.Interfaces;

namespace EchoWallpaper.Core.Empty
{
    public class EmptyMediaLibraryService : IMediaLibraryService
    {
        public void SavePicture(string file, Stream stream)
        {
        }
    }
}