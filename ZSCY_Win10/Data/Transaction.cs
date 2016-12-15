using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Util;

namespace ZSCY_Win10.Data
{
   public class DateInTransaction
    {
        //字段class 很强势
        [JsonProperty(PropertyName = "class")]
        public int _class { get; set; }
        public int day { get; set; }
        public int[] week { get; set; }
        public int weekLength { get; set; }
    }

   public class Transaction
    {
        public long id { get; set; }
        //这个高级用法并没有卵用
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string time { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public List<DateInTransaction> date { get; set; }
        public string classToLesson { get; set; }
        public string week { get; set; }

        public void GetAttribute(JObject TransationDetailJObject)
        {

            //接口改了 现在事项在哪节课/周数/星期存在date集合里 2016年11月17日23:07:23
            id = (long)TransationDetailJObject["id"];

            //time可以为空了 额
            if (TransationDetailJObject["time"] != null)
                time =TransationDetailJObject["time"].ToString();
            else time = "";
            title = TransationDetailJObject["title"].ToString();
            content = TransationDetailJObject["content"].ToString();

            classToLesson = TransationDetailJObject["date"].ToString();

            date = JsonConvert.DeserializeObject<List<DateInTransaction>>(classToLesson);

            List<int> _templist = new List<int>();
            for (int i = 0; i < date.Count; i++)
            {
                _templist = date[i].week.ToList<int>();
                date[i].weekLength = _templist.Count;
            }
            //var gradelimit = JArray.Parse(TransationDetailJObject["date"].ToString());
            // JObject tempJobject = JObject.Parse(gradelimit.ToString());
            //string[] temp = new string[gradelimit.Count];
            //JArray TransasctionArray = Utils.ReadJso(gradelimit.ToString(),"date");

            //for (int i = 0; i < gradelimit.Count; ++i)
            //{
            //    //temp[i] = Int32.Parse(gradelimit[i].ToString());
            //    temp[i].day = (int)tempJobject["day"];
            //    temp[i]._class = (int)tempJobject["class"];
            //}
            //var weeklimit = JArray.Parse(tempJobject["week"].ToString());
            //int[] weektemp = new int[weeklimit.Count];
            //for (int i = 0; i < gradelimit.Count; ++i)
            //{
            //    weektemp[i] = Int32.Parse(weeklimit[i].ToString());
            //}
            //week = temp;

            //switch (_class)
            //{
            //    case 0:
            //        classToLesson = "一二节";
            //        break;
            //    case 1:
            //        classToLesson = "三四节";
            //        break;
            //    case 2:
            //        classToLesson = "五六节";
            //        break;
            //    case 3:
            //        classToLesson = "七八节";
            //        break;
            //    case 4:
            //        classToLesson = "九十节";
            //        break;
            //    case 5:
            //        classToLesson = "十一十二节";
            //        break;
            //}
        }
    }
}
