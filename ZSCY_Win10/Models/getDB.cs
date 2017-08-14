using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.IO;

namespace ZSCY.Models
{
    public static class getDB
    {
        public readonly static string DBPath = Path.Combine("DataBase/qq_Group.db");

        public static SQLiteConnection GetDblxqConnection()
        {
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DBPath);
            conn.CreateTable<LXQ>();

            return conn;
        }

        public static SQLiteConnection GetDbxsqConnection()
        {
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DBPath);
            conn.CreateTable<XYXS>();

            return conn;
        }
    }
}