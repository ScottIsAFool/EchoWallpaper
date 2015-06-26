using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core.Empty;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Core.Model;
using EchoWallpaper.Core.Services;
using EchoWallpaper.Core.ViewModel;
using EchoWallpaper.Windows.Shared.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace EchoWallpaper.ViewModel
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

                if (!SimpleIoc.Default.IsRegistered<INavigation>())
                    SimpleIoc.Default.Register<INavigation, EmptyNavigationService>();

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

                if (!SimpleIoc.Default.IsRegistered<INavigation>())
                    SimpleIoc.Default.Register<INavigation, NavigationService>();

                if (!SimpleIoc.Default.IsRegistered<ILockScreenService>())
                    SimpleIoc.Default.Register<ILockScreenService, LockScreenService>();

                if (!SimpleIoc.Default.IsRegistered<IMediaLibraryService>())
                    SimpleIoc.Default.Register<IMediaLibraryService, MediaLibraryService>();

                if (!SimpleIoc.Default.IsRegistered<ILauncherService>())
                    SimpleIoc.Default.Register<ILauncherService, LauncherService>();

                if (!SimpleIoc.Default.IsRegistered<IStoreService>())
                    SimpleIoc.Default.Register<IStoreService, StoreService>();

                if (!SimpleIoc.Default.IsRegistered<IApplicationInfoService>())
                    SimpleIoc.Default.Register<IApplicationInfoService, ApplicationInfoService>();
            }

            if(!SimpleIoc.Default.IsRegistered<IDisplayPropertiesService>())
                SimpleIoc.Default.Register<IDisplayPropertiesService, DisplayPropertiesService>();

            if (!SimpleIoc.Default.IsRegistered<IAppSettings>())
                SimpleIoc.Default.Register<IAppSettings, AppSettings>();

            if (!SimpleIoc.Default.IsRegistered<IBackgroundTaskService>())
                SimpleIoc.Default.Register<IBackgroundTaskService, BackgroundTaskService>();

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