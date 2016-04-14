using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Data.Community;

namespace ZSCY_Win10.Service
{
    public class CommunityFeedsService
    {

        const string hotFeeds = @"cyxbsMobile/index.php/Home/Article/searchHotArticle";
        /// <summary>
        /// 获取动态列表
        /// </summary>
        /// <param name="type">动态参数，cyxw=>1,jwzx=>2,xsjz=>3,xwgg=>4,bbdd=>5</param>
        /// <returns>返回参数对应的列表数据</returns>
        public static List<Feeds> GetDatas(int type,int page,int size)
        {
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();

            return null;
        }
    }
}
