using System;
using System.Threading.Tasks;
using Windows.System;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Interfaces;
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
        private readonly INavigationService _navigationService;

        private readonly MediaLibrary _library;
        private bool _dataLoaded;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IAppSettings appSettings, INavigationService navigationService)
        {
            _appSettings = appSettings;
            _navigationService = navigationService;
            _library = new MediaLibrary();

            AutomaticallyUpdateLockscreen = _appSettings.AutomaticallyUpdateLockScreen;
            DownloadImageForStartScreen = _appSettings.DownloadImageForStartScreen;
        }

        public Wallpapers CurrentWallpapers { get; set; }
        public bool IsLockscreenProvider { get { return LockscreenService.Current.IsProvidedByCurrentApplication; } }
        public bool AutomaticallyUpdateLockscreen { get; set; }
        public bool DownloadImageForStartScreen { get; set; }

        public bool CanDoStuff
        {
            get
            {
                return !ProgressIsVisible
                       && CurrentWallpapers != null;
            }
        }

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
                    var result = LockScreenServiceRequestResult.Granted;
                    if (!IsLockscreenProvider)
                    {
                        result = await LockscreenService.Current.RequestAccessAsync();
                    }

                    if (result == LockScreenServiceRequestResult.Denied)
                    {
                        return;
                    }

                    if (CurrentWallpapers == null || CurrentWallpapers.HdNoCalendar == null)
                    {
                        return;
                    }

                    SetProgressBar("Setting lock screen...");

                    await LockscreenService.Current.SetLockScreen(CurrentWallpapers.HdNoCalendar);

                    SetProgressBar();
                });
            }
        }

        public RelayCommand DownloadImageNow
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (CurrentWallpapers == null || CurrentWallpapers == null)
                    {
                        return;
                    }
                    try
                    {
                        SetProgressBar("Saving image...");
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
                    
                    SetProgressBar();
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

        public RelayCommand NavigateToSettingsCommand
        {
            get
            {
                return new RelayCommand(() => _navigationService.Navigate("/Views/SettingsView.xaml"));
            }
        }

        public RelayCommand NavigateToAboutCommand
        {
            get
            {
                return new RelayCommand(() => _navigationService.Navigate("/Views/AboutView.xaml"));
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
        }

        [UsedImplicitly]
        private void OnDownloadImageForStartScreenChanged()
        {
            _appSettings.DownloadImageForStartScreen = DownloadImageForStartScreen;
            _appSettings.Save();
        }

        public override void UpdateProperties()
        {
            RaisePropertyChanged(() => CanDoStuff);
        }
    }
}