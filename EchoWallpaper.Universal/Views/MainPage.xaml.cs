using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace EchoWallpaper.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar.GetForCurrentView().HideAsync();
            }

            var titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = true;

            var accentBrush = Application.Current.Resources["AccentBrush"] as SolidColorBrush;
            var altAccentBrush = Application.Current.Resources["AltAccentBrush"] as SolidColorBrush;

            var titleBarWithButtons = ApplicationView.GetForCurrentView().TitleBar;
            titleBarWithButtons.ButtonBackgroundColor = accentBrush?.Color;
            titleBarWithButtons.ButtonForegroundColor = altAccentBrush?.Color;
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            SettingsFlyout flyout = new SettingsFlyout();
            flyout.ShowIndependent();
        }
    }
}
