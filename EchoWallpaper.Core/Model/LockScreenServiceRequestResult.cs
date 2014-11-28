namespace EchoWallpaper.Core.Model
{
    /// <summary>
    /// Indicates if the app was successfully or unsuccessfully set as the lock screen background provider.
    /// </summary>
    public enum LockScreenServiceRequestResult
    {
        /// <summary>
        /// The app was not set as the lock screen background provider.
        /// </summary>
        Denied,

        /// <summary>
        /// The app was set as the lock screen background provider.
        /// </summary>
        Granted
    }
}