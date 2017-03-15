using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.Collections.ObjectModel;
using Windows.UI.Notifications;

using Windows.Security.Credentials;
using Newtonsoft.Json;
using Windows.UI.Xaml;
using Windows.Storage;
using System.IO;
using System.Diagnostics;

namespace SycnRemindBackgroundTask
{
    internal sealed class DatabaseMethod
    {
        public static List<string> ClearRemindItem()
        {

            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "RemindList.db"));

            //foreach (var item in list)
            //{
            //    MyRemind temp = JsonConvert.DeserializeObject<MyRemind>(item.json);
            //    //getDetailClass(ref temp);
            //    temp.Tag = item.Id_system;
            //    temp.ClassDay = ClassMixDay(ref temp);
            //    if (visibility == Visibility.Visible)
            //        temp.Dot = Visibility.Collapsed;
            //    else
            //        temp.Dot = Visibility.Visible;
            //    temp.Rewrite = visibility;
            //    temp.DeleteIcon = visibility;
            //    App.remindList.Add(temp);
            //}
            var list = conn.Table<RemindListDB>();
            List<string> TagList = new List<string>();
            foreach (var item in list)
            {
                string[] itemList =
                    item.Id_system != null ? item.Id_system.Split(',') :new string[0];

                for (int i = 0; i < itemList.Count(); i++)
                {
                    TagList.Add(itemList[i]);
                }
            }
            //var TagList = from x in list where x.Id_system.Equals("") select x.Id_system;

            conn.DropTable<RemindListDB>();
            conn.CreateTable<RemindListDB>();
            conn.Dispose();
            return TagList;
        }
        //public static async void DeleteRemindItem(string tag)
        //{
        //    using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "RemindList.db")))
        //    {
        //        var list = conn.Table<RemindListDB>();
        //        var array = list.Where(i => i.Id_system.Equals(tag));
        //        PasswordCredential user = GetCredential.getCredential("ZSCY");
        //        string stuNum, idNum;
        //        stuNum = user.UserName;
        //        idNum = user.Password;
        //        foreach (var item in array)
        //        {
        //            MyRemind remind = new MyRemind()
        //            {
        //                StuNum = stuNum,
        //                Id = item.Id,
        //                IdNum = idNum
        //            };

        //            await NetWork.httpRequest(ApiUri.deleteRemindApi, NetWork.deleteRemind(remind));
        //        }
        //        list.Delete(i => i.Id_system.Equals(tag));
        //    }
        //    string[] TagArray = tag.Split(',');
        //    var notifier = ToastNotificationManager.CreateToastNotifier();

        //    for (int i = 0; i < TagArray.Count(); i++)
        //    {
        //        var scheduledNotifs = notifier.GetScheduledToastNotifications()
        //      .Where(n => n.Tag.Equals(TagArray[i]));

        //        // Remove all of those from the schedule
        //        foreach (var n in scheduledNotifs)
        //        {
        //            notifier.RemoveFromSchedule(n);
        //        }
        //    }
        //}
        public static ObservableCollection<RemindListDB> ToModel()
        {
            ObservableCollection<RemindListDB> modelList = new ObservableCollection<RemindListDB>();
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "RemindList.db")))
            {
                conn.CreateTable<RemindListDB>();
                var list = conn.Table<RemindListDB>();
                foreach (var item in list)
                {
                    modelList.Add(item);
                }
            }
            return modelList;
        }
        public static RemindListDB ToModel(string id)
        {
            RemindListDB item = new RemindListDB();
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "RemindList.db")))
            {
                item = (from p in conn.Table<RemindListDB>()
                        where p.Id == id
                        select p).FirstOrDefault();
            }
            return item;
        }
        public static void EditDatabase(int num, string id, string json, string id_system)
        {
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "RemindList.db"));
            var up = conn.Table<RemindListDB>();
            //机智的我，删除和插入替换了update
            //bool isfound_id, isfound_id_system;
            //isfound_id = isfound_id_system = true;
            //if (id == null)
            //    isfound_id = false;
            //if (id_system == null)
            //    isfound_id_system = false;
            //if (isfound_id)
            //    up.Delete(x => x.Id.Equals(id));
            //else if (isfound_id_system)
            //    up.Delete(x => x.Id_system.Equals(id_system));
            //else if (isfound_id_system && isfound_id)
            //    up.Delete(x => x.Id .Equals( id) && x.Id_system.Equals( id_system));
            //else
            //{
            //}
            up.Delete(x => x.Num == num);
            RemindListDB temp = new RemindListDB() { Id = id, Id_system = id_system, json = json };
            conn.Insert(temp);
            conn.Dispose();

        }

        //public static void ReadDatabase(Visibility visibility)
        //{
        //    try
        //    {

        //        using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "RemindList.db")))
        //        {

        //            App.remindList.Clear();
        //            var list = conn.Table<RemindListDB>();
        //            foreach (var item in list)
        //            {
        //                MyRemind temp = JsonConvert.DeserializeObject<MyRemind>(item.json);
        //                //getDetailClass(ref temp);
        //                temp.Tag = item.Id_system;
        //                temp.ClassDay = ClassMixDay(ref temp);
        //                if (visibility == Visibility.Visible)
        //                    temp.Dot = Visibility.Collapsed;
        //                else
        //                    temp.Dot = Visibility.Visible;
        //                temp.Rewrite = visibility;
        //                temp.DeleteIcon = visibility;
        //                App.remindList.Add(temp);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        var conn = new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "RemindList.db"));
        //        conn.CreateTable<RemindListDB>();
        //    }
        //}

        //private static string ClassMixDay(ref MyRemind remind)
        //{
        //    string temp = "";
        //    for (int i = 0; i < remind.DateItems.Count; i++)
        //    {
        //        temp += ConvertDay(int.Parse(remind.DateItems[i].Day)) + ConvertClass(int.Parse(remind.DateItems[i].Class)) + "节、";
        //    }

        //    temp = temp.Remove(temp.Length - 1);
        //    return temp;
        //}
        //private static string ConvertClass(int i)
        //{
        //    switch (i)
        //    {
        //        case 0:
        //            return "12";
        //        case 1:
        //            return "34";
        //        case 2:
        //            return "56";
        //        case 3:
        //            return "78";
        //        case 4:
        //            return "910";
        //        case 5:
        //            return "1112";
        //        default:
        //            return "";
        //    }

        //}
        //private static string ConvertDay(int i)
        //{
        //    switch (i)
        //    {
        //        case 0:
        //            return "周一";
        //        case 1:
        //            return "周二";
        //        case 2:
        //            return "周三";
        //        case 3:
        //            return "周四";
        //        case 4:
        //            return "周五";
        //        case 5:
        //            return "周六";
        //        case 6:
        //            return "周日";
        //        default:
        //            return "";
        //    }

        //}
        public static void ToDatabase(string id, string json, string id_system)
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "RemindList.db")))
            {
                conn.CreateTable<RemindListDB>();
                conn.Insert(new RemindListDB() { Id = id, Id_system = id_system, json = json });
            }

        }
        public static string[] id_systemToArray(string id)
        {
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "RemindList.db"));
            var item = (from p in conn.Table<RemindListDB>()
                        where p.Id == id
                        select p).FirstOrDefault();
            conn.Dispose();
            return item.Id_system.Split(',');
        }
    }
}
