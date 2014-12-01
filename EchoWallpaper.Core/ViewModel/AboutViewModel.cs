using System;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core.Interfaces;
using GalaSoft.MvvmLight.Command;
using ScottIsAFool.WindowsPhone.ViewModel;

namespace EchoWallpaper.Core.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly ILauncherService _launcherService;
        private readonly IStoreService _storeService;
        private readonly IApplicationInfoService _applicationInfo;

        public AboutViewModel(ILauncherService launcherService, IStoreService storeService, IApplicationInfoService applicationInfo)
        {
            _launcherService = launcherService;
            _storeService = storeService;
            _applicationInfo = applicationInfo;
        }

        public string Version
        {
            get { return string.Format("Version {0}", _applicationInfo.Version); }
        }

        public string RawHtml
        {
            get
            {
                return "Beach designed by Dave Larsen (http://www.thenounproject.com/davelarsen) from the Noun Project (http://www.thenounproject.com).";
            }
        }

        public RelayCommand LaunchArtistCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _launcherService.LaunchUriAsync(new Uri("http://www.thenounproject.com/davelarsen", UriKind.Absolute));
                });
            }
        }

        public RelayCommand LaunchNounProjectCommand
        {
            get
            {
                return new RelayCommand(() => _launcherService.LaunchUriAsync(new Uri("http://www.thenounproject.com/", UriKind.Absolute)));
            }
        }

        public RelayCommand LaunchFeedbackCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var email = string.Format("mailto:scottisafool@live.co.uk?subject=Feedback from Echo Wallpapers ({0})", _applicationInfo.Version);
                    _launcherService.LaunchUriAsync(new Uri(email));
                });
            }
        }

        public RelayCommand LaunchRateReviewCommand
        {
            get
            {
                return new RelayCommand(() => _storeService.ReviewAsync(_applicationInfo.AppId));
            }
        }
    }
}
