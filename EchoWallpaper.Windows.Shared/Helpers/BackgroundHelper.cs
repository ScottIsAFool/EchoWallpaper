using System;
using System.Threading.Tasks;
using Windows.Storage;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Extensions;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Core.Model;
using EchoWallpaper.Windows.Shared.Extensions;
using Newtonsoft.Json;

namespace EchoWallpaper.Windows.Shared.Helpers
{
    public static class BackgroundHelper
    {
        public static async Task UpdateAllTheThings(Action finish, ILockScreenService lockscreenService, IMediaLibraryService mediaLibraryService, IWallpaperService wallpaperService = null)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            var lastRunDetailsJson = localSettings.Get<string>(Constants.Settings.LastRunSettings);
            LastRunDetails lastRunDetails = null;

            if (!string.IsNullOrEmpty(lastRunDetailsJson))
            {
                lastRunDetails = JsonConvert.DeserializeObject<LastRunDetails>(lastRunDetailsJson);
                if (lastRunDetails != null && lastRunDetails.LastRunDate.HasValue)
                {
                    if (!lastRunDetails.LastRunDate.Value.LastDateOk())
                    {
                        finish();
                        return;
                    }
                }
            }

            var json = localSettings.Get<string>(Constants.Settings.AppSettings);
            if (string.IsNullOrEmpty(json))
            {
                finish();
                return;
            }

            var settings = JsonConvert.DeserializeObject<AppSettings>(json);
            if (settings == null || (!settings.AutomaticallyUpdateLockScreen && !settings.DownloadImageForStartScreen && !settings.AutomaticallyUpdateWallpaper))
            {
                finish();
                return;
            }

            try
            {
                var wallpapers = await Echo.GetWallpapersAsync();
                var date = DateTime.Now;

                var uri = wallpapers.GetUri(settings.WallpaperSizeToUse);

                if (uri == null || (lastRunDetails != null && uri == lastRunDetails.LastUsedUri))
                {
                    finish();
                    return;
                }

                using (var stream = await Echo.GetWallpaperStreamAsync(uri))
                {
                    if (settings.AutomaticallyUpdateLockScreen)
                    {
                        await lockscreenService.SetLockScreen(stream);
                    }

                    if (settings.AutomaticallyUpdateWallpaper && wallpaperService != null)
                    {
                        await wallpaperService.SetWallpaper(stream);
                    }

                    if (settings.DownloadImageForStartScreen)
                    {
                        var file = $"{date.Year}.{date.ToString("MMMM")}.jpg";
                        await mediaLibraryService.SavePicture(file, stream);
                    }
                }

                if (lastRunDetails == null)
                {
                    lastRunDetails = new LastRunDetails();
                }

                lastRunDetails.LastRunDate = date;
                lastRunDetails.LastUsedUri = uri;

                lastRunDetailsJson = JsonConvert.SerializeObject(lastRunDetails);
                localSettings.Values[Constants.Settings.LastRunSettings] = lastRunDetailsJson;
            }
            catch (Exception ex)
            {
            }

            finish();
        }
    }
}
