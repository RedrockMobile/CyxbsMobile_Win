using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter.RemindConverter
{
    internal class IsRewriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int i = int.Parse(parameter.ToString());
            bool exclusive;
            if (i == 1)
                exclusive = true;
            else
                exclusive = false;
            if ((bool)value ^ exclusive)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}