using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveTileBackgroundTask
{
    class DateInTransaction
    {
        [JsonProperty(PropertyName = "class")]
        public int _Class { get; set; }
        public int Day { get; set; }
        public int[] Week { get; set; }
        public int WeekLength { get; set; }
    }
    class TransactionModel
    {
        public long Id { get; set; }
        //这个高级用法并没有卵用
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Time { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<DateInTransaction> Date { get; set; }
        public string ClassToLesson { get; set; }
        public void GetAttribute(JObject TransationDetailJObject)
        {
            Id = (long)TransationDetailJObject["id"];
            if (TransationDetailJObject["time"] != null)
                Time = TransationDetailJObject["time"].ToString();
            else Time = "";
            Title = TransationDetailJObject["title"].ToString();
            Content = TransationDetailJObject["content"].ToString();
            ClassToLesson = TransationDetailJObject["Date"].ToString();
            Date = JsonConvert.DeserializeObject<List<DateInTransaction>>(ClassToLesson);
            List<int> _templist = new List<int>();
            for (int i = 0; i < Date.Count; i++)
            {
                _templist = Date[i].Week.ToList<int>();
                Date[i].WeekLength = _templist.Count;
            }
        }
    }
}
