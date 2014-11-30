using EchoWallpaper.Windows.Shared;
using Windows.ApplicationModel.Background;

namespace EchoWallpaper.Windows.Background
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var settings = new AppSettings()
            var lockScreen = new LockScreenService();
            
        }
    }
}
