using System;
using Cimbalino.Toolkit.Extensions;
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
    }
}