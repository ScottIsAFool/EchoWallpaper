using System;
using System.Windows;
using Windows.ApplicationModel;
using Microsoft.Phone.Tasks;

namespace EchoWallpaper.WindowsPhone.Silverlight.Views
{
    public partial class AboutView
    {
        private string VersionText
        {
            get
            {
                var version = Package.Current.Id.Version;
                return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build)
            }
        }
        // Constructor
        public AboutView()
        {
            InitializeComponent();

            Version.Text =  string.Format("Version: {0}", VersionText);
        }

        private void DaveLarsen_OnClick(object sender, RoutedEventArgs e)
        {
            new WebBrowserTask()
            {
                Uri = new Uri("http://www.thenounproject.com/davelarsen")
            }.Show();
        }

        private void NounProject_OnClick(object sender, RoutedEventArgs e)
        {
            new WebBrowserTask()
            {
                Uri = new Uri("http://www.thenounproject.com/")
            }.Show();
        }

        private void Feedback_OnClick(object sender, RoutedEventArgs e)
        {
            new EmailComposeTask()
            {
                To = "scottisafool@live.co.uk",
                Subject = string.Format("Feedback from Echo Wallpapers ({0})", VersionText)
            }.Show();
        }

        private void RateAndReview_OnClick(object sender, RoutedEventArgs e)
        {
            new MarketplaceReviewTask().Show();
        }
    }
}