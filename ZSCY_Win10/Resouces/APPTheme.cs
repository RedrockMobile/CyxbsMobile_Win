using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ZSCY_Win10.Resource
{
    public class APPTheme : DependencyObject
    {
        //APP主题颜色
        public SolidColorBrush APP_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(APP_Color_Brush_DP);
            }
            set
            {
                SetValue(APP_Color_Brush_DP, value);
            }
        }

        //5b5b5b
        public SolidColorBrush Gary_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Gary_Color_Brush_DP);
            }
            set
            {
                SetValue(Gary_Color_Brush_DP, value);
            }
        }

        //d6d6d6
        public SolidColorBrush Header_Black_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Header_Black_Color_Brush_DP);
            }
            set
            {
                SetValue(Header_Black_Color_Brush_DP, value);
            }
        }

        //#c9cacb
        public SolidColorBrush Second_White_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Second_White_Color_Brush_DP);
            }
            set
            {
                SetValue(Second_White_Color_Brush_DP, value);
            }
        }

        //#515151
        public SolidColorBrush Foreground_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Foreground_Color_Brush_DP);
            }
            set
            {
                SetValue(Foreground_Color_Brush_DP, value);
            }
        }

        //偏灰的底色
        public SolidColorBrush Backgorund_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Backgorund_Color_Brush_DP);
            }
            set
            {
                SetValue(Backgorund_Color_Brush_DP, value);
            }
        }

        //#333333
        public SolidColorBrush Content_Header_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Content_Header_Color_Brush_DP);
            }
            set
            {
                SetValue(Content_Header_Color_Brush_DP, value);
            }
        }

        //#555555
        public SolidColorBrush Light_Gary_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Light_Gary_Color_Brush_DP);
            }
            set
            {
                SetValue(Light_Gary_Color_Brush_DP, value);
            }
        }

        //白色
        public SolidColorBrush White_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(White_Color_Brush_DP);
            }
            set
            {
                SetValue(White_Color_Brush_DP, value);
            }
        }

        //#adadae
        public SolidColorBrush ADADAE_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(ADADAE_Color_Brush_DP);
            }
            set
            {
                SetValue(ADADAE_Color_Brush_DP, value);
            }
        }

        //Header字体尺寸
        public int Header_Size
        {
            get
            {
                return (int)GetValue(Header_Size_DP);
            }
            set
            {
                SetValue(Header_Size_DP, value);
            }
        }


        //内容文字尺寸
        public int Content_Size
        {
            get
            {
                return (int)GetValue(Content_Size_DP);
            }
            set
            {
                SetValue(Content_Size_DP, value);
            }
        }

        //二级内容文字尺寸
        public int Second_Content_Size
        {
            get
            {
                return (int)GetValue(Second_Content_Size_DP);
            }
            set
            {
                SetValue(Second_Content_Size_DP, value);
            }

        }
   
        private SolidColorBrush line_Color;
        /// <summary>
        /// 分割线颜色
        /// </summary>
        public SolidColorBrush Line_Color
        {
            get
            {
                return (SolidColorBrush)GetValue(Line_Color_DP);
            }

            set
            {
                SetValue(Line_Color_DP, value);
            }
        }

        private SolidColorBrush Font_Bule;
        /// <summary>
        /// 强调字体
        /// </summary>
        public SolidColorBrush Font_Bule1
        {
            get
            {
                return (SolidColorBrush)GetValue(Font_Blue_Dp);
            }

            set
            {
                SetValue(Font_Blue_Dp, value);
            }
        }


        public static readonly DependencyProperty Font_Blue_Dp = DependencyProperty.Register("Font_Blue", typeof(SolidColorBrush),typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 140, 196, 246))));
        public static readonly DependencyProperty APP_Color_Brush_DP = DependencyProperty.Register("APP_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 6, 140, 253)))); //蓝色色调

        public static readonly DependencyProperty Gary_Color_Brush_DP = DependencyProperty.Register("Gary_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 136, 136, 136)))); //5b5b5b

        public static readonly DependencyProperty Header_Black_Color_Brush_DP = DependencyProperty.Register("Header_Black_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 214, 214, 214)))); //d6d6d6

        public static readonly DependencyProperty Second_White_Color_Brush_DP = DependencyProperty.Register("Second_White_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 201, 202, 203)))); //#c9cacb

        public static readonly DependencyProperty Foreground_Color_Brush_DP = DependencyProperty.Register("Foreground_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)))); //白色

        public static readonly DependencyProperty Backgorund_Color_Brush_DP = DependencyProperty.Register("Backgorund_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 247, 247, 247)))); //偏灰的底色

        public static readonly DependencyProperty Content_Header_Color_Brush_DP = DependencyProperty.Register("Content_Header_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 51, 51, 51)))); //#333333

        public static readonly DependencyProperty Light_Gary_Color_Brush_DP = DependencyProperty.Register("Light_Gary_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 85, 85, 85)))); //#555555

        public static readonly DependencyProperty White_Color_Brush_DP = DependencyProperty.Register("White_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 10, 10, 10))));//透明

        public static readonly DependencyProperty ADADAE_Color_Brush_DP = DependencyProperty.Register("ADADAE_Color_Brush_DP", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 173, 173, 173)))); //#adadae

        public static readonly DependencyProperty Header_Size_DP = DependencyProperty.Register("Header_Size", typeof(int), typeof(APPTheme), new PropertyMetadata(18));

        public static readonly DependencyProperty Content_Size_DP = DependencyProperty.Register("Content_Size", typeof(int), typeof(APPTheme), new PropertyMetadata(15));

        public static readonly DependencyProperty Second_Content_Size_DP = DependencyProperty.Register("Second_Content_Size", typeof(int), typeof(APPTheme), new PropertyMetadata(15));
        public static readonly DependencyProperty Line_Color_DP = DependencyProperty.Register("Line_Color", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 239, 240, 242))));
    }
}
