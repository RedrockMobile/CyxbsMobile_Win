using Newtonsoft.Json.Linq;
using Windows.Storage;

namespace ZSCY.Data
{
    public class PersonalIno
    {
        private string stunum;
        private string idnum;
        private string name;
        private string gender;
        private string college;
        private string depart;
        private string grade;
        private ApplicationDataContainer appSetting;

        public PersonalIno()
        {
            appSetting = ApplicationData.Current.LocalSettings; //本地存储
            this.Stunum = "";
            this.Name = "";
            this.College = "";
            this.Gender = "";
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

        public void GetAttribute(JObject PeopleDetailJObject)
        {
            stunum = PeopleDetailJObject["stunum"].ToString();
            name = PeopleDetailJObject["name"].ToString();
            gender = PeopleDetailJObject["gender"].ToString();
            depart = PeopleDetailJObject["depart"].ToString();
            grade = PeopleDetailJObject["grade"].ToString();
        }
    }
}