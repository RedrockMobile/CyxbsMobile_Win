using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace LiveTileBackgroundTask
{
    public sealed class LiveTileBackgroundTask : IBackgroundTask
    {
        private static string stuNum = "";
        private static string resourceName = "ZSCY";
        List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                Debug.WriteLine("开始动态磁贴后台任务");
                //获取stuNum
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                stuNum = credentialList[0].UserName;

                BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
                paramList.Add(new KeyValuePair<string, string>("stuNum", stuNum));
                string kbTemp1 = await NetWork.getHttpWebRequest("redapi2/api/kebiao", paramList);
                JObject jObject1 = (JObject)JsonConvert.DeserializeObject(kbTemp1);
                string kbTemp2 = jObject1["data"].ToString();
                int nowWeek = int.Parse(jObject1["nowWeek"].ToString());
                JArray jArray = (JArray)JsonConvert.DeserializeObject(kbTemp2);
                List<ClassList> tempList = JsonConvert.DeserializeObject<List<ClassList>>(jArray.ToString());
                string weekDay = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
                //更新动态磁贴
                Util.UpdateTile(tempList, nowWeek, weekDay);
                deferral.Complete();
                Debug.WriteLine("结束动态磁贴后台任务");
            }
            catch (Exception)
            {
                Debug.WriteLine("动态磁贴后台任务出现异常");
            }
        }
    }
}
