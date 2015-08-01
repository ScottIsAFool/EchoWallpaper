using System;
using Windows.UI.Xaml.Data;
using Cimbalino.Toolkit.Services;
using EchoWallpaper.Core.Model;

namespace EchoWallpaper.Universal.Converters
{
    public class PreviewImageUrlConverter : IValueConverter
    {
        private static readonly DisplayPropertiesService Display = new DisplayPropertiesService();
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var wallpapers = value as Wallpapers;
            if (wallpapers == null) return null;
            
#if WINDOWS_PHONE_APP
            return wallpapers.PreviewImage;
#else
            var dpi = Display.LogicalDpi;

            return wallpapers.NineteenTwentyTenEighty;
#endif
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
