using System;
using Windows.UI.Xaml.Data;
using ZSCY_Win10.Data.Community;

namespace ZSCY_Win10.Util.Converter
{
    public class GetOneorNonePicValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Img[] src = value as Img[];
            if (src == null)
            {
                return "";
            }
            else if (src.Length == 1)
            {
                return src[0].ImgSrc;
            }
            else if (src.Length > 1)
            {
                return src[0].ImgSmallSrc;
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}