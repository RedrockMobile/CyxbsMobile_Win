using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter.RemindConverter
{
    class RemindListClassTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string[] temp = value.ToString().Split('、');
            string s = "";
            if(temp.Length>2)
            {
                for(int i=0;i<2;i++)
                {
                    s += temp[i] + "、";
                }
                s = s.Remove(s.Length - 1)+"...";
            }
            else
            {
                s = value.ToString();
            }
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
