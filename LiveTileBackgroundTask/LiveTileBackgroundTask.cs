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
        private static string idNum = "";
        private static string resourceName = "ZSCY";
        List<KeyValuePair<string, string>> courseScheduleParamList = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> transactionParamList = new List<KeyValuePair<string, string>>();
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                Debug.WriteLine("开始动态磁贴后台任务");
                //获取stuNum,idNum
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                stuNum = credentialList[0].UserName;
                idNum = credentialList[0].Password;

                BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
                courseScheduleParamList.Add(new KeyValuePair<string, string>("stuNum", stuNum));
                transactionParamList.Add(new KeyValuePair<string, string>("stuNum", stuNum));
                transactionParamList.Add(new KeyValuePair<string, string>("idNum", idNum));

                string courseScheduleTemp1 = await NetWork.GetCourseSchedule(@"http://hongyan.cqupt.edu.cn/redapi2/api/kebiao", courseScheduleParamList);
                JObject jObject1 = (JObject)JsonConvert.DeserializeObject(courseScheduleTemp1);
                string courseScheduleTemp2 = jObject1["data"].ToString();
                int nowWeek = int.Parse(jObject1["nowWeek"].ToString());
                JArray jArray1 = (JArray)JsonConvert.DeserializeObject(courseScheduleTemp2);
                List<ClassList> tempList1 = JsonConvert.DeserializeObject<List<ClassList>>(jArray1.ToString());
                //获取本机时间
                string weekDay = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);

                string transactionTemp1 = await NetWork.GetTransaction(@"http://hongyan.cqupt.edu.cn/cyxbsMobile/index.php/Home/Person/getTransaction", transactionParamList);
                JObject jObject2 = (JObject)JsonConvert.DeserializeObject(transactionTemp1);
                string transactionTemp2 = jObject2["data"].ToString();
                JArray jArray2 = (JArray)JsonConvert.DeserializeObject(transactionTemp2);
                List<TransactionModel> tempList2 = JsonConvert.DeserializeObject<List<TransactionModel>>(jArray2.ToString());
                //更新动态磁贴
                Util.UpdateTile(tempList1, tempList2, nowWeek, weekDay);
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
