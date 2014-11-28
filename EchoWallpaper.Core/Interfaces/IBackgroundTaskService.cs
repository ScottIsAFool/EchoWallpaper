namespace EchoWallpaper.Core.Interfaces
{
    public interface IBackgroundTaskService
    {
        bool AgentRunning { get; }
        void StopAgent();
        void CreateAgent();
    }
}