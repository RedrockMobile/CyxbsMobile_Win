using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Models.TopicModels;

namespace ZSCY_Win10.Models.Topic
{
    public class TopicContent
    {
        public string keyword { get; set; }
        public string content { get; set; }
        public string photo_src { get; set; }
        public int join_num { get; set; }
        public int article_num { get; set; }
        public int topic_id { get; set; }
        public bool is_my_join { get; set; }
        public List<Articles> articles { get; set; } = new List<Articles>();
        public string articleString { get; set; }

        public void GetAttribute(JObject TopicContentDetailJObject)
        {
            topic_id = (int)TopicContentDetailJObject["topic_id"];
            keyword = TopicContentDetailJObject["keyword"].ToString();
            content = TopicContentDetailJObject["content"].ToString();
            photo_src = TopicContentDetailJObject["photo_src"].ToString();
            join_num = (int)TopicContentDetailJObject["join_num"];
            is_my_join = (bool)TopicContentDetailJObject["is_my_join"];
            articleString = TopicContentDetailJObject["articles"].ToString();
            articles = JsonConvert.DeserializeObject<List<Articles>>(articleString);
            if (articles.Count != 0)
                foreach (var item in articles)
                {
                    if (item.article_photo_src != "" && item.article_thumbnail_src != "")
                    {
                        string[] temp = item.article_photo_src.Split(',');
                        string[] _temp = item.article_thumbnail_src.Split(',');
                        for (int i = 0; i < temp.Length; i++)
                        {
                            item.articlesPic.Add(new pic { article_photo_src = $"http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/photo/{temp[i]}", article_thumbnail_src = $"http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/photo/thumbnail/{_temp[i]}" });
                        }
                    }
                }
        }
    }
}
