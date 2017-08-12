using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY.Data
{
    public class ExamList
    {
        public string Student { get; set; }
        public string Course { get; set; }
        public string Classroom { get; set; }
        public string Seat { get; set; }
        public string Week { get; set; }
        public string Weekday { get; set; }
        public string Begin_time { get; set; }
        public string End_time { get; set; }
        public string Status { get; set; }
        public string Term { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string DateTime { get; set; }

        public void GetAttribute(JObject ExamDetailJObject)
        {
            Student = ExamDetailJObject["student"].ToString();
            Course = ExamDetailJObject["course"].ToString();
            Classroom = ExamDetailJObject["classroom"].ToString();
            Seat = ExamDetailJObject["seat"].ToString() + "号座位";
            Week = ExamDetailJObject["week"] == null ? "" : ExamDetailJObject["week"].ToString();
            Weekday = ExamDetailJObject["weekday"] == null ? "" : ExamDetailJObject["weekday"].ToString();
            Begin_time = ExamDetailJObject["begin_time"] == null ? "" : ExamDetailJObject["begin_time"].ToString();
            End_time = ExamDetailJObject["end_time"] == null ? "" : ExamDetailJObject["end_time"].ToString();
            Status = ExamDetailJObject["status"] == null ? "" : ExamDetailJObject["status"].ToString();
            Term = ExamDetailJObject["term"] == null ? "" : ExamDetailJObject["term"].ToString();
            Date = ExamDetailJObject["date"] == null ? "" : ExamDetailJObject["date"].ToString();
            Time = ExamDetailJObject["time"] == null ? "" : ExamDetailJObject["time"].ToString();
        }
    }
}
