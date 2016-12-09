using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY.Data
{
    public class NewsList
    {
        private string v;

        public NewsList()
        {
        }

        public NewsList(string id, string articleid, string title, string head, string date, string read, string content, string all)
        {
            ID = id;
            Articleid = articleid;
            Title = title;
            Head = head;
            Date = date;
            Read = read;
            Content = content;
            Content_all = all;
        }

        public string ID { get; set; }
        public string Articleid { get; set; }
        public string Title { get; set; }
        public string Head { get; set; }
        public string Date { get; set; }
        public string Read { get; set; }
        public string Content { get; set; }
        public string Content_all { get; set; }


        public void GetListAttribute(JObject NewsListJObject)
        {
            ID = NewsListJObject["id"].ToString();
            Articleid = NewsListJObject["articleid"].ToString();
            Title = NewsListJObject["title"].ToString();
            Head = NewsListJObject["head"] != null ? NewsListJObject["head"].ToString() : "";
            Date = NewsListJObject["date"].ToString();
            Read = NewsListJObject["read"] != null ? NewsListJObject["read"].ToString() : "";
        }
    }

}
