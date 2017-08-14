using System;
using Windows.UI.Xaml.Controls;

namespace ZSCY_Win10
{
    public class NavMenuItem
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public string Image { get; set; }

        public char SymbolAsChar
        {
            get
            {
                return (char)this.Symbol;
            }
        }

        public Type DestPage { get; set; }
        public object Arguments { get; set; }
    }
}