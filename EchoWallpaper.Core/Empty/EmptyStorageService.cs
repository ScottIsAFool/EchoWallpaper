using Cimbalino.Toolkit.Services;

namespace EchoWallpaper.Core.Empty
{
    public class EmptyStorageService : IStorageService
    {
        public IStorageServiceHandler Local { get; private set; }
        public IStorageServiceHandler Roaming { get; private set; }
        public IStorageServiceHandler Temporary { get; private set; }
        public IStorageServiceHandler LocalCache { get; private set; }
        public IStorageServiceHandler Package { get; private set; }
    }
}
