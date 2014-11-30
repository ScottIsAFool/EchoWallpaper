using Windows.Storage;

namespace EchoWallpaper.Windows.Shared.Extensions
{
    public static class ApplicationDataExtensions
    {
        public static T Get<T>(this ApplicationDataContainer container, string key)
        {
            if (container.Values.ContainsKey(key))
            {
                return (T) container.Values[key];
            }

            return default(T);
        }
    }
}
