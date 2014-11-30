using System.IO;
using System.Threading.Tasks;
using EchoWallpaper.Core.Interfaces;

namespace EchoWallpaper.Core.Empty
{
    public class EmptyMediaLibraryService : IMediaLibraryService
    {
        public async Task SavePicture(string file, Stream stream)
        {
        }
    }
}