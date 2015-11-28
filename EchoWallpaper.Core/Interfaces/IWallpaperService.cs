using System;
using System.IO;
using System.Threading.Tasks;

namespace EchoWallpaper.Core.Interfaces
{
    public interface IWallpaperService
    {
        bool CanSetWallpaper { get; }
        Task SetWallpaper(Uri uri);
        Task SetWallpaper(Stream stream);
    }
}
