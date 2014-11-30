using System;
using System.IO;
using System.Threading.Tasks;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Core.Model;

namespace EchoWallpaper.Windows.Shared.Services
{
    public class LockScreenService : ILockScreenService
    {
        public string LockScreenImageUrl { get; private set; }
        public string LockScreenFile { get; private set; }
        public bool IsProvidedByCurrentApplication { get; private set; }
        public Uri ImageUri { get; set; }
        public async Task<LockScreenServiceRequestResult> RequestAccessAsync()
        {
            return LockScreenServiceRequestResult.Granted;
        }

        public async Task SetLockScreen()
        {
        }

        public async Task SetLockScreen(Uri uri)
        {
        }

        public async Task SetLockScreen(Stream stream)
        {
        }
    }
}