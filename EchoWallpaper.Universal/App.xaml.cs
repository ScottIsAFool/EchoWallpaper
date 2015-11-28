using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Callisto.Controls.SettingsManagement;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Extensions;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Core.Services;
using EchoWallpaper.Universal.ViewModel;
using EchoWallpaper.Universal.Views;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using ScottIsAFool.Windows.Controls;

namespace EchoWallpaper.Universal
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : BaseApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        public override void AppStarted()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar.GetForCurrentView().HideAsync();
            }

            var titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = true;

            var accentBrush = Application.Current.Resources["AccentBrush"] as SolidColorBrush;

            var titleBarWithButtons = ApplicationView.GetForCurrentView().TitleBar;
            titleBarWithButtons.ButtonBackgroundColor = accentBrush?.Color;
            titleBarWithButtons.ButtonForegroundColor = Colors.White;
            titleBarWithButtons.BackgroundColor = accentBrush?.Color;
            titleBarWithButtons.ForegroundColor = Colors.White;

            NavigationService.HandleBackButtonVisibility = false;

            LoadSettings();

            base.AppStarted();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }

            base.OnLaunched(e);
            
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void LoadSettings()
        {
            var localSettings = ViewModelLocator.SettingsService.Local;
            var json = localSettings.Get(Constants.Settings.AppSettings, string.Empty);
            var app = ViewModelLocator.Settings;

            var settings = JsonConvert.DeserializeObject<Core.Model.AppSettings>(json);
            if (settings != null)
            {
                app.AutomaticallyUpdateLockScreen = settings.AutomaticallyUpdateLockScreen;
                app.DownloadImageForStartScreen = settings.DownloadImageForStartScreen;
                app.AutomaticallyUpdateWallpaper = settings.AutomaticallyUpdateWallpaper;
            }

            SetScreenSize(app);
        }

        private void SetScreenSize(IAppSettings appSettings)
        {
            var display = SimpleIoc.Default.GetInstance<IDisplayPropertiesService>();

            appSettings.WallpaperSizeToUse = display.PhysicalBounds.GetWallpaperSize();
            appSettings.Save();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
