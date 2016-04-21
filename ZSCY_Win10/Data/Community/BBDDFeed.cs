using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ZSCY_Win10.Data.Community
{
    public class BBDDFeed:IFeeds
    {
        public string title { get; set; }
        public string id { get; set; }
        public string article_photo_src { get; set; }
        public string article_thumbnail_src { get; set; }
        public string type_id { get; set; }
        public string content { get; set; }
        public string updated_time { get; set; }
        public string created_time { get; set; }
        public string like_num { get; set; }
        public string remark_num { get; set; }
        public string nickname { get; set; }
        public string photo_src { get; set; }
        public string photo_thumbnail_src { get; set; }
        public bool is_my_like { get; set; }

        public void GetAttributes(JObject feedsJObject)
        {
            throw new NotImplementedException();
        }
    }
}
