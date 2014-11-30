using System.IO;
using System.Threading.Tasks;

namespace EchoWallpaper.Core.Interfaces
{
    public interface IMediaLibraryService
    {
        Task SavePicture(string file, Stream stream);
    }
}
