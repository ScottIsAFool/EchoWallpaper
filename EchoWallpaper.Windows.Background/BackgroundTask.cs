using System;
using Windows.Storage;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Extensions;
using EchoWallpaper.Core.Model;
using EchoWallpaper.Windows.Shared.Extensions;
using EchoWallpaper.Windows.Shared.Services;
using Windows.ApplicationModel.Background;
using Newtonsoft.Json;

namespace EchoWallpaper.Windows.Background
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        private static readonly LockScreenService LockScreenService = new LockScreenService();
        private static readonly MediaLibraryService MediaLibraryService = new MediaLibraryService();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
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
                        Finish(deferral);
                        return;
                    }
                }
            }

            var json = localSettings.Get<string>(Constants.Settings.AppSettings);
            if (string.IsNullOrEmpty(json))
            {
                Finish(deferral);
                return;
            }

            var settings = JsonConvert.DeserializeObject<AppSettings>(json);
            if (settings == null || (!settings.AutomaticallyUpdateLockScreen && !settings.DownloadImageForStartScreen))
            {
                Finish(deferral);
                return;
            }

            try
            {
                var wallpapers = await Echo.GetWallpapersAsync();
                var date = DateTime.Now;

                var uri = wallpapers.GetUri(settings.WallpaperSizeToUse);

                if (uri == null || (lastRunDetails != null && uri == lastRunDetails.LastUsedUri))
                {
                    Finish(deferral);
                    return;
                }

                using (var stream = await Echo.GetWallpaperStreamAsync(uri))
                {
                    if (settings.AutomaticallyUpdateLockScreen)
                    {
                        await LockScreenService.SetLockScreen(stream);
                    }

                    if (settings.DownloadImageForStartScreen)
                    {
                        var file = string.Format("{0}.{1}.jpg", date.Year, date.ToString("MMMM"));
                        await MediaLibraryService.SavePicture(file, stream);
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

            Finish(deferral);
        }

        private void Finish(BackgroundTaskDeferral deferral)
        {
            deferral.Complete();
        }
    }
}
