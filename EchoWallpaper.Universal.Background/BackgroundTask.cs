using Windows.ApplicationModel.Background;
using Cimbalino.Toolkit.Core.Services;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core.Interfaces;
using EchoWallpaper.Universal.Shared.Services;
using EchoWallpaper.Windows.Shared.Helpers;
using EchoWallpaper.Windows.Shared.Services;
using ILockScreenService = EchoWallpaper.Core.Interfaces.ILockScreenService;

namespace EchoWallpaper.Universal.Background
{
    public sealed class BackgroundTask
    {
        private static readonly ILockScreenService UniversalLockScreenService = new UniversalLockScreenService(new StorageService(), new PersonalizationService());
        private static readonly IMediaLibraryService MediaLibraryService = new MediaLibraryService();
        private static readonly IWallpaperService UniversalWallpaperService = new UniversalWallpaperService(new StorageService(), new PersonalizationService());

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            await BackgroundHelper.GetValue(() => Finish(deferral), UniversalLockScreenService, MediaLibraryService, UniversalWallpaperService);
        }

        private static void Finish(BackgroundTaskDeferral deferral)
        {
            deferral.Complete();
        }
    }
}
