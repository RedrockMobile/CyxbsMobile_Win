using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

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
            this.Stunum= appSetting.Values["stuNum"].ToString();
            this.Name= appSetting.Values["name"].ToString();
            this.Classnum= appSetting.Values["classNum"].ToString();
            this.Major= appSetting.Values["major"].ToString();
            this.College= appSetting.Values["college"].ToString();
            this.Gender= appSetting.Values["gender"].ToString();
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
