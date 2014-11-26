using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EchoWallpaper.Core.Model;
using HtmlAgilityPack;

namespace EchoWallpaper.Core
{
    public static class Echo
    {
        private static readonly HttpClient HttpClient;

        private const string DivContainer = "grid_8 flatHtml";
        private const string BaseUrl = "http://www.bournemouthecho.co.uk";

        static Echo()
        {
            HttpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });
        }

        public static async Task<Wallpapers> GetWallpapersAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var htmlResponse = await HttpClient.GetAsync(Constants.WallpaperUrl, cancellationToken);

            htmlResponse.EnsureSuccessStatusCode();

            var html = await htmlResponse.Content.ReadAsStringAsync();

            var wallpapers = ParseHtml(html);


            return wallpapers;
        }

        private static Wallpapers ParseHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var divs = doc.DocumentNode.Descendants("div");

            var imageDiv = GetImageDiv(divs);

            if (imageDiv == null)
            {
                return null;
            }

            var result = new Wallpapers();

            GetPreviewDetails(imageDiv, result);

            GetWindowsWallpapers(imageDiv, result);

            return new Wallpapers();
        }

        private static void GetWindowsWallpapers(HtmlNode imageDiv, Wallpapers result)
        {
            
        }

        private static void GetPreviewDetails(HtmlNode imageDiv, Wallpapers result)
        {
            var image = imageDiv.Descendants("img").FirstOrDefault();
            if (image == null || !image.HasAttributes) return;

            var src = image.Attributes["src"];
            if (src != null)
            {
                result.PreviewImage = CreateUri(src.Value);
            }

            var title = image.Attributes["title"];
            if (title != null)
            {
                result.Title = title.Value;
            }
            else
            {
                var alt = image.Attributes["alt"];
                if (alt != null)
                {
                    result.Title = alt.Value;
                }
            }
            
        }

        private static Uri CreateUri(string resource)
        {
            var url = string.Format("{0}{1}", BaseUrl, resource);
            return new Uri(url);
        }

        private static HtmlNode GetImageDiv(IEnumerable<HtmlNode> divs)
        {
            HtmlNode result = null;
            foreach (var div in divs.ToList())
            {
                if (!div.HasAttributes) continue;

                var classAttribute = div.Attributes.FirstOrDefault(x => x.Value == DivContainer);
                if (classAttribute == null)
                {
                    continue;
                }

                result = div;
                break;
            }
            return result;
        }
    }
}
