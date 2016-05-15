using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MyMessageBackgroundTask
{
    public sealed class MessageBackgroundTask : IBackgroundTask
    {
        private ApplicationDataContainer appSetting = ApplicationData.Current.LocalSettings; //本地存储
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            if (bool.Parse(appSetting.Values["isUseingBackgroundTask"].ToString()))
            {
                Debug.WriteLine("开始后台任务");
                try
                {
                    string letterstatus = "";
                    BackgroundTaskDeferral deferral = taskInstance.GetDeferral();  //获取 BackgroundTaskDeferral 对象，表示后台任务延期

                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
                    paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
                    paramList.Add(new KeyValuePair<string, string>("page", "0"));
                    paramList.Add(new KeyValuePair<string, string>("size", "15"));
                    letterstatus = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Article/aboutme", paramList);

                    Debug.WriteLine("letterstatus" + letterstatus);

                    if (letterstatus != "")
                    {
                        string aboutme = "";
                        IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                        try
                        {
                            IStorageFile storageFileRE = await applicationFolder.GetFileAsync("aboutme.txt");
                            IRandomAccessStream accessStream = await storageFileRE.OpenReadAsync();
                            using (StreamReader streamReader = new StreamReader(accessStream.AsStreamForRead((int)accessStream.Size)))
                            {
                                aboutme = streamReader.ReadToEnd();
                            }
                            Debug.WriteLine("aboutme：" + aboutme);
                            if (aboutme != letterstatus)
                            {
                                JObject obj = JObject.Parse(letterstatus);
                                if (Int32.Parse(obj["status"].ToString()) == 200)
                                {
                                    JArray jArray = (JArray)JsonConvert.DeserializeObject(obj["data"].ToString());
                                    if (jArray[0]["type"].ToString() == "praise")
                                    {
                                        Utils.Toast(jArray[0]["nickname"].ToString() + "赞了你~~\n你可能还有有新的消息，快来看看吧");
                                    }
                                    else if (jArray[0]["type"].ToString() == "remark")
                                    {
                                        Utils.actionsToast(jArray[0]["nickname"].ToString() + " 评论了 " + "\""+ jArray[0]["article_content"].ToString() + "\"", "\"" + jArray[0]["content"].ToString() + "\"", jArray[0]["article_id"].ToString());
                                        //if (jArray[0]["content"].ToString().Length > 20)
                                        //{
                                        //    Utils.Toast(jArray[0]["nickname"].ToString() + "评论了你~\n" + "\"" + jArray[0]["content"].ToString().Substring(0, 20) + "\"" + "\n你可能还有有新的消息，快来看看吧");
                                        //}
                                        //else
                                        //{
                                        //    Utils.Toast(jArray[0]["nickname"].ToString() + "评论了你~\n" + "\"" + jArray[0]["content"].ToString() + "\"" + "\n你可能还有有新的消息，快来看看吧");
                                        //}
                                    }
                                    else
                                    {
                                        Utils.Toast("你可能有新的消息，快来看看吧");
                                    }
                                }

                                IStorageFile storageFileWR = await applicationFolder.CreateFileAsync("aboutme.txt", CreationCollisionOption.ReplaceExisting);
                                await FileIO.WriteTextAsync(storageFileWR, letterstatus);
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            IStorageFile storageFileWR = await applicationFolder.CreateFileAsync("aboutme.txt", CreationCollisionOption.ReplaceExisting);
                            await FileIO.WriteTextAsync(storageFileWR, letterstatus);
                        }


                    }
                    deferral.Complete(); //所有的异步调用完成之后，释放延期，表示后台任务的完成
                }
                catch (Exception) { Debug.WriteLine("后台任务网络异常"); }

            }
        }
    }
}
