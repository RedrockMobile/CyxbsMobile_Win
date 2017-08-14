using Newtonsoft.Json.Linq;

namespace ZSCY_Win10.Data.Community
{
    public class HotFeedsContentBase : IFeeds
    {
        public JWZXFeeds contentbase { get; set; }
        public string content { get; set; }

        public void GetAttributes(JObject feedsJObject)
        {
            if (feedsJObject["id"] != null)
            {
                JWZXFeeds j = new JWZXFeeds();
                j.GetAttributes(feedsJObject);
                contentbase = j;
            }
            else
                content = feedsJObject["content"].ToString();

            //try
            //{
            //    JWZXFeeds j = new JWZXFeeds();
            //    j.GetAttributes(feedsJObject);
            //    contentbase = j;
            //}
            //catch (Exception)
            //{
            //    content = feedsJObject["content"].ToString();
            //}
        }
    }
}