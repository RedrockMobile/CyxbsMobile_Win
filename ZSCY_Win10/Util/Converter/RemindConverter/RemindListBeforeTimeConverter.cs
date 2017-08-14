using System;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter.RemindConverter
{
    internal class RemindListBeforeTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string s = "";
            if (value == null)
                s = "不提醒";
            else
            {
                s = $"提前{value.ToString()}分钟";
            }
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}