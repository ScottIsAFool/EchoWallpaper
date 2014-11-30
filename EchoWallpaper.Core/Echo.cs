using System;
using System.Collections.Generic;
using System.IO;
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

        public static async Task<Stream> GetWallpaperStreamAsync(Uri uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await HttpClient.GetAsync(uri, cancellationToken);

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            return stream;
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

            GetMobileWallpapers(imageDiv, result);

            return result;
        }

        private static void GetMobileWallpapers(HtmlNode imageDiv, Wallpapers result)
        {
            var liContainers = imageDiv.Descendants("li");

            foreach(var li in liContainers)
            {
                var container = li.Descendants("a").FirstOrDefault();
                if (container == null || !container.HasAttributes) continue;

                var href = container.Attributes["href"];
                if (href != null)
                {
                    var resolution = container.InnerText;
                    var uri = CreateUri(href.Value);
                    switch (resolution.Trim())
                    {
                        case "iPad, landscape with calendar":
                            result.IpadLandscape = uri;
                            break;
                        case "iPad, landscape with no calendar":
                            result.IpadLandscapeNoCalendar = uri;
                            break;
                        case "iPad upright, no calendar":
                            result.IpadPortraitNoCalendar = uri;
                            break;
                        case "iPad mini, no calendar":
                            result.IpadMiniNoCalendar = uri;
                            break;
                        case "1136x640 landscape with calendar":
                            result.AndroidLandscape = uri;
                            break;
                        case "1080x1920 mobile phone upright, no calendar":
                            result.HdNoCalendar = uri;
                            break;
                        case "iPhone, no calendar":
                            result.IphoneNoCalendar = uri;
                            break;
                        case "720x1280":
                            result.SevenTwentyP = uri;
                            break;
                        case "768x1280":
                            result.Wxga = uri;
                            break;
                        case "480x800":
                            result.Wvga = uri;
                            break;
                    }
                }
            }
        }

        private static void GetWindowsWallpapers(HtmlNode imageDiv, Wallpapers result)
        {
            var pContainer = imageDiv.Descendants("p").FirstOrDefault(x => x.InnerText.Contains("1920 x 1200"));
            if (pContainer == null) return;

            var aContainers = pContainer.Descendants("a").ToList();
            foreach (var container in aContainers)
            {
                if (!container.HasAttributes) continue;
                var href = container.Attributes["href"];
                if (href != null)
                {
                    var resolution = container.InnerText;
                    var uri = CreateUri(href.Value);
                    switch (resolution.Trim())
                    {
                        case "1920 x 1200":
                            result.NineteenTwentyTwelveHundred = uri;
                            break;
                        case "1920 x 1080":
                            result.NineteenTwentyTenEighty = uri;
                            break;
                        case "1280 x 1024":
                            result.TwelveEightyTenTwentyFour = uri;
                            break;
                        case "1024 x 768":
                            result.TenTwentyFourSevenSixtyEight = uri;
                            break;
                        case "1280 x 768":
                            result.TwelveEightySevenSixtyEight = uri;
                            break;
                        case "1280 x 720":
                            result.TwelveEightySevenTwenty = uri;
                            break;
                    }
                }
            }
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
