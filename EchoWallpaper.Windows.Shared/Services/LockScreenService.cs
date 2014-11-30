using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.System.UserProfile;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Core.Model;

namespace EchoWallpaper.Windows.Shared.Services
{
    public class LockScreenService : ILockScreenService
    {
        public bool IsProvidedByCurrentApplication { get { return true; } }
        public Uri ImageUri { get; set; }
        public async Task<LockScreenServiceRequestResult> RequestAccessAsync()
        {
            return LockScreenServiceRequestResult.Granted;
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