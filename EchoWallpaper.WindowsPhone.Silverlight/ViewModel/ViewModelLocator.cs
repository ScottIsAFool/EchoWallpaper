/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:EchoWallpaper.WindowsPhone.Silverlight"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core.Empty;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Core.ViewModel;
using EchoWallpaper.WindowsPhone.Silverlight.Background.Model;
using EchoWallpaper.WindowsPhone.Silverlight.Background.Services;
using EchoWallpaper.WindowsPhone.Silverlight.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace EchoWallpaper.WindowsPhone.Silverlight.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                if (!SimpleIoc.Default.IsRegistered<IStorageService>())
                    SimpleIoc.Default.Register<IStorageService, EmptyStorageService>();

                if (!SimpleIoc.Default.IsRegistered<IApplicationSettingsService>())
                    SimpleIoc.Default.Register<IApplicationSettingsService, EmptySettingsService>();

                if(!SimpleIoc.Default.IsRegistered<INavigationService>())
                    SimpleIoc.Default.Register<INavigationService, EmptyNavigationService>();

                if (!SimpleIoc.Default.IsRegistered<ILockScreenService>())
                    SimpleIoc.Default.Register<ILockScreenService, EmptyLockScreenService>();

                if (!SimpleIoc.Default.IsRegistered<IMediaLibraryService>())
                    SimpleIoc.Default.Register<IMediaLibraryService, EmptyMediaLibraryService>();

                if (!SimpleIoc.Default.IsRegistered<ILauncherService>())
                    SimpleIoc.Default.Register<ILauncherService, EmptyLauncherService>();

                if (!SimpleIoc.Default.IsRegistered<IStoreService>())
                    SimpleIoc.Default.Register<IStoreService, EmptyStoreService>();

                if (!SimpleIoc.Default.IsRegistered<IApplicationInfoService>())
                    SimpleIoc.Default.Register<IApplicationInfoService, EmptyApplicationInfoService>();
            }
            else
            {
                // Create run time view services and models
                if (!SimpleIoc.Default.IsRegistered<IStorageService>())
                    SimpleIoc.Default.Register<IStorageService, StorageService>();

                if (!SimpleIoc.Default.IsRegistered<IApplicationSettingsService>())
                    SimpleIoc.Default.Register<IApplicationSettingsService, ApplicationSettingsService>();

                if (!SimpleIoc.Default.IsRegistered<INavigationService>())
                    SimpleIoc.Default.Register<INavigationService, NavigationService>();

                if (!SimpleIoc.Default.IsRegistered<ILockScreenService>())
                    SimpleIoc.Default.Register<ILockScreenService>(() => LockscreenService.Current);

                if (!SimpleIoc.Default.IsRegistered<IMediaLibraryService>())
                    SimpleIoc.Default.Register<IMediaLibraryService, MediaLibraryService>();

                if (!SimpleIoc.Default.IsRegistered<ILauncherService>())
                    SimpleIoc.Default.Register<ILauncherService, LauncherService>();

                if (!SimpleIoc.Default.IsRegistered<IStoreService>())
                    SimpleIoc.Default.Register<IStoreService, StoreService>();

                if (!SimpleIoc.Default.IsRegistered<IApplicationInfoService>())
                    SimpleIoc.Default.Register<IApplicationInfoService, ApplicationInfoService>();
            }

            if (!SimpleIoc.Default.IsRegistered<IAppSettings>())
                SimpleIoc.Default.Register<IAppSettings, AppSettings>();

            if(!SimpleIoc.Default.IsRegistered<LockscreenService>())
                SimpleIoc.Default.Register<LockscreenService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public AboutViewModel About
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutViewModel>();
            }
        }

        public static IAppSettings Settings
        {
            get { return ServiceLocator.Current.GetInstance<IAppSettings>(); }
        }

        public static IApplicationSettingsService SettingsService
        {
            get { return ServiceLocator.Current.GetInstance<IApplicationSettingsService>(); }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}