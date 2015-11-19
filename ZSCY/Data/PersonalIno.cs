using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;
using ZSCY.Util;

namespace ZSCY.Data
{
    public class PersonalIno
    {
        private string stunum;
        private string idnum;
        private string name;
        private string gender;
        private string clsssnum;
        private string major;
        private string college;
        private ApplicationDataContainer appSetting;

        public PersonalIno()
        {

            appSetting = ApplicationData.Current.LocalSettings; //本地存储
            if (!appSetting.Values.ContainsKey(("college")) )
            {
                getInfo();
            }
            else
            {
                if(appSetting.Values["major"].ToString() == "")
                    getInfo();
           
            }
            this.Stunum = appSetting.Values["stuNum"].ToString();
            this.Name = appSetting.Values["name"].ToString();
            this.Classnum = appSetting.Values["classNum"].ToString();
            this.Major = appSetting.Values["major"].ToString();
            this.College = appSetting.Values["college"].ToString();
            this.Gender = appSetting.Values["gender"].ToString();

        }

        private async void getInfo()
        {
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
            string login = await NetWork.getHttpWebRequest("api/verify", paramList);
            Debug.WriteLine("login->" + login);
            if (login != "")
            {
                try
                {
                    JObject obj = JObject.Parse(login);
                    if (Int32.Parse(obj["status"].ToString()) == 200)
                    {
                        JObject dataobj = JObject.Parse(obj["data"].ToString());
                        appSetting.Values["name"] = dataobj["name"].ToString();
                        appSetting.Values["classNum"] = dataobj["classNum"].ToString();
                        appSetting.Values["gender"] = dataobj["gender"].ToString();
                        appSetting.Values["major"] = dataobj["major"].ToString();
                        appSetting.Values["college"] = dataobj["college"].ToString();

                    }
                    else
                    {
                        appSetting.Values["gender"] = "";
                        appSetting.Values["major"] = "";
                        appSetting.Values["college"] = "";
                    }
                 
                }
                catch (Exception)
                {
                    Debug.WriteLine("登陆->返回值解析异常");
                }
            }
            else
                Utils.Message("网络异常");
        }


        public string Stunum
        {
            get
            {
                return stunum;
            }

            set
            {
                stunum = value;
            }
        }

        public string Idnum
        {
            get
            {
                return idnum;
            }

            set
            {
                idnum = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Gender
        {
            get
            {
                return gender;
            }

            set
            {
                gender = value;
            }
        }

        public string Classnum
        {
            get
            {
                return clsssnum;
            }

            set
            {
                clsssnum = value;
            }
        }

        public string Major
        {
            get
            {
                return major;
            }

            set
            {
                major = value;
            }
        }

        public string College
        {
            get
            {
                return college;
            }

            set
            {
                college = value;
            }
        }
    }
}
