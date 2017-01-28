
using System.Runtime.Serialization;

namespace ZSCY_Win10.Models.RemindModels
{
    [DataContract]
    public class DateModel
    {
        [DataMember(Name ="week")]
        public string Week { get; set; }
        [DataMember(Name ="class")]
        public string Class { get; set; }
        [DataMember(Name ="day")]
        public string Day { get; set; }
        public string ClassTime()
        {
            string weekString = "";
            string classString = "";
            switch (int.Parse(Day))
            {
                case 0:
                    weekString = "周一";
                    break;
                case 1:
                    weekString = "周二";
                    break;
                case 2:
                    weekString = "周三";
                    break;
                case 3:
                    weekString = "周四";
                    break;
                case 4:
                    weekString = "周五";
                    break;
                case 5:
                    weekString = "周六";
                    break;
                case 6:
                    weekString = "周日";
                    break;
            }
            switch (int.Parse(Class))
            {
                case 0:
                    classString = "12节";
                    break;
                case 1:
                    classString = "34节";
                    break;
                case 2:
                    classString = "56节";
                    break;
                case 3:
                    classString = "78节";
                    break;
                case 4:
                    classString = "910节";
                    break;
                case 5:
                    classString = "1112节";
                    break;
            }
            string temp = string.Concat(weekString, classString);
            return temp;
        }

    }
}