using Windows.Storage;
using Windows.UI.Xaml.Controls;

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

            var folder = ApplicationData.Current.LocalFolder;
            var name = folder.Name;
        }
    }
}
