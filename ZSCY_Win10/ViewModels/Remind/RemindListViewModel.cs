using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Models.RemindModels;
using ZSCY_Win10.Util.Remind;

namespace ZSCY_Win10.ViewModels.Remind
{
    public class RemindListViewModel
    {
        public RemindListViewModel()
        {
            RemindListOC = new ObservableCollection<RemindListModel>();
           
        }
        private ObservableCollection<RemindListModel> _RemindListOC;

        internal ObservableCollection<RemindListModel> RemindListOC
        {
            get
            {
                return _RemindListOC;
            }

            set
            {
                _RemindListOC = value;
            }
        }
        public void DeleteRemind(int num)
        {
            DatabaseMethod.DeleteRemindItem(num);
        }
        public void Rewrite()
        {

            foreach (var item in RemindListOC)
            {
                item.IsRewrite = item.IsRewrite ? false : true;
            }
        }

        public async void RefreshList(bool e=false)
        {
            bool isRewrite;
            if (e)
            {
                isRewrite = e;
            }
            else
             isRewrite = RemindListOC.Count > 0 ? RemindListOC[0].IsRewrite : false;
            RemindListOC.Clear();
            var remindList = await RemindSystemUtil.SyncRemindList();
            DatabaseMethod.ClearDatabase();
            int num = 1;
            foreach (var item in remindList)
            {
                RemindListModel temp = new RemindListModel();
                temp.Remind = item;
                temp.Num = num++;
                temp.IsRewrite = isRewrite;
                temp.Id = item.Id;
                temp.json = JsonConvert.SerializeObject(item);
                temp.ClassTime();
                if (item.Time != null)
                {
                    int hour, min;
                    hour = (int)item.Time / 60;
                    min = (int)item.Time % 60;
                    TimeSpan beforeTime = new TimeSpan(hour, min, 0);
                    temp.Id_system = await RemindSystemUtil.AddAllRemind(OnceRemind(temp.Remind), beforeTime);
                }
                RemindListOC.Add(temp);
                DatabaseMethod.ToDatabase(temp.Id, temp.json, temp.Id_system);
            }
        }
        public void ReloadList()
        {
            RemindListOC.Clear();
            var DBList = DatabaseMethod.ToModel();
            foreach (var item in DBList)
            {
                RemindListModel temp = new RemindListModel()
                {
                    Id = item.Id,
                    Id_system = item.Id_system,
                    Num = item.Num,
                    json = item.json,
                    IsRewrite = true
                };
                temp.JsonToModel();
                temp.ClassTime();
                RemindListOC.Add(temp);
            }
        }
        private List<RemindSystemModel> OnceRemind(RemindBackupModel remind)
        {

            DateTime oneWeekTime = WeekNumClass.OneWeek();
            List<RemindSystemModel> remindList = new List<RemindSystemModel>();
            foreach (var item in remind.DateItems)
            {
                string[] weekNum = item.Week.Split(',');
                for (int i = 0; i < weekNum.Length; i++)
                {
                    int num = int.Parse(weekNum[i]);
                    SelCourseModel selCourse = new SelCourseModel(int.Parse(item.Day), int.Parse(item.Class));
                    DateTime remindTime = oneWeekTime.AddDays((num - 1) * 7).Add(selCourse.NowTime());
                    if (remindTime < DateTime.Now)
                    {
                        continue;
                    }
                    else
                    {
                        RemindSystemModel remindSystem = new RemindSystemModel()
                        {
                            Content = remind.Content,
                            Title = remind.Title,
                            RemindTime = remindTime
                        };
                        remindList.Add(remindSystem);
                    }
                }
            }
            return remindList;
        }

        public void GetData()
        {
            RemindListOC.Clear();

            var DBList = DatabaseMethod.ToModel();
            foreach (var item in DBList)
            {
                RemindListModel temp = new RemindListModel()
                {
                    Id = item.Id,
                    Id_system = item.Id_system,
                    Num = item.Num,
                    json = item.json,
                    IsRewrite = false
                };
                temp.JsonToModel();
                temp.ClassTime();
                RemindListOC.Add(temp);
            }
        }
    }
}
