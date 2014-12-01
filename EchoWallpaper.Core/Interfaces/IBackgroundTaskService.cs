using System.Threading.Tasks;

namespace EchoWallpaper.Core.Interfaces
{
    public interface IBackgroundTaskService
    {
        bool AgentRunning { get; }
        bool CanRunTask { get; }
        void StopAgent();
        Task CreateAgent();
    }

    public enum TaskRequest
    {
        Granted,
        Denied
    }
}