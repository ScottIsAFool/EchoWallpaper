using System;
using System.IO;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Extensions;
using EchoWallpaper.Core.Model;
using ILockScreenService = EchoWallpaper.Core.Interfaces.ILockScreenService;
using LockScreenServiceRequestResult = EchoWallpaper.Core.Model.LockScreenServiceRequestResult;

namespace EchoWallpaper.Universal.Services
{
    public class UniversalLockScreenService : ILockScreenService
    {
        private readonly IPersonalizationService _personalizationService;
        private readonly IStorageServiceHandler _localStorage;

        private const string FileName = "EchoWallpaper.jpg";
        private const string FilePath = "ms-appdata:///local/" + FileName;

        public UniversalLockScreenService(IStorageService storageService, IPersonalizationService personalizationService)
        {
            _personalizationService = personalizationService;
            _localStorage = storageService.Local;
        }

        public bool IsProvidedByCurrentApplication { get; } = true;
        public Uri ImageUri { get; set; }
        public Task<LockScreenServiceRequestResult> RequestAccessAsync()
        {
            return _personalizationService.IsSupported 
                ? Task.FromResult(LockScreenServiceRequestResult.Granted) 
                : Task.FromResult(LockScreenServiceRequestResult.Denied);
        }

        public Task RevokeAccessAsync()
        {
            return Task.FromResult(0);
        }

        public async Task SetLockScreen(Uri uri)
        {
            using (var stream = await Echo.GetWallpaperStreamAsync(uri))
            {
                await SetLockScreen(stream);
            }
        }

        public async Task SetLockScreen(Stream stream)
        {
            await _localStorage.WriteAllBytesAsync(FileName, await stream.ToArrayAsync());

            await _personalizationService.SetLockScreenImageAsync(FilePath);
        }

        public Uri ImageUriToUse(Wallpapers wallpapers, WallpaperSize wallpaperSize)
        {
            return wallpapers?.GetUri(wallpaperSize);
        }
    }
}
