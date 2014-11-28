using EchoWallpaper.Core.Interfaces;

namespace EchoWallpaper.Services
{
    public class ApplicationInfoService : IApplicationInfoService
    {
        public string Version { get; private set; }
        public string AppId { get; private set; }
    }
}