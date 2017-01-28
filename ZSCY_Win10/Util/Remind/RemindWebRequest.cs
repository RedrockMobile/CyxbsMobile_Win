using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Models.RemindModels;

namespace ZSCY_Win10.Util.Remind
{
    class RemindWebRequest : NetWork
    {
        public static List<KeyValuePair<string, string>> addRemind(RemindBackupModel myRemind)
        {
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", myRemind.StuNum));
            paramList.Add(new KeyValuePair<string, string>("idNum", myRemind.IdNum));
            string date = "[";

            for (int i = 0; i < myRemind.DateItems.Count; i++)
            {
                string dateJson = JsonConvert.SerializeObject(myRemind.DateItems[i]);
                date += $"{dateJson},";
            }
            date = date.Remove(date.Length - 1) + "]";

            paramList.Add(new KeyValuePair<string, string>("date", date));
            paramList.Add(new KeyValuePair<string, string>("title", myRemind.Title));
            paramList.Add(new KeyValuePair<string, string>("time", myRemind.Time.ToString()));
            paramList.Add(new KeyValuePair<string, string>("content", myRemind.Content));
            return paramList;
        }
        public static List<KeyValuePair<string, string>> editRemind(RemindBackupModel myRemind)
        {
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", myRemind.StuNum));
            paramList.Add(new KeyValuePair<string, string>("idNum", myRemind.IdNum));
            paramList.Add(new KeyValuePair<string, string>("id", myRemind.Id));
            string dateJson = JsonConvert.SerializeObject(myRemind.DateItems);
         
            paramList.Add(new KeyValuePair<string, string>("date", dateJson));
            paramList.Add(new KeyValuePair<string, string>("title", myRemind.Title));
            paramList.Add(new KeyValuePair<string, string>("time", myRemind.Time.ToString()));
            paramList.Add(new KeyValuePair<string, string>("content", myRemind.Content));
            return paramList;
        }
        public static List<KeyValuePair<string, string>> deleteRemind(RemindBackupModel myRemind)
        {
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", myRemind.StuNum));
            paramList.Add(new KeyValuePair<string, string>("idNum", myRemind.IdNum));
            paramList.Add(new KeyValuePair<string, string>("id", myRemind.Id));
            return paramList;
        }
        public static List<KeyValuePair<string,string>> getRemind()
        {
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            var user = GetCredential.getCredential("ZSCY");
            paramList.Add(new KeyValuePair<string, string>("stuNum",user.UserName));
            paramList.Add(new KeyValuePair<string, string>("idNum", user.Password));
            return paramList;
        }
    }
}
