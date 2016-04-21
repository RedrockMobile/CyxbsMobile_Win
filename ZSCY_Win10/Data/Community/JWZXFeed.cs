using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ZSCY_Win10.Data.Community
{
    class JWZXFeed:IFeeds
    {
        public string id { get; set; }
        public string articletype_id { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public string content { get; set; }
        public string like_num { get; set; }
        public string unit { get; set; }
        public string remark_num { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string articleid { get; set; }
        public string read { get; set; }
        public string head { get; set; }
        public bool is_my_like { get; set; }

        public void GetAttributes(JObject feedsJObject)
        {
            throw new NotImplementedException();
        }
    }
}
