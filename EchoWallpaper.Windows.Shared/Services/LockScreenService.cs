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
        public bool IsProvidedByCurrentApplication
        {
            get
            {
                var status = BackgroundExecutionManager.GetAccessStatus();
                return status != BackgroundAccessStatus.Denied && status != BackgroundAccessStatus.Unspecified;
            }
        }

        public Uri ImageUri { get; set; }

        private static LockScreenService _current;
        public static LockScreenService Current { get { return _current ?? (_current = new LockScreenService()); } }

        public async Task<LockScreenServiceRequestResult> RequestAccessAsync()
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            switch (status)
            {
                case BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity:
                    case BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity:
                    return LockScreenServiceRequestResult.Granted;
                default:
                    return LockScreenServiceRequestResult.Denied;
            }
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