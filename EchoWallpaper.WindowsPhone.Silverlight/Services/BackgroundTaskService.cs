using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EchoWallpaper.Core;
using EchoWallpaper.Core.Interfaces;
using Microsoft.Phone.Scheduler;

namespace EchoWallpaper.WindowsPhone.Silverlight.Services
{
    public class BackgroundTaskService : IBackgroundTaskService
    {
        private static BackgroundTaskService _current;
        public static BackgroundTaskService Current { get { return _current ?? (_current = new BackgroundTaskService()); } }

        public bool AgentRunning
        {
            get { return GetAgent() != null; }
        }

        public bool CanRunTask { get { return true; } }

        private static ScheduledAction GetAgent()
        {
            return ScheduledActionService.Find(Constants.BackgroundAgentName);
        }

        public void StopAgent()
        {
            if (AgentRunning)
            {
                ScheduledActionService.Remove(Constants.BackgroundAgentName);
            }
        }

        public async Task CreateAgent()
        {
            if (!AgentRunning)
            {
                var task = new PeriodicTask(Constants.BackgroundAgentName) { Description = "Automatically set your lockscreen wallpaper to that of a Bournemouth Echo wallpaper" };

                ScheduledActionService.Add(task);
            }

            TestScheduledEvent();
        }

        [Conditional("DEBUG")]
        private void TestScheduledEvent()
        {
            if (AgentRunning)
            {
                ScheduledActionService.LaunchForTest(Constants.BackgroundAgentName, new TimeSpan(0, 0, 1));
            }
        }
    }
}
