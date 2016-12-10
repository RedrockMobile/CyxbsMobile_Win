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
        private static string resourceName = "ZSCY";
        private static string stuNum = "";
        private static string idNum = "";
        List<KeyValuePair<string, string>> curriculumParamList = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> transactionParamList = new List<KeyValuePair<string, string>>();
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                Debug.WriteLine("开始动态磁贴后台任务");
                //获取参数列表
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                stuNum = credentialList[0].UserName;
                idNum = credentialList[0].Password;
                curriculumParamList.Add(new KeyValuePair<string, string>("stuNum", stuNum));
                transactionParamList.Add(new KeyValuePair<string, string>("stuNum", stuNum));
                transactionParamList.Add(new KeyValuePair<string, string>("idNum", idNum));

                //异步后台任务开始
                BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
                #region 获取课表
                string curriculumTemp1 = await NetWork.getCurriculum(@"redapi2/api/kebiao", curriculumParamList);
                JObject jObject1 = (JObject)JsonConvert.DeserializeObject(curriculumTemp1);
                string curriculumTemp2 = jObject1["data"].ToString();
                JArray jArray1 = (JArray)JsonConvert.DeserializeObject(curriculumTemp2);
                int nowWeek = int.Parse(jObject1["nowWeek"].ToString());
                List<ClassList> tempList1 = JsonConvert.DeserializeObject<List<ClassList>>(jArray1.ToString());
                #endregion

                #region 获取事项
                string transactionTemp1 = await NetWork.getTransaction(@"http://hongyan.cqupt.edu.cn/cyxbsMobile/index.php/Home/Person/getTransaction", transactionParamList);
                JObject jObject2 = (JObject)JsonConvert.DeserializeObject(transactionTemp1);
                string transactionTemp2 = jObject2["data"].ToString();
                JArray jArray2 = (JArray)JsonConvert.DeserializeObject(transactionTemp2);
                List<Transaction> tempList2 = JsonConvert.DeserializeObject<List<Transaction>>(jArray2.ToString());
                #endregion
                //获取当地星期
                string weekDay = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
                Util.UpdateTile(tempList1, tempList2, nowWeek, weekDay);
                deferral.Complete();
                //异步后台任务结束
                Debug.WriteLine("结束动态磁贴后台任务");
            }
            catch (Exception)
            {
                Debug.WriteLine("动态磁贴后台任务出现异常");
            }
        }
    }
}
