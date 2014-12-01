using System;
using Cimbalino.Toolkit.Extensions;
using Cimbalino.Toolkit.Foundation;
using EchoWallpaper.Core.Model;

namespace EchoWallpaper.Core.Extensions
{
    public static class WallpaperExtensions
    {
        public static Uri GetUri(this Wallpapers wallpapers, WallpaperSize size)
        {
            if (wallpapers == null) return null;

            return wallpapers.GetPropertyValue<Uri>(size.ToString());
        }

        public static WallpaperSize GetWallpaperSize(this Rect screenResolution)
        {
            if(screenResolution.IsEmpty) return WallpaperSize.HdNoCalendar;

            if (screenResolution.Width > screenResolution.Height)
            {
                return HandleLandscapeResolution(screenResolution);
            }
            
            return HandlePortraitResolution(screenResolution);
        }

        private static WallpaperSize HandlePortraitResolution(Rect screenResolution)
        {
            return WallpaperSize.HdNoCalendar;
        }

        private static WallpaperSize HandleLandscapeResolution(Rect screenResolution)
        {
            var scaleFactor = (double)(screenResolution.Width / screenResolution.Height);

            return scaleFactor == 1.6 ? WallpaperSize.NineteenTwentyTenEighty : WallpaperSize.NineteenTwentyTwelveHundred;
        }
    }
}