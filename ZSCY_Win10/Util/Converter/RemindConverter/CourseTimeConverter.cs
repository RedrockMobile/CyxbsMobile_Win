using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter.RemindConverter
{
    public class CourseTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string returnString = "";
            if (value != null)
            {
                var temp = value.ToString().Split('、');
                if (temp.Length > 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        returnString += $"{temp[i]}、";
                    }
                    returnString = returnString.Remove(returnString.Length - 1)+"...";
                }
                else
                {
                    returnString = value.ToString();
                }
            }
            return returnString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
