using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using EchoWallpaper.Core.Services;
using EchoWallpaper.Core.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using U2UC.WinRT.HyperlinkBox.Helpers;

namespace EchoWallpaper.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutView : Page
    {
        public AboutView()
        {
            this.InitializeComponent();

            var vm = DataContext as AboutViewModel;
            if (vm != null)
            {
                RichTextBox.AssignRawText(vm.RawHtml);
            }
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
