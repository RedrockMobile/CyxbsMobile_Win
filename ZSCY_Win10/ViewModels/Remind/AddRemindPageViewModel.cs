using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Models.RemindModels;
using Windows.UI.Xaml;
using ZSCY_Win10.Util.Remind;
using ZSCY_Win10.Controls;
using System.Diagnostics;
using System.Collections.Generic;
using ZSCY_Win10.Util;
using ZSCY_Win10.Resource;
using Newtonsoft.Json;

namespace ZSCY_Win10.ViewModels.Remind
{
    public class AddRemindPageViewModel : BasePageViewModel
    {
        public AddRemindPageViewModel()
        {
            RemindModel = new AddRemindModel();
            RemindModel.BeforeTime = new List<AddRemindModel._BeforeTimeClass>();
            RemindModel.BeforeTime.AddRange
            (
               new[]
               {
                    new AddRemindModel._BeforeTimeClass() { BeforeTimeString = "不提醒", BeforeTime = new TimeSpan(-1),IconVisibility=Visibility.Collapsed },
                    new AddRemindModel._BeforeTimeClass() { BeforeTimeString = "提前五分钟", BeforeTime = new TimeSpan(0, 5, 0),IconVisibility=Visibility.Collapsed  },
                    new AddRemindModel._BeforeTimeClass() { BeforeTimeString = "提前十分钟", BeforeTime = new TimeSpan(0, 10, 0) ,IconVisibility=Visibility.Collapsed },
                    new AddRemindModel._BeforeTimeClass() { BeforeTimeString = "提前二十分钟", BeforeTime = new TimeSpan(0, 20, 0),IconVisibility=Visibility.Collapsed  },
                    new AddRemindModel._BeforeTimeClass() { BeforeTimeString = "提前一个小时", BeforeTime = new TimeSpan(1, 0, 0) ,IconVisibility=Visibility.Collapsed }
               }
            );

        }
        private AddRemindModel _RemindModel;

        public AddRemindModel RemindModel
        {
            get
            {
                return _RemindModel;
            }

            set
            {
                _RemindModel = value;
                RaisePropertyChanged(nameof(RemindModel));
            }
        }
        public async void AddRemind(string content, string title)
        {
            List<RemindSystemModel> remindSystemList = new List<RemindSystemModel>();
            string weekString = "";
            //List<RemindBackupModel> remindBackupList = new List<RemindBackupModel>();
            weekString = GetSelWeek(content, title, remindSystemList);
            try
            {
                int indexBefore = App.indexBefore;
                string json = "";
                var temp = await AddRemindBakcup(content, title, weekString, indexBefore);
                AddRemindBackModel backInfo=new AddRemindBackModel();
                if (!temp.Item1.Equals(""))
                    backInfo = JsonConvert.DeserializeObject<AddRemindBackModel>(temp.Item1);
                json = temp.Item2;
                var beforeTime = RemindModel.BeforeTime[indexBefore].BeforeTime;
                string localId = "";
                if (indexBefore != 0)
                    localId = await RemindSystemUtil.AddAllRemind(remindSystemList, beforeTime);

                DatabaseMethod.ToDatabase(backInfo.Id.ToString(), json, localId);

            }
            catch (Exception n)
            {
                Debug.WriteLine(n);
            }
        }
       
        private static string GetSelWeek(string content, string title, List<RemindSystemModel> remindSystemList)
        {
            string weekString = "";
            foreach (var week in App.SelWeekList)
            {
                GetSelClassTime(content, title, remindSystemList, week);
                weekString += $"{week + 1},";
               
            }
            weekString = weekString.Remove(weekString.Length - 1);
            return weekString;
        }

        public async void EditRemind(string content, string title, RemindListModel remind)
        {
            string[] tag = remind.Id_system.Split(',');
            RemindSystemUtil.DeleteRemind(tag);
            List<RemindSystemModel> remindSystemList = new List<RemindSystemModel>();
            string weekString = GetSelWeek(content, title, remindSystemList);//设置本地通知的时间转换
            try
            {
                int indexBefore = App.indexBefore;
                string json = "";
                remind.Remind = await EditRemindBackup(content, title, weekString, indexBefore, remind);
                json = JsonConvert.SerializeObject(remind.Remind);
                var beforeTime = RemindModel.BeforeTime[indexBefore].BeforeTime;
                string localId = "";
                if (indexBefore != 0)
                    localId = await RemindSystemUtil.AddAllRemind(remindSystemList, beforeTime);
                DatabaseMethod.EditItem(remind.Num, remind.Id, json, localId);
                //DatabaseMethod.ToDatabase(remind.Id.ToString(), json, localId);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        private static void GetSelClassTime(string content, string title, List<RemindSystemModel> remindSystemList, int week)
        {
            DateTime oneWeekTime = WeekNumClass.OneWeek();
            foreach (var item in App.SelCoursList)
            {

                DateTime remindTime = oneWeekTime.Add(item.NowTime()).AddDays(week * 7);
                if (remindTime < DateTime.Now)
                {
                    continue;
                }
                else
                {
                    RemindSystemModel remindSystem = new RemindSystemModel()
                    {
                        Content = content,
                        Title = title,
                        RemindTime = remindTime
                    };
                    remindSystemList.Add(remindSystem);
                }
            }
        }

        private async Task<Tuple<string, string>> AddRemindBakcup(string content, string title, string week, int indexBefore)
        {
            var user = GetCredential.getCredential("ZSCY");
            RemindBackupModel remindBackup = new RemindBackupModel();
            remindBackup.Content = content;
            remindBackup.Title = title;
            remindBackup.Time = null;
            if (indexBefore != 0)
                remindBackup.Time = Convert.ToInt32(App.addRemindViewModel.RemindModel.BeforeTime[indexBefore].BeforeTime.TotalMinutes);
            remindBackup.DateItems = new List<DateModel>();


            foreach (var item in App.SelCoursList)
            {
                remindBackup.DateItems.Add(new DateModel()
                {
                    Week = week,
                    Class = item.ClassNum.ToString(),
                    Day = item.DayNum.ToString()
                });
            }
            string json = JsonConvert.SerializeObject(remindBackup);
            remindBackup.StuNum = user.UserName;
            remindBackup.IdNum = user.Password;
            string returnString = "";
            try
            {
                returnString = await RemindWebRequest.getHttpWebRequest(Api.AddRemindApi, RemindWebRequest.addRemind(remindBackup), 0, true);
            }
            finally
            {

            }
            return
               new Tuple<string, string>
               (
                returnString,
                json
               );

        }
        private async Task<RemindBackupModel> EditRemindBackup(string content, string title, string week, int indexBefore, RemindListModel remind)
        {
            var user = GetCredential.getCredential("ZSCY");
            RemindBackupModel remindBackup = new RemindBackupModel();
            remindBackup.Content = remind.Remind.Content = content;
            remindBackup.Title = remind.Remind.Title = title;
            remindBackup.Id = remind.Id;
            remindBackup.Time = remind.Remind.Time = null;
            if (indexBefore != 0)
                remindBackup.Time = remind.Remind.Time = Convert.ToInt32(App.addRemindViewModel.RemindModel.BeforeTime[indexBefore].BeforeTime.TotalMinutes);
            remindBackup.DateItems = new List<DateModel>();
            remind.Remind.DateItems.Clear();
            foreach (var item in App.SelCoursList)
            {
                var temp = new DateModel()
                {
                    Week = week,
                    Class = item.ClassNum.ToString(),
                    Day = item.DayNum.ToString()
                };
                remindBackup.DateItems.Add(temp);
                remind.Remind.DateItems.Add(temp);
            }
            remindBackup.StuNum = user.UserName;
            remindBackup.IdNum = user.Password;
            remind.Remind.StuNum = null;
            remind.Remind.IdNum = null;
            await RemindWebRequest.getHttpWebRequest(Api.EditRemindApi, RemindWebRequest.editRemind(remindBackup), 0, true);
            return remind.Remind;
        }
    }
}
