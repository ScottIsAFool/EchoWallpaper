using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.System.UserProfile;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Core.Model;

namespace EchoWallpaper.Windows.Shared.Services
{
    public class LockScreenService : ILockScreenService
    {
        public LockScreenService()
        {
            Current = this;
        }

        public bool IsProvidedByCurrentApplication
        {
            get { return true; }
        }

        public Uri ImageUri { get; set; }

        public static LockScreenService Current { get; private set; }

        public async Task<LockScreenServiceRequestResult> RequestAccessAsync()
        {
            return LockScreenServiceRequestResult.Granted;
        }

        public async Task RevokeAccessAsync()
        {
        }

        public async Task SetLockScreen(Uri uri)
        {
            using (var stream = await Echo.GetWallpaperStreamAsync(uri))
            {
                await SetLockScreen(stream);
            }
        }

        public async Task SetLockScreen(Stream stream)
        {
            await LockScreen.SetImageStreamAsync(stream.AsRandomAccessStream());
        }

        public Uri ImageUriToUse(Wallpapers wallpapers)
        {
            return wallpapers != null ? wallpapers.NineteenTwentyTenEighty : null;
        }
    }
}