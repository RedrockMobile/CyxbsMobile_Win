using Newtonsoft.Json.Linq;
using System;

namespace ZSCY_Win10.Data
{
    internal class NewsContentList
    {
        public class Rootobject
        {
            public int state { get; set; }
            public string info { get; set; }
            public string id { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public string title { get; set; }
            public string content { get; set; }
            public Annex[] annex { get; set; }
            public string date { get; set; }
            public object unit { get; set; }
        }

        public class Annex
        {
            public string name { get; set; }
            public string address { get; set; }
            public Uri Anneximg { get; set; }

            public void GetAttribute(JObject AnnexDetailJObject)
            {
                name = AnnexDetailJObject["name"].ToString();
                address = AnnexDetailJObject["address"].ToString();
            }
        }
    }
}