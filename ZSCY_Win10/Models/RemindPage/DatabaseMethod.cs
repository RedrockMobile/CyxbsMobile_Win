using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.Collections.ObjectModel;

namespace ZSCY_Win10.Models.RemindPage
{
    public static class DatabaseMethod
    {
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
