using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.Collections.ObjectModel;
using Windows.UI.Notifications;
using ZSCY_Win10.Util;
using Windows.Security.Credentials;

namespace ZSCY_Win10.Models.RemindPage
{
    public static class DatabaseMethod
    {
        public static async void DeleteRemindItem(string tag)
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
            {
                var list = conn.Table<RemindListDB>();
                var array = list.Where(i => i.Id_system.Equals(tag));
                PasswordCredential user = GetCredential.getCredential("ZSCY");
                string stuNum, idNum;
                stuNum = user.UserName;
                idNum = user.Password;
                foreach (var item in array)
                {
                    MyRemind remind = new MyRemind()
                    {
                        StuNum = stuNum,
                        Id = item.Id,
                        IdNum = idNum
                    };

                    await NetWork.httpRequest(ApiUri.deleteRemindApi, NetWork.deleteRemind(remind));
                }
                list.Delete(i => i.Id_system.Equals(tag));
            }
            string[] TagArray = tag.Split(',');
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
        public static ObservableCollection<RemindListDB> ToModel()
        {
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath);
            var list = conn.Table<RemindListDB>();
            ObservableCollection<RemindListDB> modelList = new ObservableCollection<RemindListDB>();
            foreach (var item in list)
            {
                modelList.Add(item);
            }
            return modelList;
        }
        public static RemindListDB ToModel(string id)
        {
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath);
            var item = (from p in conn.Table<RemindListDB>()
                        where p.Id == id
                        select p).FirstOrDefault();
            return item;
        }
        public static void EditDatabase(string id, string json, string id_system)
        {
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath);
            var up = conn.Table<RemindListDB>();
            //机智的我，删除和插入替换了update

            up.Delete(x => x.Id == id || x.Id_system == id_system);
                RemindListDB temp = new RemindListDB() { Id = id, Id_system = id_system, json = json };
            conn.Insert(temp);


        }
        public static void ToDatabase(string id, string json, string id_system)
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
            {
                conn.CreateTable<RemindListDB>();
                conn.Insert(new RemindListDB() { Id = id, Id_system = id_system, json = json });
            }

        }
        public static string[] id_systemToArray(string id)
        {
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath);
            var item = (from p in conn.Table<RemindListDB>()
                        where p.Id == id
                        select p).FirstOrDefault();
            return item.Id_system.Split(',');
        }
    }
}
