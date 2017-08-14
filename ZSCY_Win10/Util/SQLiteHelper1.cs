using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ZSCY_Win10.Data;

namespace ZSCY_Win10.Util
{
    class SQLiteHelper1
    {
        public string DbName = "XSQqqData.db";
        public string DbPath;
        internal SQLite.Net.SQLiteConnection GetCreateConn()
        {
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
            var con = new SQLite.Net.SQLiteConnection(new SQLitePlatformWinRT(), DbPath);

            return con;

        }
        internal void CreateDB()
        {
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
            using (var conn = GetCreateConn())
            {
                conn.CreateTable<XSQqqData>();

            }
        }
        internal int AddData(XSQqqData Addqq)
        {
            int result = 0;
            using (var conn = GetCreateConn())
            {
                result = conn.Insert(Addqq);
            }

            return result;
        }
        internal int DeleteData(XSQqqData qqUID)
        {
            int result = 0;
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
            using (var conn = GetCreateConn())
            {
                result = conn.Delete(qqUID);
            }
            return result;
        }
        internal void UpadateData(string deleteSqliteSequence)
        {
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
        }
        internal int UpadateData(XSQqqData updataqqData)
        {
            int result = 0;
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
            using (var conn = GetCreateConn())
            {
                result = conn.Update(updataqqData);
            }
            return result;
        }
        internal List<XSQqqData> CheckData(string conditions)
        {
            string Condition = "%" + conditions + "%";
            #region 
            using (var conn = GetCreateConn())
            {
                return conn.Query<XSQqqData>("select * from XSQqqData where college like?;", Condition);

            }
            #endregion
        }
        internal ObservableCollection<XSQqqData> ReadData(ObservableCollection<XSQqqData> qqList)
        {
            qqList.Clear();
            DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
            CreateDB();
            using (var conn = GetCreateConn())
            {
                var dbFavoriteData = conn.Table<XSQqqData>();
                foreach (var item in dbFavoriteData)
                {
                    qqList.Add(item);
                }
            }
            return qqList;
        }
    }
}
