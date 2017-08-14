using Newtonsoft.Json.Linq;

namespace ZSCY_Win10.Data.Community
{
    internal interface IFeeds
    {
        void GetAttributes(JObject feedsJObject);
    }
}