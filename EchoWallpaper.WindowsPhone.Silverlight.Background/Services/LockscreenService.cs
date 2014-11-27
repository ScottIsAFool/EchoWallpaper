using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Phone.System.UserProfile;
using Cimbalino.Toolkit.Extensions;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core;
using ScottIsAFool.WindowsPhone.Logging;

namespace EchoWallpaper.WindowsPhone.Silverlight.Background.Services
{
    public class LockscreenService 
    {
        private const string LockScreenImageUrlNormal = "ms-appdata:///Local/shared/shellcontent/MBWallpaper.png";
        private const string LockScreenImageUrlAlternative = "ms-appdata:///Local/shared/shellcontent/MBWallpaper2.png";
        private const string LockScreenFileNormal = "shared\\shellcontent\\MBWallpaper.png";
        private const string LockScreenFileAlternative = "shared\\shellcontent\\MBWallpaper2.png";
        private const string DefaultLockScreenImageFormat = "ms-appx:///DefaultLockScreen.jpg";

        private readonly IStorageServiceHandler _storage;
        private readonly ILog _logger;
        private static LockscreenService _current;
        public static LockscreenService Current { get { return _current ?? (_current = new LockscreenService()); } }

        public LockscreenService()
        {
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

        public async Task SetLockScreen()
        {
            try
            {
                var wallpapers = await Echo.GetWallpapersAsync();
                if (wallpapers != null && wallpapers.HdNoCalendar != null)
                {
                    var uri = wallpapers.HdNoCalendar;
                    await DownloadAndSaveImage(uri);
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorException("SetLockScreen()", ex);
            }
        }

        private async Task DownloadAndSaveImage(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    await _storage.WriteAllBytesAsync(LockScreenImageUrl, await stream.ToArrayAsync());
                }
            }
        }

        public async Task SetLockScreen(Uri uri)
        {
            try
            {
                await DownloadAndSaveImage(uri);
            }
            catch (Exception ex)
            {
                _logger.ErrorException("SetLockScreen(uri)", ex);
            }
        }
    }

    /// <summary>
    /// Indicates if the app was successfully or unsuccessfully set as the lock screen background provider.
    /// </summary>
    public enum LockScreenServiceRequestResult
    {
        /// <summary>
        /// The app was not set as the lock screen background provider.
        /// </summary>
        Denied,

        /// <summary>
        /// The app was set as the lock screen background provider.
        /// </summary>
        Granted
    }
}
