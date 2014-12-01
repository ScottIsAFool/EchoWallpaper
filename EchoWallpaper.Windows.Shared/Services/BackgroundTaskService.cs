using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Interfaces;

namespace EchoWallpaper.Windows.Shared.Services
{
    public class BackgroundTaskService : IBackgroundTaskService
    {
        public static BackgroundTaskService Current { get; private set; }

        public BackgroundTaskService()
        {
            Current = this;
        }

        public bool AgentRunning
        {
            get
            {
                var task = BackgroundTaskRegistration.AllTasks.FirstOrDefault(x => x.Value.Name == Constants.BackgroundAgentName);
                return task.Value != null;
            }
        }

        public bool CanRunTask
        {
            get
            {
                var status = BackgroundExecutionManager.GetAccessStatus();
                return status != BackgroundAccessStatus.Denied && status != BackgroundAccessStatus.Unspecified;
            }
        }

        public void StopAgent()
        {
            BackgroundExecutionManager.RemoveAccess();
        }

        public async Task CreateAgent()
        {
            if (!AgentRunning)
            {
                if (!CanRunTask)
                {
                    var status = await BackgroundExecutionManager.RequestAccessAsync();
                    switch (status)
                    {
                        case BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity:
                        case BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity:
                            CreateTask();
                            break;
                        default:
                            return;
                    }
                }
                else
                {
                    CreateTask();
                }
            }
        }

        private void CreateTask()
        {
            var task = BackgroundTaskRegistration.AllTasks.FirstOrDefault(x => x.Value.Name == Constants.BackgroundAgentName);
            if (task.Value == null)
            {
                var taskBuilder = new BackgroundTaskBuilder
                {
                    Name = Constants.BackgroundAgentName,
                    TaskEntryPoint = Constants.BackgroundAgentEntryPoint
                };

                var trigger = new TimeTrigger(720, false);
                taskBuilder.SetTrigger(trigger);
                taskBuilder.Register();
            }
        }
    }
}
