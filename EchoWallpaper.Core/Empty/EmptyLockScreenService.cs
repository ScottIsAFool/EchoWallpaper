using System;
using System.IO;
using System.Threading.Tasks;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Core.Model;

namespace EchoWallpaper.Core.Empty
{
    public class EmptyLockScreenService : ILockScreenService
    {
        public bool IsProvidedByCurrentApplication { get; private set; }
        public Uri ImageUri { get; set; }
        public async Task<LockScreenServiceRequestResult> RequestAccessAsync()
        {
            return LockScreenServiceRequestResult.Granted;
        }

        public async Task RevokeAccessAsync()
        {
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

        public Uri ImageUriToUse(Wallpapers wallpapers)
        {
            return null;
        }
    }
}