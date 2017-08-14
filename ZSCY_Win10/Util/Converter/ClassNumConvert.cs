﻿using System;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    public class ClassNumConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value + "节";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}