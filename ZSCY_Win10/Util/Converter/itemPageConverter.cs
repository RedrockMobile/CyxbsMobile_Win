using System;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    internal class itemScrollViewerHeighConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return double.Parse(value.ToString()) - 95;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class EmptyitemScrollViewerHeighConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return double.Parse(value.ToString()) - 95 - 80;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class EmptyitemWidthScrollViewerHeighConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return double.Parse(value.ToString()) / (double.Parse(value.ToString()) / 60);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}