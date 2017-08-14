using System;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter.StartPageConverts
{
    internal class BackgroundImageConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}