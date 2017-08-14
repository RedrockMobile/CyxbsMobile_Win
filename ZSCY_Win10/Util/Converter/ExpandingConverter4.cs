using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    public class ExpandingConverter4 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value.ToString() + "张");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
