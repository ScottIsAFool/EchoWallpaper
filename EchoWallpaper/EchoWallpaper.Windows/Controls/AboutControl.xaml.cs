// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

using System;
using System.Windows.Input;
using Windows.UI.Xaml.Documents;
using EchoWallpaper.Core.ViewModel;
using U2UC.WinRT.HyperlinkBox.Helpers;

namespace EchoWallpaper.Controls
{
    public sealed partial class AboutControl
    {
        public AboutControl()
        {
            InitializeComponent();

            var vm = DataContext as AboutViewModel;
            if (vm != null)
            {
                RichTextBox.AssignRawText(vm.RawHtml);
            }
        }
    }
}
