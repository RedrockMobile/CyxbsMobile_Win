using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Util;

namespace ZSCY_Win10.Models.RemindModels
{
    public class RemindListModel:DataBaseModel
    {
       
        private bool _IsRewrite;
        private RemindBackupModel _Remind;
        private string _DayAndClass;
        public bool IsRewrite
        {
            get
            {
                return _IsRewrite;
            }

            set
            {
                _IsRewrite = value;
                OnProperChanged(nameof(IsRewrite));  
            }
        }
        public void JsonToModel()
        {
            Remind = new RemindBackupModel();
            Remind = JsonConvert.DeserializeObject<RemindBackupModel>(json);
            Remind.Id = Id;
            var user = GetCredential.getCredential("ZSCY");
            Remind.IdNum = user.Password;
            Remind.StuNum = user.UserName;
        }
        public string ClassTime()
        {
            string temp = "";
            foreach (var item in Remind.DateItems)
            {
                temp += $"{item.ClassTime()}、";
            }
            temp = temp.Remove(temp.Length - 1);
            DayAndClass = temp;
            return temp;
        }

        public RemindBackupModel Remind
        {
            get
            {
                return _Remind;
            }

            set
            {
                _Remind = value;
            }
        }

        public string DayAndClass
        {
            get
            {
                return _DayAndClass;
            }

            set
            {
                _DayAndClass = value;
            }
        }
    }
}
