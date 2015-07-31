using System;
using System.IO;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Extensions;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Interfaces;

namespace EchoWallpaper.Universal.Services
{
    public class UniversalWallpaperService : IWallpaperService
    {
        private readonly IPersonalizationService _personalizationService;
        private readonly IStorageServiceHandler _localStorage;

        private const string FileName = "EchoWallpaper.jpg";
        private const string FilePath = "ms-appdata:///local/" + FileName;

        public UniversalWallpaperService(IStorageService storageService, IPersonalizationService personalizationService)
        {
            _personalizationService = personalizationService;
            _localStorage = storageService.Local;
        }

        public bool CanSetWallpaper => _personalizationService.IsSupported;

        public async Task SetWallpaper(Uri uri)
        {
            using (var stream = await Echo.GetWallpaperStreamAsync(uri))
            {
                await SetWallpaper(stream);
            }
        }

        public async Task SetWallpaper(Stream stream)
        {
            await _localStorage.WriteAllBytesAsync(FileName, await stream.ToArrayAsync());

            await _personalizationService.SetWallpaperImageAsync(FilePath);
        }
    }
}