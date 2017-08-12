using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Notifications;
using ZSCY_Win10.Models.RemindModels;
using ZSCY_Win10.Resource;

namespace ZSCY_Win10.Util.Remind
{
    class RemindSystemUtil
    {

        private static async Task AddRemind(RemindSystemModel remind, TimeSpan beforeTime)
        {
            remind.Id = Guid.NewGuid();
            await Task.Run(delegate
            {
                addNotification(remind, beforeTime);
            });
        }
        private static void addNotification(RemindSystemModel remind, TimeSpan beforeTime)
        {

            ScheduledToastNotification scheduledNotifi = GenerateAlarmNotification(remind, beforeTime);

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledNotifi);
        }

        private static ScheduledToastNotification GenerateAlarmNotification(RemindSystemModel remind, TimeSpan beforeTime)
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
                Actions = new ToastActionsSnoozeAndDismiss()  //自动创建一个自动本地化的有延迟提醒时间间隔，贪睡和取消的按钮，贪睡时间由系统自动处理
            };
            return new ScheduledToastNotification(content.GetXml(), remind.RemindTime - beforeTime)
            {
                Tag = GetTag(remind)
            };

        }
        [DataContract]
        private class GetRemindData
        {
            [DataMember(Name = "data")]
            public List<DataModel> DataList;
            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "status")]
            public int Status { get; set; }

            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "info")]
            public string Info { get; set; }

            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "term")]
            public int Term { get; set; }

            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "stuNum")]
            public long StuNum { get; set; }
        }
        [DataContract]
        private class DataModel
        {
            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "id")]
            public long Id { get; set; }

            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "time")]
            public int? Time { get; set; }

            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "title")]
            public string Title { get; set; }

            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "content")]
            public string Content { get; set; }

            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "updated_time")]
            public string Updated_Time { get; set; }

            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "date")]
            public List<DateItemModel> DateItems { get; set; }
          
        }
        [DataContract]
        private class DateItemModel
        {
            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "class")]
            public int Class { get; set; }

            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "day")]
            public int Day { get; set; }

            ///<summary>
            /// 
            /// </summary>
            [DataMember(Name = "week")]
            public List<int> WeekItems { get; set; }
        }
        private static void GetRemindJsonToModel(ref ObservableCollection<RemindBackupModel> remindList, string json)
        {
            //GetRemindData remindData = new GetRemindData();
            //remindData.DataList = new List<DataModel>();
            //remindData = JsonConvert.DeserializeObject<GetRemindData>(json);
            var remindData = JsonConvert.DeserializeObject<GetRemindData>(json);
            foreach (var item in remindData.DataList)
            {
                RemindBackupModel temp = new RemindBackupModel();
                temp.Content = item.Content;
                temp.Title = item.Title;
                temp.Id = item.Id.ToString();
                temp.Time = item.Time;
                temp.DateItems = new List<DateModel>();
                foreach (var dateItem in item.DateItems)
                {
                    string week = "";
                    for(int i=0;i<dateItem.WeekItems.Count;i++)
                    {
                        week += dateItem.WeekItems[i] + ",";
                    }
                    week = week.Remove(week.Length - 1);
                    temp.DateItems.Add(new DateModel()
                    {
                        Day = dateItem.Day.ToString(),
                        Class = dateItem.Class.ToString(),
                        Week = week
                    });
                }
                remindList.Add(temp);
            }
        }



        private static string id;
        public static string GetTag(RemindSystemModel remind)
        {
            string temp = remind.Id.GetHashCode().ToString();
            id += temp + ",";
            // Tag needs to be 16 chars or less, so hash the Id
            return temp;
        }
        public static void DeleteRemind(string[] TagArray)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();

            for (int i = 0; i < TagArray.Count(); i++)
            {
                var scheduledNotifs = notifier.GetScheduledToastNotifications()
              .Where(n => n.Tag.Equals(TagArray[i]));

                // Remove all of those from the schedule
                foreach (var n in scheduledNotifs)
                {
                    notifier.RemoveFromSchedule(n);
                }
            }
        }

        public static async Task<string> AddAllRemind(List<RemindSystemModel> remind, TimeSpan beforeTime)
        {
            id = "";
            foreach (var item in remind)
            {
                await AddRemind(item, beforeTime);
            }
            return id;
        }
        public static async Task<string> AddEditRemind(List<RemindSystemModel> remind,TimeSpan beforeTime)
        {
            id = "";
            foreach(var item in remind)
            {
                await AddRemind(item, beforeTime);
            }
            return id;
        }
        public static async Task<ObservableCollection<RemindBackupModel>> SyncRemindList()
        {
            ObservableCollection<RemindBackupModel> remindList = new ObservableCollection<RemindBackupModel>();
            List<KeyValuePair<string, string>> paramList = RemindWebRequest.getRemind();
            string json = await NetWork.getHttpWebRequest(Api.GetRemindApi,paramList, 0, true);
            GetRemindJsonToModel(ref remindList, json);
            return remindList;

        }
        public static async Task<string> SyncAllRemind(RemindSystemModel remind)
        {
            id = "";
          
            return id;
        }
        //    public static async void SyncRemind()
        //    {
        //        List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
        //        PasswordCredential user = GetCredential.getCredential("ZSCY");
        //        paramList.Add(new KeyValuePair<string, string>("stuNum", user.UserName));
        //        paramList.Add(new KeyValuePair<string, string>("idNum", user.Password));
        //        string content = "";
        //        try
        //        {
        //            content = await NetWork.httpRequest(ApiUri.getRemindApi, paramList);
        //        }
        //        catch
        //        {

        //            Debug.WriteLine("网络问题请求失败");
        //        }
        //        //相当于MyRemind
        //        GetRemindModel getRemid = JsonConvert.DeserializeObject<GetRemindModel>(content);

        //        try
        //        {

        //            getRemid = await JsonConvert.DeserializeObjectAsync<GetRemindModel>(content);
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.Write(e);
        //        }
        //        List<string> getRemindList_json = new List<string>();
        //        List<MyRemind> remindList = new List<MyRemind>();
        //        #region 同步返回的json格式和添加的风格不一样，需要转换
        //        foreach (var item in getRemid.DataItems)
        //        {
        //            //getRemindList_json.Add(getRemid.DataItems[0].Id.ToString());
        //            MyRemind mr = new MyRemind();
        //            List<DateItemModel> dim = new List<DateItemModel>();
        //            //每个MyRemind的date
        //            foreach (var itemData in item.DateItems)
        //            {
        //                DateItemModel dateitme = new DateItemModel();
        //                string week = "";
        //                foreach (var itemWeek in itemData.WeekItems)
        //                {
        //                    week += itemWeek + ",";
        //                }
        //                week = week.Remove(week.Length - 1);
        //                dateitme.Class = itemData.Class.ToString();
        //                dateitme.Day = itemData.Day.ToString();
        //                dateitme.Week = week;
        //                dim.Add(dateitme);
        //            }
        //            mr.Title = item.Title;
        //            mr.Content = item.Content;
        //            mr.DateItems = dim;
        //            mr.Time = item.Time;
        //            mr.Id = item.Id.ToString();
        //            remindList.Add(mr);

        //        }
        //        #endregion
        //        List<string> RemindTagList = new List<string>();
        //        RemindTagList = DatabaseMethod.ClearRemindItem() as List<string>;
        //        var notifier = ToastNotificationManager.CreateToastNotifier();
        //        if (RemindTagList != null)
        //        {

        //            for (int i = 0; i < RemindTagList.Count(); i++)
        //            {
        //                var scheduledNotifs = notifier.GetScheduledToastNotifications()
        //              .Where(n => n.Tag.Equals(RemindTagList[i]));

        //                // Remove all of those from the schedule
        //                foreach (var n in scheduledNotifs)
        //                {
        //                    notifier.RemoveFromSchedule(n);
        //                }
        //            }
        //        }

        //        foreach (var remindItem in remindList)
        //        {
        //            string id = await SyncAllRemind(remindItem);
        //            DatabaseMethod.ToDatabase(remindItem.Id, JsonConvert.SerializeObject(remindItem), id);
        //        }
        //        DatabaseMethod.ReadDatabase(Windows.UI.Xaml.Visibility.Collapsed);


        //}

    }
}
