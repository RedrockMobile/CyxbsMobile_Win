using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using ZSCY_Win10.Models.RemindModels;
using ZSCY_Win10.Resource;

namespace ZSCY_Win10.Util.Remind
{
    class DatabaseMethod
    {
        public static List<string> ClearRemindItem()
        {
            List<string> TagList = new List<string>();

            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
            {
                var list = conn.Table<DataBaseModel>();
                string tagString = "";
                foreach (var item in list)
                {
                    tagString += item.Id_system;
                }
                TagList = tagString.Split(',').ToList<string>();
                ClearDatabase();
            }
            return TagList;
        }
        public static async void DeleteRemindItem(int num)
        {
            string tag = "";
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
            {
                var list = conn.Table<DataBaseModel>();
                var array = list.Where(i => i.Num == num);
                PasswordCredential user = GetCredential.getCredential("ZSCY");
                string stuNum, idNum;

                stuNum = user.UserName;
                idNum = user.Password;
                foreach (var item in array)
                {
                    tag += item.Id_system;
                    RemindBackupModel remind = new RemindBackupModel()
                    {
                        StuNum = stuNum,
                        Id = item.Id,
                        IdNum = idNum
                    };
                    await NetWork.getHttpWebRequest(Api.DeleteRemindApi, RemindWebRequest.deleteRemind(remind), 0, true);
                    conn.Delete<DataBaseModel>(item.Num);
                }

            }
            string[] TagArray = tag.Split(',');
            RemindSystemUtil.DeleteRemind(TagArray);
        }


        public static ObservableCollection<DataBaseModel> ToModel()
        {
            ObservableCollection<DataBaseModel> modelList = new ObservableCollection<DataBaseModel>();

            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
            {
                conn.CreateTable<DataBaseModel>();
                var list = conn.Table<DataBaseModel>();
                foreach (var item in list)
                {
                    modelList.Add(item);
                }
            }
            return modelList;
        }
        public static DataBaseModel ToModel(string id)
        {
            DataBaseModel item = new DataBaseModel();
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
            {
                item = (from p in conn.Table<DataBaseModel>()
                        where p.Id.Equals(id)
                        select p).FirstOrDefault();
            }
            return item;
        }

        #region
        //public static void EditDatabase(int num, string id, string json, string id_system)
        //{
        //    var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath);
        //    var up = conn.Table<RemindListDB>();
        //    //机智的我，删除和插入替换了update
        //    //bool isfound_id, isfound_id_system;
        //    //isfound_id = isfound_id_system = true;
        //    //if (id == null)
        //    //    isfound_id = false;
        //    //if (id_system == null)
        //    //    isfound_id_system = false;
        //    //if (isfound_id)
        //    //    up.Delete(x => x.Id == id);
        //    //else if (isfound_id_system)
        //    //    up.Delete(x => x.Id_system == id_system);
        //    //else if (isfound_id_system && isfound_id)
        //    //    up.Delete(x => x.Id == id && x.Id_system == id_system);
        //    //else
        //    //{

        //    //}
        //    up.Delete(x => x.Num == num);
        //    //if (id != null && id_system == null)
        //    //    up.Delete(x => x.Id == id);
        //    //else if (id == null && id_system != null)
        //    //    up.Delete(x => x.Id_system == id_system);
        //    //else if (id != null && id_system != null)
        //    //    up.Delete(x => x.Id == id && x.Id_system == id_system);
        //    RemindListDB temp = new RemindListDB() { Id = id, Id_system = id_system, json = json };
        //    conn.Insert(temp);


        //}
        //public static void ReadDatabase(Visibility visibility)
        //{
        //    try
        //    {

        //        using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
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
        //        var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath);
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
        #endregion
        public static void ToDatabase(string id, string json, string id_system)
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
            {

                conn.CreateTable<DataBaseModel>();

                conn.Insert(new DataBaseModel() { Id = id, Id_system = id_system, json = json });
            }

        }
        public static void ClearDatabase()
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
            {
                conn.DropTable<DataBaseModel>();
                conn.CreateTable<DataBaseModel>();
            }
        }
        public static void EditItem(int num, string id, string json, string id_system)
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
            {
                conn.Delete<DataBaseModel>(num);
                ToDatabase(id, json, id_system);
            }
        }
        public static string DataReset()
        {
            string tag = "";

            return tag;

        }
    }
}
