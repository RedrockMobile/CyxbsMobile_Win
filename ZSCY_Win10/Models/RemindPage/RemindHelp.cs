//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Toolkit.Uwp.Notifications;
//using Windows.UI.Notifications;
//using System.Threading.Tasks;


//namespace ZSCY_Win10.Models.RemindPage
//{
//    public static class RemindHelp
//    {
//        public static async Task AddRemind(MyRemind remind)
//        {
//            remind.Id = Guid.NewGuid();
//            await Task.Run(delegate
//            {
//                addNotification(remind);
//            });
//        }
//        private static void addNotification(MyRemind remind)
//        {
//            ScheduledToastNotification scheduledNotifi = GenerateAlarmNotification(remind);
            
//            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledNotifi);
//        }
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
//                                        Text = $"提醒: {remind.RemindTitle}"
//                                    },

//                                    new AdaptiveText()
//                                    {
//                                        Text = remind.RemindContent
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

//            return new ScheduledToastNotification(content.GetXml(), remind.RemindTime)
//            {
//                Tag = GetTag(remind),

//                // RemoteId is a 1607 feature, if you support older systems, use ApiInformation to check if property is present
                
//                //RemoteId = remoteId
//            };
//        }

//        private static string GetTag(MyRemind remind)
//        {
//           return remind.Id.GetHashCode().ToString();

//        }
//    }
//}
