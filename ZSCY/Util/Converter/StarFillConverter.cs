using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace ZSCY.Util.Converter
{
    public class StarFillConverter : IValueConverter
    {
        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;


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
                int nowweek = Int32.Parse(appSetting.Values["nowWeek"].ToString());
                DateTime now = DateTime.Now;
                int dayofweek = (int)now.DayOfWeek == 0 ? 7 : (int)now.DayOfWeek;
                if (nowweek == week && dayofweek > Int32.Parse(w[1].Value))
                {
                    return "/Assets/StarFilled.png";
                }
                else if (nowweek > week)
                {
                    return "/Assets/StarFilled.png";
                }
                else
                {
                    return "/Assets/StarEmpty.png";
                }
            }
            return "/Assets/starun.png";

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
