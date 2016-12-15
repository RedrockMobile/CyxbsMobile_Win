using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY.Data
{
    public class ClassList
    {
        public int coler { get; set; }
        public int Hash_day { get; set; }
        public int Hash_lesson { get; set; }
        public int Begin_lesson { get; set; }
        public string Day { get; set; }
        public string Lesson { get; set; }
        public string Course { get; set; }
        public string Teacher { get; set; }
        public string Classroom { get; set; }
        public string RawWeek { get; set; }
        public string WeekModel { get; set; }
        public int WeekBegin { get; set; }
        public int WeekEnd { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int Period { get; set; }
        public string _Id { get; set; }
        public int[] Week { get; set; }
        public string Classtime { get; set; }

        public int weekLength { get; set; }

        public void GetAttribute(JObject classDetailJObject)
        {
            Hash_day = (int)classDetailJObject["hash_day"];
            Hash_lesson = (int)classDetailJObject["hash_lesson"];
            Begin_lesson = (int)classDetailJObject["begin_lesson"];
            Day = classDetailJObject["day"].ToString();
            Lesson = classDetailJObject["lesson"].ToString();
            Course = classDetailJObject["course"].ToString();
            Teacher = classDetailJObject["teacher"].ToString();
            Classroom = classDetailJObject["classroom"].ToString();
            RawWeek = classDetailJObject["rawWeek"].ToString();
            WeekModel = classDetailJObject["weekModel"].ToString();
            WeekBegin = (int)classDetailJObject["weekBegin"];
            WeekEnd = (int)classDetailJObject["weekEnd"];
            Type = classDetailJObject["type"] != null ? classDetailJObject["type"].ToString() : "";
            if (classDetailJObject["status"] != null)
                Status = classDetailJObject["status"].ToString();
            Period = (int)classDetailJObject["period"];
            _Id = classDetailJObject["_id"] != null ? classDetailJObject["_id"].ToString() : "";
            var gradelimit = JArray.Parse(classDetailJObject["week"].ToString());
            int[] temp = new int[gradelimit.Count];
            weekLength = gradelimit.Count;
            for (int i = 0; i < gradelimit.Count; ++i)
            {
                temp[i] = Int32.Parse(gradelimit[i].ToString());
            }
            Week = temp;
            //Classtime = (RawWeek != "单周" && RawWeek !="双周" ? RawWeek : "") + " " +( WeekModel == "all" ? "" : (WeekModel == "double" ? "双周" : "单周")) ;
            Debug.WriteLine(RawWeek);
        }
    }
}
