using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
namespace SycnRemindBackgroundTask
{
    public sealed class RemindBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            RemindHelp.SyncRemind();
            deferral.Complete();
        }
    }
}
