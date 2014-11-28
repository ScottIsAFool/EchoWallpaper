using EchoWallpaper.Core.Interfaces;

namespace EchoWallpaper.Core.Empty
{
    public class EmptyApplicationInfoService : IApplicationInfoService
    {
        public string Version { get; private set; }
        public string AppId { get; private set; }
    }
}