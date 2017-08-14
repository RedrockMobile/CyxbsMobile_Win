﻿using System;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    internal class JsonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string s = value.ToString();
            s = s.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}