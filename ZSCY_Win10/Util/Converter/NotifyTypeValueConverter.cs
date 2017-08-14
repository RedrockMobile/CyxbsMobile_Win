using System;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    public class NotifyTypeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string r = value as string;
            try
            {
                if (r == "praise")
                {
                    return "赞了我";
                }
                else if (r == "remark")
                {
                    return "评论了我";
                }
                else if (int.Parse(r) >= 0)
                {
                    return r;
                }
                else
                    return "";
            }
            catch (Exception)
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