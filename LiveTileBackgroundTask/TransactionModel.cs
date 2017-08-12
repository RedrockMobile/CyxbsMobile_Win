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
        //字段class 很强势
        [JsonProperty(PropertyName = "class")]
        public int _Class { get; set; }
        public int Day { get; set; }
        public int[] Week { get; set; }
        public int WeekLength { get; set; }
    }

    class Transaction
    {
        public long id { get; set; }
        //这个高级用法并没有卵用
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Time { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<DateInTransaction> Date { get; set; }
        public string ClassToLesson { get; set; }

        public void GetAttribute(JObject TransationDetailJObject)
        {

            //接口改了 现在事项在哪节课/周数/星期存在date集合里 2016年11月17日23:07:23
            id = (long)TransationDetailJObject["id"];

            //time可以为空了 额
            if (TransationDetailJObject["time"] != null)
                Time = TransationDetailJObject["time"].ToString();
            else Time = "";
            Title = TransationDetailJObject["title"].ToString();
            Content = TransationDetailJObject["content"].ToString();

            ClassToLesson = TransationDetailJObject["date"].ToString();

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
