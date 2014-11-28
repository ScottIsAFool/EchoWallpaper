using System.IO;

namespace EchoWallpaper.Core.Interfaces
{
    public interface IMediaLibraryService
    {
        void SavePicture(string file, Stream stream);
    }
}
