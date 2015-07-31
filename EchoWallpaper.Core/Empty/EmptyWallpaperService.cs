using System;
using System.IO;
using System.Threading.Tasks;
using EchoWallpaper.Core.Interfaces;

namespace EchoWallpaper.Core.Empty
{
    public class EmptyWallpaperService : IWallpaperService
    {
        public bool CanSetWallpaper { get; } = false;
        public async Task SetWallpaper(Uri uri)
        {
        }

        public async Task SetWallpaper(Stream stream)
        {
        }
    }
}
