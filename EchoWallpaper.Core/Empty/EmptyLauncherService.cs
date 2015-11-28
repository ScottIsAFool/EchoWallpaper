using System;
using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;

namespace EchoWallpaper.Core.Empty
{
    public class EmptyLauncherService : ILauncherService
    {
        public async Task LaunchUriAsync(Uri uri)
        {
        }

        public async Task LaunchUriAsync(string url)
        {
        }

        public async Task LaunchFileAsync(string file)
        {
        }
    }
}
