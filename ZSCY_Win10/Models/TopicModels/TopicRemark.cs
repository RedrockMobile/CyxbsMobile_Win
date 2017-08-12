using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Models.TopicModels
{
    class TopicRemark
    {
        public string nickname { get; set; }
        public string photo_src { get; set; }
        public string created_time { get; set; }
        public string content { get; set; }

        public void GetAttribute(JObject TopicContentDetailJObject)
        {
            nickname = TopicContentDetailJObject["nickname"].ToString();
            photo_src = TopicContentDetailJObject["photo_src"].ToString();
            created_time = TopicContentDetailJObject["created_time"].ToString();
            content=TopicContentDetailJObject["content"].ToString();
        }
    }
}
