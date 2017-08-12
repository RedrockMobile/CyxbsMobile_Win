using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Models.TopicModels
{
    public class imgSource
    {
        public string img_small_src { get; set; }
        public string img_src { get; set; }

    }
    public class Content {
        public string content { get; set; }
    }
    public class Topic
    {
        public int topic_id { get; set; }
        public string keyword { get; set; }
        public int like_num { get; set; }
        public int join_num { get; set; }
        public string join { get; set; }
        //public string remark_num { get; set; }
        public int article_num { get; set; }
        public string user_id { get; set; }
        public string nickname { get; set; }
        public imgSource img { get; set; }
        public string imgdata { get; set; }
        public bool is_my_join { get; set; }
        public string contentdata { get; set; }
        public Content content { get; set; }
        public string color { get; set; }
        public void GetAttribute(JObject TopicDetailJObject)
        {
            topic_id = (int)TopicDetailJObject["topic_id"];
            keyword = TopicDetailJObject["keyword"].ToString();
            contentdata = TopicDetailJObject["content"].ToString();
            content = JsonConvert.DeserializeObject<Content>(contentdata);
            contentdata = content.content;
            imgdata = TopicDetailJObject["img"].ToString() ;
            img= JsonConvert.DeserializeObject<imgSource>(imgdata);
            imgdata = img.img_small_src;
            like_num =(int)TopicDetailJObject["like_num"];
            join_num = (int)TopicDetailJObject["join_num"];
            nickname = TopicDetailJObject["nickname"].ToString();
            user_id = TopicDetailJObject["user_id"].ToString();
            is_my_join = (bool)TopicDetailJObject["is_my_join"];
            color = "White";
            join = $"{join_num}人参与";
        }
        
    }
}
