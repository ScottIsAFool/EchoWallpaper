using System;
using Cimbalino.Toolkit.Services;

namespace EchoWallpaper.WindowsPhone.Silverlight.Background.Extensions
{
    public static class DisplayExtensions
    {
        public static ScreenInfoServiceResolution ToScreenResolution(this IDisplayPropertiesService displayProperties)
        {
            if(displayProperties == null) return ScreenInfoServiceResolution.Unknown;

            var physicalScreenResolution = displayProperties.PhysicalBounds;

            var scaleFactor = (int)(physicalScreenResolution.Width / 4.8);
            if (Enum.IsDefined(typeof(ScreenInfoServiceResolution), scaleFactor))
            {
                return (ScreenInfoServiceResolution)scaleFactor;
            }

            return ScreenInfoServiceResolution.Unknown;
        }
    }

    /// <summary>
    /// Describes the device resolution.
    /// </summary>
    public enum ScreenInfoServiceResolution
    {
        /// <summary>
        /// The device has an unknown resolution.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The device has a WVGA (480x800) resolution.
        /// </summary>
        WVGA = 100,

        /// <summary>
        /// The device has a HD 720p (720x1280) resolution.
        /// </summary>
        HD720p = 150,

        /// <summary>
        /// The device has a WXGA (768x1280) resolution.
        /// </summary>
        WXGA = 160,

        /// <summary>
        /// The device has a HD 1080p (1080x1920) resolution.
        /// </summary>
        HD1080p = 225
    }
}
