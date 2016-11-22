using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using ZSCY_Win10.Models.RemindPage;
using ZSCY_Win10.Service;

namespace RemindBackgroundTask
{
    public sealed class RemindBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            try
            {

                RemindHelp.SyncRemind();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            deferral.Complete();
        }

    }
}
