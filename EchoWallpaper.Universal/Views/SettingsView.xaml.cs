using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using EchoWallpaper.Core.Services;
using GalaSoft.MvvmLight.Ioc;

namespace EchoWallpaper.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsView : Page
    {
        public SettingsView()
        {
            this.InitializeComponent();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var nav = SimpleIoc.Default.GetInstance<INavigation>();
            if (nav.CanGoBack)
            {
                nav.GoBack();
            }
        }
    }
}
