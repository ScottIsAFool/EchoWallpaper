using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using EchoWallpaper.Core.Interfaces;

namespace EchoWallpaper.Windows.Shared.Services
{
    public class MediaLibraryService : IMediaLibraryService
    {
        public async Task SavePicture(string file, Stream stream)
        {
            var picturesLibrary = KnownFolders.SavedPictures;

            var pictureFile = await picturesLibrary.FileExists(file) ? await picturesLibrary.GetFileAsync(file) : await picturesLibrary.CreateFileAsync(file);

            using (var fileStream = await pictureFile.OpenStreamForWriteAsync())
            {
                await stream.CopyToAsync(fileStream);
            }
        }
    }

    public static class FileExtensions
    {
        public static async Task<bool> FileExists(this IStorageFolder storageFolder, string file)
        {
            try
            {
                await storageFolder.GetFileAsync(file);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}