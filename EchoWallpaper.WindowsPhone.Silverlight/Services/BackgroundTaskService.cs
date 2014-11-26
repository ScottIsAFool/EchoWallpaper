namespace EchoWallpaper.WindowsPhone.Silverlight.Services
{
    public class BackgroundTaskService
    {
        private static BackgroundTaskService _current;
        public static BackgroundTaskService Current { get { return _current ?? (_current = new BackgroundTaskService()); } }
    }
}
