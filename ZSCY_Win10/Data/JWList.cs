using Newtonsoft.Json.Linq;

namespace ZSCY.Data
{
    public class JWList
    {
        public JWList()
        {
        }

        public JWList(string iD, string title, string date, string read, string Content)
        {
            ID = iD;
            Title = title;
            Date = date;
            Read = read;
            this.Content = Content;
        }

        public string ID { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Read { get; set; }
        public string Content { get; set; }

        public void GetListAttribute(JObject JWListJObject)
        {
            ID = JWListJObject["id"].ToString();
            Title = JWListJObject["title"].ToString();
            Date = JWListJObject["date"].ToString();
            Read = JWListJObject["read"].ToString();
            //Content = JWListJObject["content"].ToString();
        }
    }
}