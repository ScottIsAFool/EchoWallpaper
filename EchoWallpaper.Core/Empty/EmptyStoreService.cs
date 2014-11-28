using System.Threading.Tasks;
using Cimbalino.Toolkit.Services;

namespace EchoWallpaper.Core.Empty
{
    public class EmptyStoreService : IStoreService
    {
        public async Task ShowAsync()
        {
        }

        public async Task ShowPublisherAsync(string publisherName)
        {
        }

        public async Task SearchAsync(string keywords)
        {
        }

        public async Task ReviewAsync(string applicationId)
        {
        }
    }
}
