using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Security.Credentials;


namespace SycnRemindBackgroundTask
{
    public sealed class RemindBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            bool isLoad = false;
            PasswordCredential credential = null;
            var vault = new PasswordVault();
            var credentialList = vault.FindAllByResource("ZSCY");
            if (credentialList.Count == 1)
            {
                credentialList[0].RetrievePassword();
                credential = credentialList[0];
            }
            try
            {
                var list = DatabaseMethod.ToModel();

                BackgroundTaskDeferral deferral1 = taskInstance.GetDeferral();
                if (list.Count > 0)
                    //上传上次未上传的
                    foreach (var item in list)
                    {

                        if (item.Id == null)
                        {
                            string content = "";
                            isLoad = true;
                            MyRemind tempRemind = new MyRemind();
                            tempRemind = JsonConvert.DeserializeObject<MyRemind>(item.json);
                            tempRemind.IdNum = credential.Password;
                            tempRemind.StuNum = credential.UserName;
                            content = await NetWork.httpRequest(@"http://hongyan.cqupt.edu.cn/cyxbsMobile/index.php/Home/Person/addTransaction", NetWork.addRemind(tempRemind));
                            Debug.WriteLine(content);

                            AddRemindReturn tempReturn = JsonConvert.DeserializeObject<AddRemindReturn>(content);
                            if (tempReturn.Status == 200)
                            {
                                string id = tempReturn.Id;
                                tempRemind.Id = id;
                                tempRemind.IdNum = null;
                                tempRemind.StuNum = null;
                                DatabaseMethod.EditDatabase(item.Num, id, JsonConvert.SerializeObject(tempRemind), item.Id_system);
                            }


                        }
                    }

                deferral1.Complete();
                BackgroundTaskDeferral deferral2 = taskInstance.GetDeferral();
                if (isLoad)
                {

                }
                else
                {
                    RemindHelp.SyncRemind();
                }

                deferral2.Complete();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
