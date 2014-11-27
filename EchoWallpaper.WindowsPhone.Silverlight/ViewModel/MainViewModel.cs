using System;
using System.Data.Linq;
using System.Threading.Tasks;
using System.Windows;
using Windows.Security.Authentication.OnlineId;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Model;
using EchoWallpaper.WindowsPhone.Silverlight.Background.Model;
using EchoWallpaper.WindowsPhone.Silverlight.Background.Services;
using EchoWallpaper.WindowsPhone.Silverlight.Services;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;
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
        private bool _dataLoaded;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IAppSettings appSettings)
        {
            _appSettings = appSettings;
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
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
                return new RelayCommand(() =>
                {
                    
                });
            }
        }

        public RelayCommand DownloadImageNow
        {
            get
            {
                return new RelayCommand(() =>
                {
                    
                });
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
            CheckAndStartStopAgent();
        }

        [UsedImplicitly]
        private void OnDownloadImageForStartScreenChanged()
        {
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