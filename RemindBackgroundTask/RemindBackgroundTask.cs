using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;


namespace RemindBackgroundTask3
{
    public sealed class RemindBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

                RemindHelp.SyncRemind();
                deferral.Complete();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

    }
}
