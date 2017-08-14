using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace ZSCY_Win10.Util.Converter
{
    public class TextBlockInlineConverter : IValueConverter
    {
        private static SolidColorBrush TagColor = new SolidColorBrush(Color.FromArgb(255, 80, 125, 175));

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }
            var span = new Span();
            string s = (string)value;
            int index = 0;
            while ((index = s.IndexOf("#")) >= 0)
            {
                if (s.Substring(index + 1).IndexOf("#") >= 0)
                {
                    if (index != 0)
                    {
                        Run r = new Run();
                        r.Text = s.Substring(0, index);
                        span.Inlines.Add(r);
                        s = s.Substring(index);
                        index = 0;
                    }
                    int index2 = s.Substring(index + 1).IndexOf("#");
                    if (index2 > 0)
                    {
                        string highlight = s.Substring(index, index2 + index + 2);
                        Run high = new Run();
                        high.Text = highlight;
                        high.Foreground = TagColor;
                        span.Inlines.Add(high);
                        s = s.Substring(index2 + index + 2);
                        index = 0;
                    }
                }
                else
                {
                    break;
                }
            }
            Run left = new Run();
            left.Text = s;
            span.Inlines.Add(left);
            return span;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}