using Windows.ApplicationModel;
using EchoWallpaper.Core.Interfaces;

namespace EchoWallpaper.Windows.Shared.Services
{
    public class ApplicationInfoService : IApplicationInfoService
    {
        public string Version
        {
            get
            {
                var version = Package.Current.Id.Version;
                return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            }
        }

        public string AppId
        {
            get { return Package.Current.Id.FamilyName; }
        }
    }
}