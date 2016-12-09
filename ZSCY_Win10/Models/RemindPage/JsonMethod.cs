using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ZSCY_Win10.Models.RemindPage
{
   public static class JsonMethod
    {
        public static string ToJson(MyRemind myRemind)
        {
            string json;
            json = JsonConvert.SerializeObject(myRemind);
            return json;
        }
        public static MyRemind ToModel(string json)
        {
            MyRemind myRemind;
            myRemind = JsonConvert.DeserializeObject<MyRemind>(json);
            return myRemind;
        }
    }
}
