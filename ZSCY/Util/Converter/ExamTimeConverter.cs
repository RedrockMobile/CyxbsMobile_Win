using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZSCY.Util.Converter
{
    public class ExamTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string data = value.ToString();
            string[] sp = new string[] { "\n" };
            string[] date = data.Split(sp, StringSplitOptions.None);

            return date[1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
