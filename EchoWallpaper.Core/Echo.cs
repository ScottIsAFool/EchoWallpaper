using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EchoWallpaper.Core.Model;
using Newtonsoft.Json;

namespace EchoWallpaper.Core
{
    public static class Echo
    {
        private static readonly HttpClient HttpClient;

        static Echo()
        {
            HttpClient = new HttpClient(new HttpClientHandler
            {
                //AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });
        }

        public static async Task<Wallpapers> GetWallpapersAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var htmlResponse = await HttpClient.GetAsync(Constants.WallpaperUrl, cancellationToken);

            htmlResponse.EnsureSuccessStatusCode();

            var json = await htmlResponse.Content.ReadAsStringAsync();

            var wallpapers = JsonConvert.DeserializeObject<Wallpapers>(json);

            return wallpapers;
        }

        public static async Task<Stream> GetWallpaperStreamAsync(Uri uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await HttpClient.GetAsync(uri, cancellationToken);

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            return stream;
        }
    }
}
