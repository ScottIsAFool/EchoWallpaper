using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

namespace EchoWallpaper.WindowsPhone.Silverlight.Views
{
    public partial class MainPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ApplicationBar != null)
            {
                ApplicationBar.BackgroundColor = (Color) Application.Current.Resources["AppBarBackground"];
                ApplicationBar.ForegroundColor = (Color) Application.Current.Resources["PhoneAccentColor"];
            }

            base.OnNavigatedTo(e);
        }
    }
}