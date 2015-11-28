using System;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Core.Model;
using EchoWallpaper.Core.Services;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;
using ScottIsAFool.WindowsPhone.ViewModel;
using ILockScreenService = EchoWallpaper.Core.Interfaces.ILockScreenService;
using LockScreenServiceRequestResult = EchoWallpaper.Core.Model.LockScreenServiceRequestResult;

namespace EchoWallpaper.Core.ViewModel
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
        private readonly INavigation _navigationService;
        private readonly ILockScreenService _lockscreenService;
        private readonly IMediaLibraryService _mediaLibraryService;
        private readonly IBackgroundTaskService _backgroundTaskService;
        private readonly IDeviceSettingsService _deviceSettingsService;
        private readonly IWallpaperService _wallpaperService;

        private bool _dataLoaded;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(
            IAppSettings appSettings, 
            INavigation navigationService,
            ILockScreenService lockscreenService,
            IMediaLibraryService mediaLibraryService,
            IBackgroundTaskService backgroundTaskService,
            IDeviceSettingsService deviceSettingsService,
            IWallpaperService wallpaperService)
        {
            _appSettings = appSettings;
            _navigationService = navigationService;
            _lockscreenService = lockscreenService;
            _mediaLibraryService = mediaLibraryService;
            _backgroundTaskService = backgroundTaskService;
            _deviceSettingsService = deviceSettingsService;
            _wallpaperService = wallpaperService;

            if (IsInDesignMode)
            {
                CurrentWallpapers = new Wallpapers
                {
                    PortraitPreviewImage = new Uri("http://www.bournemouthecho.co.uk/resources/images/3635576.jpg"),
                    LandscapePreviewImage = new Uri("http://www.bournemouthecho.co.uk/resources/images/3345456.jpg"),
                    NineteenTwentyTenEighty = new Uri("http://www.bournemouthecho.co.uk/resources/images/3345456.jpg"),
                    Title = "A picture from the Bournemouth Echo"
                };
            }

            AutomaticallyUpdateLockscreen = _appSettings.AutomaticallyUpdateLockScreen;
            DownloadImageForStartScreen = _appSettings.DownloadImageForStartScreen;
        }

        public Wallpapers CurrentWallpapers { get; set; }
        public bool IsLockscreenProvider { get { return _lockscreenService.IsProvidedByCurrentApplication; } }
        public bool AutomaticallyUpdateLockscreen { get; set; }
        public bool DownloadImageForStartScreen { get; set; }
        public bool AutomaticallyUpdateWallpaper { get; set; }
        public bool BackgroundAgentAllowed { get { return _backgroundTaskService.CanRunTask; } }

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
                    try
                    {
                        await _backgroundTaskService.CreateAgent();
                    }
                    catch (Exception ex)
                    {
                        var i = 1;
                    }
                    await LoadData(false);
                });
            }
        }

        public RelayCommand SettingsLoadedCommand
        {
            get
            {

                return new RelayCommand(() => RaisePropertyChanged(() => BackgroundAgentAllowed));
            }
        }

        public RelayCommand CreateBackgroundTaskCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    _backgroundTaskService.StopAgent();
                    
                    await _backgroundTaskService.CreateAgent();

                    RaisePropertyChanged(() => BackgroundAgentAllowed);
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
                        await _lockscreenService.RevokeAccessAsync();
                        result = await _lockscreenService.RequestAccessAsync();
                    }

                    if (result == LockScreenServiceRequestResult.Denied)
                    {
                        return;
                    }

                    var uri = _lockscreenService.ImageUriToUse(CurrentWallpapers, _appSettings.WallpaperSizeToUse);

                    if (CurrentWallpapers == null || uri == null)
                    {
                        return;
                    }

                    SetProgressBar("Setting lock screen...");

                    await _lockscreenService.SetLockScreen(uri);

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
                    var uri = _lockscreenService.ImageUriToUse(CurrentWallpapers, _appSettings.WallpaperSizeToUse);

                    if (CurrentWallpapers == null || uri == null)
                    {
                        return;
                    }
                    try
                    {
                        SetProgressBar("Saving image...");
                        using (var stream = await Echo.GetWallpaperStreamAsync(uri))
                        {
                            var date = DateTime.Now;
                            var file = string.Format("{0}.{1}.jpg", date.Year, date.ToString("MMMM"));
                            await _mediaLibraryService.SavePicture(file, stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Log.ErrorException("DownloadImageNow", ex);
                    }
                    
                    SetProgressBar();
                });
            }
        }

        public RelayCommand GoToLockScreenSettings
        {
            get
            {
                return new RelayCommand(() => _deviceSettingsService.ShowLockScreenSettingsAsync());
            }
        }

        public RelayCommand NavigateToSettingsCommand
        {
            get
            {
                return new RelayCommand(() => _navigationService.NavigateToSettings());
            }
        }

        public RelayCommand NavigateToAboutCommand
        {
            get
            {
                return new RelayCommand(() => _navigationService.NavigateToAbout());
            }
        }

        public RelayCommand SetAsLockScreenAppCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await _lockscreenService.RevokeAccessAsync();
                    var result = await _lockscreenService.RequestAccessAsync();

                    if (result == LockScreenServiceRequestResult.Granted)
                    {
                        RaisePropertyChanged(() => IsLockscreenProvider);
                    }
                });
            }
        }

        public RelayCommand SetAsWallpaperCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (_wallpaperService.CanSetWallpaper)
                    {
                        var uri = _lockscreenService.ImageUriToUse(CurrentWallpapers, _appSettings.WallpaperSizeToUse);

                        if (uri == null)
                        {
                            return;
                        }

                        SetProgressBar("Setting wallpaper...");

                        await _wallpaperService.SetWallpaper(uri);

                        SetProgressBar();
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
                //Log.ErrorException("LoadData(" + isRefresh + ")", ex);
                var s = "";
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
        private void OnAutomaticallyUpdateWallpaperChanged()
        {
            _appSettings.AutomaticallyUpdateWallpaper = AutomaticallyUpdateWallpaper;
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