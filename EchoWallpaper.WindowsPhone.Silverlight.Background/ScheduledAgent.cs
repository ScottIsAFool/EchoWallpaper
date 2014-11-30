using System;
using System.Diagnostics;
using System.Windows;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Extensions;
using EchoWallpaper.Core.Model;
using EchoWallpaper.WindowsPhone.Silverlight.Background.Model;
using EchoWallpaper.WindowsPhone.Silverlight.Background.Services;
using Microsoft.Phone.Scheduler;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;

namespace EchoWallpaper.WindowsPhone.Silverlight.Background
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static IApplicationSettingsServiceHandler _settingsService;
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
                _settingsService = new ApplicationSettingsService().Local;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override async void OnInvoke(ScheduledTask task)
        {
            var lastRunDetailsJson = _settingsService.Get<string>(Constants.Settings.LastRunSettings);
            LastRunDetails lastRunDetails = null;

            if (!string.IsNullOrEmpty(lastRunDetailsJson))
            {
                lastRunDetails = JsonConvert.DeserializeObject<LastRunDetails>(lastRunDetailsJson);
                if (lastRunDetails != null && lastRunDetails.LastRunDate.HasValue)
                {
                    if (!lastRunDetails.LastRunDate.Value.LastDateOk())
                    {
                        NotifyComplete();
                        return;
                    }
                }
            }

            var json = _settingsService.Get<string>(Constants.Settings.AppSettings);
            if (string.IsNullOrEmpty(json))
            {
                NotifyComplete();
                return;
            }

            var settings = JsonConvert.DeserializeObject<AppSettings>(json);
            if (settings == null || (!settings.AutomaticallyUpdateLockScreen && !settings.DownloadImageForStartScreen))
            {
                NotifyComplete();
                return;
            }

            try
            {
                var wallpapers = await Echo.GetWallpapersAsync();
                var date = DateTime.Now;

                if (lastRunDetails != null && wallpapers.HdNoCalendar == lastRunDetails.LastUsedUri)
                {
                    NotifyComplete();
                    return;
                }

                using (var stream = await Echo.GetWallpaperStreamAsync(wallpapers.HdNoCalendar))
                {
                    if (settings.AutomaticallyUpdateLockScreen)
                    {
                        await LockscreenService.Current.SetLockScreen(stream);
                    }

                    if (settings.DownloadImageForStartScreen)
                    {
                        var library = new MediaLibrary();
                        var file = string.Format("{0}.{1}.jpg", date.Year, date.ToString("MMMM"));
                        library.SavePicture(file, stream);
                    }
                }

                if (lastRunDetails == null)
                {
                    lastRunDetails = new LastRunDetails();
                }

                lastRunDetails.LastRunDate = date;
                lastRunDetails.LastUsedUri = wallpapers.HdNoCalendar;

                lastRunDetailsJson = JsonConvert.SerializeObject(lastRunDetails);
                _settingsService.Set(Constants.Settings.LastRunSettings, lastRunDetailsJson);
            }
            catch (Exception ex)
            {
                
            }

            NotifyComplete();
        }
    }
}