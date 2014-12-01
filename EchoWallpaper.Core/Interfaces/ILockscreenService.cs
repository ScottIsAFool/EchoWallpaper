using System;
using System.IO;
using System.Threading.Tasks;
using EchoWallpaper.Core.Model;

namespace EchoWallpaper.Core.Interfaces
{
    public interface ILockScreenService
    {
        /// <summary>
        /// Gets a value indicating whether the app is the current lock screen background provider.
        /// </summary>
        /// <value>true if the app is the current lock screen background provider; otherwise, false.</value>
        bool IsProvidedByCurrentApplication { get; }

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) of the current lock screen background.
        /// </summary>
        /// <value>The Uniform Resource Identifier (URI) of the current lock screen background.</value>
        Uri ImageUri { get; set; }

        /// <summary>
        /// Sets the current app as the lock screen background provider.
        /// </summary>
        /// <returns>The <see cref="Task"/> object representing the asynchronous operation.</returns>
        Task<LockScreenServiceRequestResult> RequestAccessAsync();

        /// <summary>
        /// Revokes the access.
        /// </summary>
        /// <returns></returns>
        Task RevokeAccessAsync();

        Task SetLockScreen(Uri uri);
        Task SetLockScreen(Stream stream);
        Uri ImageUriToUse(Wallpapers wallpapers);
    }
}