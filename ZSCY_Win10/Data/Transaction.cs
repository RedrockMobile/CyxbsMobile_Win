using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Data
{
    class Transaction
    {
        public long id { get; set; }
        public int _class { get; set; }
        public int day { get; set; }
        public int time { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public int[] week { get; set; }
        public string[] weekToLesson { get; set; }
        public string classToLesson { get; set;}

        public void GetAttribute(JObject TransationDetailJObject)
        {
            id = (long)TransationDetailJObject["id"];
            _class = (int)TransationDetailJObject["class"];
            day = (int)TransationDetailJObject["day"];
            time = (int)TransationDetailJObject["time"];
            title = TransationDetailJObject["title"].ToString();
            content = TransationDetailJObject["content"].ToString();

            var gradelimit = JArray.Parse(TransationDetailJObject["week"].ToString());
            int[] temp = new int[gradelimit.Count];
            for (int i = 0; i < gradelimit.Count; ++i)
            {
                temp[i] = Int32.Parse(gradelimit[i].ToString());
            }
            week = temp;

            switch (_class)
            {
                case 0:
                    classToLesson = "一二节";
                    break;
                case 1:
                    classToLesson = "三四节";
                    break;
                case 2:
                    classToLesson = "五六节";
                    break;
                case 3:
                    classToLesson = "七八节";
                    break;
                case 4:
                    classToLesson = "九十节";
                    break;
                case 5:
                    classToLesson = "十一十二节";
                    break;
            }
        }
    }
}
