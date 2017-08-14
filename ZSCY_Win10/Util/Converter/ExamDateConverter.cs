using System;
using System.Text.RegularExpressions;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    internal class ExamDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
            string data = value.ToString();
            string[] sp = new string[] { "\r" };
            string[] date = data.Split(sp, StringSplitOptions.None);
            if (!date[0].Contains("周0"))
            {
                Regex r = new Regex(@"\d+");
                var w = r.Matches(date[0]);
                int week = Int32.Parse(w[0].Value);
                if (week > 99)
                {
                    return week / 10000 + "\r" + week % 10000 / 100 + "/" + week % 10;
                }
                int nowweek = Int32.Parse(appSetting.Values["nowWeek"].ToString());
                DateTime now = DateTime.Now;
                int dayofweek = (int)now.DayOfWeek == 0 ? 7 : (int)now.DayOfWeek;
                DateTime targetdate = now.AddDays((week - nowweek) * 7 + Int32.Parse(w[1].Value) - dayofweek);
                string weekname = date[0].Substring(date[0].Length - 2, 2);
                string replaceweekname = "";
                switch (weekname)
                {
                    case "周1": replaceweekname = "周一"; break;
                    case "周2": replaceweekname = "周二"; break;
                    case "周3": replaceweekname = "周三"; break;
                    case "周4": replaceweekname = "周四"; break;
                    case "周5": replaceweekname = "周五"; break;
                    case "周6": replaceweekname = "周六"; break;
                    case "周7": replaceweekname = "周日"; break;
                }
                string result = date[0].Replace(weekname, replaceweekname);
                return result + "\r" + targetdate.Month + "/" + targetdate.Day;
            }
            return "待\r定";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}