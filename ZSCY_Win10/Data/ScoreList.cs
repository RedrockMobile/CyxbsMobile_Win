﻿using Newtonsoft.Json.Linq;

namespace ZSCY.Data
{
    internal class ScoreList
    {
        public string Student { get; set; }
        public string Course { get; set; }
        public string Grade { get; set; }
        public string Property { get; set; }
        public string Status { get; set; }
        public string Term { get; set; }

        public void GetAttribute(JObject scoreDetailJObject)
        {
            Student = scoreDetailJObject["student"].ToString();
            Course = scoreDetailJObject["course"].ToString();
            Grade = scoreDetailJObject["grade"].ToString();
            Property = scoreDetailJObject["property"].ToString();
            Status = scoreDetailJObject["status"].ToString();
            Term = scoreDetailJObject["term"].ToString();
        }
    }
}