using System;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    internal class CommunityItemPhotoFlipViewHeighConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return double.Parse(value.ToString()) - 48;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}