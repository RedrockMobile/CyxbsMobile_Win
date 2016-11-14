//using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using System.Threading.Tasks;
using System;
using ZSCY_Win10.Models.RemindPage;

namespace ZSCY_Win10.Models.RemindPage
{
    public static class RemindHelp
    {
        public static async Task AddRemind(MyRemind remind)
        {
            remind.Id_system = Guid.NewGuid();
            await Task.Run(delegate
            {
                addNotification(remind);
            });
        }
        private static void addNotification(MyRemind remind)
        {

            ScheduledToastNotification scheduledNotifi = GenerateAlarmNotification(remind);

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledNotifi);
        }

        private static ScheduledToastNotification GenerateAlarmNotification(MyRemind remind)
        {
            ToastContent content = new ToastContent()
            {
                Scenario = ToastScenario.Alarm,

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                                            {
                                                new AdaptiveText()
                                                {
                                                    Text = $"提醒: {remind.Title}"
                                                },

                                                new AdaptiveText()
                                                {
                                                    Text = remind.Content
                                                }
                                            }
                    }
                },
                Actions = new ToastActionsSnoozeAndDismiss()//自动创建一个自动本地化的有延迟提醒时间间隔，贪睡和取消的按钮，贪睡时间由系统自动处理
            };
            return new ScheduledToastNotification(content.GetXml(), remind.time)
            {
                Tag = GetTag(remind)
            };

        }

        private static string id;
        public static string GetTag(MyRemind remind)
        {
            string temp = remind.Id_system.GetHashCode().ToString();
            id += temp + ",";
            // Tag needs to be 16 chars or less, so hash the Id
            return temp;
        }
        public static async Task<string> AddAllRemind(MyRemind remind, TimeSpan beforeTime)
        {
            id = "";
            for (int i = 0; i < App.selectedWeekNumList.Count; i++)
            {
                //TODO 崩溃点
                for (int r = 0; r < 6; r++)
                {
                    for (int c = 0; c < 7; c++)
                    {
                        if (App.timeSet[r, c].IsCheck)
                        {
                            remind.time = App.selectedWeekNumList[i].WeekNumOfMonday.AddDays(c) + App.timeSet[r, c].Time - beforeTime;
                            if (remind.time < DateTime.Now.ToUniversalTime())
                            {

                            }
                            else
                            {
                                await AddRemind(remind);
                            }

                        }
                    }
                }
            }
            return id;
        }
    }

}
//        private static ScheduledToastNotification GenerateAlarmNotification(MyRemind remind)
//        {
//            // Using NuGet package Microsoft.Toolkit.Uwp.Notifications
//            ToastContent content = new ToastContent()
//            {
//                Scenario = ToastScenario.Alarm,

//                Visual = new ToastVisual()
//                {
//                    BindingGeneric = new ToastBindingGeneric()
//                    {
//                        Children =
//                                {
//                                    new AdaptiveText()
//                                    {
//                                        Text = $"提醒: {remind.Title}"
//                                    },

//                                    new AdaptiveText()
//                                    {
//                                        Text = remind.Content
//                                    }
//                                }
//                    }
//                },

//                Actions = new ToastActionsSnoozeAndDismiss()//自动创建一个自动本地化的有延迟提醒时间间隔，贪睡和取消的按钮，贪睡时间由系统自动处理
//            };
//            // We can easily enable Universal Dismiss by generating a RemoteId for the alarm that will be
//            // the same on both devices. We'll just use the alarm delivery time. If an alarm on one device
//            // has the same delivery time as an alarm on another device, it'll be dismissed when one of the
//            // alarms is dismissed.
//            //string remoteId = (remindTime.Ticks / 10000000 / 60).ToString(); // Minutes

//            return new ScheduledToastNotification(content.GetXml(), remind.)
//            {
//                Tag = GetTag(remind),

//                // RemoteId is a 1607 feature, if you support older systems, use ApiInformation to check if property is present

//                //RemoteId = remoteId
//            };
//        }
//        //private static DateTime remindTime(MyRemind remind)
//        //{
//        //    int time = int.Parse(remind.Time); 
//        //    TimeSpan beforeTime= 
//        //}

//    }
//}
