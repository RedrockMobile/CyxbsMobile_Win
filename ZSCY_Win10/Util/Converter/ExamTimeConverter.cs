using System;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    internal class ExamTimeConverter : IValueConverter
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