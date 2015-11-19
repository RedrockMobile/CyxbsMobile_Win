using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Data;

namespace ZSCY.Util.Converter
{
    public class LongClassRoomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string classroom = value as string;
            string room = "";
            if (classroom.Length > 12)
            {
                string pattern = "^[a-z0-9]*|[a-z0-9]*$";
                Regex reg = new Regex("^[a-z0-9]*|[a-z0-9]*$", RegexOptions.IgnoreCase);
                if (Regex.IsMatch(classroom, pattern))
                {

                    MatchCollection mc = reg.Matches(classroom);
                    foreach (Match item in mc)
                    {
                        if (item.Value != "")
                        {
                            room += item.Value;
                            string temp = item.Value;
                            classroom = classroom.Replace(temp, "");
                            room += " ";
                        }
                    }
                }
            }
            return room + classroom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
