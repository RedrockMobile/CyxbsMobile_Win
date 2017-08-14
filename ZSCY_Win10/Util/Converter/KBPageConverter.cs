using System;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    public class KBAllScrollViewerHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Utils.getPhoneHeight() - 48 - 50;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}