using System;
using System.IO;
using System.Threading.Tasks;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Core.Model;

namespace EchoWallpaper.Services
{
    public class LockscreenService : ILockScreenService
    {
        private static LockscreenService _current;
        public static LockscreenService Current { get { return _current ?? (_current = new LockscreenService()); } }
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
