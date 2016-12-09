using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Data.Community
{
    interface IFeeds
    {
        void GetAttributes(JObject feedsJObject);
    }
}
