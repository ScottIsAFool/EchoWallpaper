using System;
using System.Threading.Tasks;
using Windows.System;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Model;
using EchoWallpaper.WindowsPhone.Silverlight.Background.Model;
using EchoWallpaper.WindowsPhone.Silverlight.Background.Services;
using EchoWallpaper.WindowsPhone.Silverlight.Services;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Media;
using ScottIsAFool.WindowsPhone.ViewModel;

namespace EchoWallpaper.WindowsPhone.Silverlight.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IAppSettings _appSettings;
        private readonly IStorageServiceHandler _storage;

        private readonly MediaLibrary _library;
        private bool _dataLoaded;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IAppSettings appSettings, IStorageService storageService)
        {
            _appSettings = appSettings;
            _storage = storageService.Local;
            _library = new MediaLibrary();

            AutomaticallyUpdateLockscreen = _appSettings.AutomaticallyUpdateLockScreen;
            DownloadImageForStartScreen = _appSettings.DownloadImageForStartScreen;
        }

        public Wallpapers CurrentWallpapers { get; set; }
        public bool IsLockscreenProvider { get { return LockscreenService.Current.IsProvidedByCurrentApplication; } }
        public bool AutomaticallyUpdateLockscreen { get; set; }
        public bool DownloadImageForStartScreen { get; set; }

        public RelayCommand MainPageLoaded
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await LoadData(false);
                });
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await LoadData(true);
                });
            }
        }

        public RelayCommand SetLockscreenCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (CurrentWallpapers == null)
                    {
                        await LockscreenService.Current.SetLockScreen();
                    }
                    else
                    {
                        await LockscreenService.Current.SetLockScreen(CurrentWallpapers.HdNoCalendar);
                    }
                });
            }
        }

        public RelayCommand DownloadImageNow
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        using (var stream = await Echo.GetWallpaperStreamAsync(CurrentWallpapers.HdNoCalendar))
                        {
                            var date = DateTime.Now;
                            var file = string.Format("{0}.{1}.jpg", date.Year, date.ToString("MMMM"));
                            _library.SavePicture(file, stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorException("DownloadImageNow", ex);
                    }
                });
            }
        }

        public RelayCommand GoToLockScreenSettings
        {
            get
            {
                return new RelayCommand(() => Launcher.LaunchUriAsync(new Uri("ms-settings-lock:", UriKind.Absolute)));
            }
        }

        public RelayCommand SetAsLockScreenAppCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var result = await LockscreenService.Current.RequestAccessAsync();

                    if (result == LockScreenServiceRequestResult.Granted)
                    {
                        RaisePropertyChanged(() => IsLockscreenProvider);
                    }
                });
            }
        }

        private async Task LoadData(bool isRefresh)
        {
            if (_dataLoaded && !isRefresh)
            {
                return;
            }

            try
            {
                SetProgressBar("Getting current wallpapers...");
                var wallpapers = await Echo.GetWallpapersAsync();
                CurrentWallpapers = wallpapers;
                _dataLoaded = true;
            }
            catch (Exception ex)
            {
                Log.ErrorException("LoadData(" + isRefresh + ")", ex);
            }

            SetProgressBar();
        }

        [UsedImplicitly]
        private void OnAutomaticallyUpdateLockscreenChanged()
        {
            _appSettings.AutomaticallyUpdateLockScreen = AutomaticallyUpdateLockscreen;
            _appSettings.Save();
            CheckAndStartStopAgent();
        }

        [UsedImplicitly]
        private void OnDownloadImageForStartScreenChanged()
        {
            _appSettings.DownloadImageForStartScreen = DownloadImageForStartScreen;
            _appSettings.Save();
            CheckAndStartStopAgent();
        }

        private void CheckAndStartStopAgent()
        {
            if (AutomaticallyUpdateLockscreen || DownloadImageForStartScreen)
            {
                BackgroundTaskService.Current.CreateAgent();
            }
            else
            {
                BackgroundTaskService.Current.StopAgent();
            }
        }
    }
}