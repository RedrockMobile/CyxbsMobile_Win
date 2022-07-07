using Newtonsoft.Json;
using System.Collections.Generic;
using ZSCY_Win10.Models.RemindModels;

namespace ZSCY_Win10.Util.Remind
{
    internal class RemindWebRequest : Requests
    {
        public static Dictionary<string, string> addRemind(RemindBackupModel myRemind)
        {
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            string date = "[";

            for (int i = 0; i < myRemind.DateItems.Count; i++)
            {
                string dateJson = JsonConvert.SerializeObject(myRemind.DateItems[i]);
                date += $"{dateJson},";
            }
            date = date.Remove(date.Length - 1) + "]";

            paramList.Add("date", date);
            paramList.Add("title", myRemind.Title);
            paramList.Add("time", myRemind.Time.ToString());
            paramList.Add("content", myRemind.Content);
            return paramList;
        }

        public static Dictionary<string, string> editRemind(RemindBackupModel myRemind)
        {
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            paramList.Add("id", myRemind.Id);
            string dateJson = JsonConvert.SerializeObject(myRemind.DateItems);

            paramList.Add("date", dateJson);
            paramList.Add("title", myRemind.Title);
            paramList.Add("time", myRemind.Time.ToString());
            paramList.Add("content", myRemind.Content);
            return paramList;
        }

        public static Dictionary<string, string> deleteRemind(RemindBackupModel myRemind)
        {
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            paramList.Add("id", myRemind.Id);
            return paramList;
        }
    }
}