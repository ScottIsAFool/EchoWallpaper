using Windows.ApplicationModel.Background;
using EchoWallpaper.Windows.Shared.Helpers;
using EchoWallpaper.Windows.Shared.Services;

namespace EchoWallpaper.Windows.Background
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        private static readonly WindowsLockScreenService WindowsLockScreenService = new WindowsLockScreenService();
        private static readonly MediaLibraryService MediaLibraryService = new MediaLibraryService();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            await BackgroundHelper.UpdateAllTheThings(() => Finish(deferral), WindowsLockScreenService, MediaLibraryService);
        }

        private static void Finish(BackgroundTaskDeferral deferral)
        {
            deferral.Complete();
        }
    }
}
