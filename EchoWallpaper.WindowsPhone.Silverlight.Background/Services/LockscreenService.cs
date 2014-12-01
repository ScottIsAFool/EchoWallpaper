using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Phone.System.UserProfile;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Core.Model;
using EchoWallpaper.WindowsPhone.Silverlight.Background.Extensions;
using ScottIsAFool.WindowsPhone.Logging;

namespace EchoWallpaper.WindowsPhone.Silverlight.Background.Services
{
    public class LockscreenService : ILockScreenService
    {
        private const string LockScreenImageUrlNormal = "ms-appdata:///Local/shared/shellcontent/MBWallpaper.png";
        private const string LockScreenImageUrlAlternative = "ms-appdata:///Local/shared/shellcontent/MBWallpaper2.png";
        private const string LockScreenFileNormal = "shared\\shellcontent\\MBWallpaper.png";
        private const string LockScreenFileAlternative = "shared\\shellcontent\\MBWallpaper2.png";

        private readonly IStorageServiceHandler _storage;
        private readonly IDisplayPropertiesService _display;
        private readonly ILog _logger;
        private static LockscreenService _current;
        public static LockscreenService Current { get { return _current ?? (_current = new LockscreenService()); } }

        public LockscreenService()
        {
            _display = new DisplayPropertiesService();
            _storage = new StorageService().Local;
            _logger = new WPLogger(typeof(LockscreenService));
        }

        public string LockScreenImageUrl
        {
            get
            {
                return ImageUri.ToString().EndsWith("2.png") ? LockScreenImageUrlNormal : LockScreenImageUrlAlternative;
            }
        }

        public string LockScreenFile
        {
            get
            {
                return ImageUri.ToString().EndsWith("2.png") ? LockScreenFileNormal : LockScreenFileAlternative;
            }
        }

        /// <summary>
        /// Sets the current app as the lock screen background provider.
        /// </summary>
        /// <returns>The <see cref="Task"/> object representing the asynchronous operation.</returns>
        public async Task<LockScreenServiceRequestResult> RequestAccessAsync()
        {
            var result = await LockScreenManager.RequestAccessAsync();

            switch (result)
            {
                case LockScreenRequestResult.Denied:
                    return LockScreenServiceRequestResult.Denied;

                case LockScreenRequestResult.Granted:
                    return LockScreenServiceRequestResult.Granted;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task RevokeAccessAsync()
        {
        }

        /// <summary>
        /// Gets a value indicating whether the app is the current lock screen background provider.
        /// </summary>
        /// <value>true if the app is the current lock screen background provider; otherwise, false.</value>
        public bool IsProvidedByCurrentApplication
        {
            get
            {
                return LockScreenManager.IsProvidedByCurrentApplication;
            }
        }

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) of the current lock screen background.
        /// </summary>
        /// <value>The Uniform Resource Identifier (URI) of the current lock screen background.</value>
        public Uri ImageUri
        {
            get
            {
                return LockScreen.GetImageUri();
            }
            set
            {
                LockScreen.SetImageUri(value);
            }
        }

        private async Task DownloadAndSaveImage(Uri uri)
        {
            using (var stream = await Echo.GetWallpaperStreamAsync(uri))
            {
                await SetLockScreen(stream);
            }
        }

        public async Task SetLockScreen(Uri uri)
        {
            try
            {
                await DownloadAndSaveImage(uri);

                ImageUri = new Uri(LockScreenImageUrl, UriKind.RelativeOrAbsolute);
            }
            catch (Exception ex)
            {
                _logger.ErrorException("SetLockScreen(uri)", ex);
            }
        }

        public async Task SetLockScreen(Stream stream)
        {
            using (var fileStream = await _storage.CreateFileAsync(LockScreenFile))
            {
                await stream.CopyToAsync(fileStream);
            }

            ImageUri = new Uri(LockScreenImageUrl, UriKind.RelativeOrAbsolute);
        }

        public Uri ImageUriToUse(Wallpapers wallpapers)
        {
            var res = _display.ToScreenResolution();

            switch (res)
            {
                case ScreenInfoServiceResolution.HD1080p:
                    return wallpapers.HdNoCalendar;
                case ScreenInfoServiceResolution.HD720p:
                    return wallpapers.SevenTwentyP;
                case ScreenInfoServiceResolution.WVGA:
                    return wallpapers.Wvga;
                case ScreenInfoServiceResolution.WXGA:
                    return wallpapers.Wxga;
                default:
                    return null;
            }
        }
    }
}
