﻿using System;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    public class ExpandingConverter3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int n = value.ToString().Length;
            char[] a = value.ToString().ToCharArray();

            if (n > 50)
            {
                if (App.isReduced[3])
                {
                    string s = "";
                    for (int i = 0; i < 50; i++)
                    {
                        s += a[i];
                    }
                    return s + "...";
                }
                else
                {
                    return value.ToString();
                }
            }
            else
            {
                return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}