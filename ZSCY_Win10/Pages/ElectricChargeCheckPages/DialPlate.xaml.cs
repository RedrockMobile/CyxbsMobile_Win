using System;
using Windows.UI;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.Brushes;
using System.ComponentModel;
using System.Diagnostics;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZSCY_Win10.Pages.ElectricChargeCheckPages
{
    public sealed partial class DialPlate : UserControl,INotifyPropertyChanged
    {
        public DialPlate()
        {
            this.InitializeComponent();
        }

        public double Percent   //表盘的百分比，定义域0.0-100.0，double型，默认值0
        {
            get { return (double)GetValue(_percent); }
            set { SetValue(_percent, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Percent")); }
        }

        private static readonly DependencyProperty _percent = DependencyProperty.Register("_percent", typeof(double), typeof(DialPlate), new PropertyMetadata((double)0));

        public string BalanceProperty   //电费余额，注意长度不建议长于5位,表盘会溢出,string型,默认值--
        {
            get { return (string)GetValue(_balanceProperty); }
            set { SetValue(_balanceProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BalanceProperty")); }
        }

        private static readonly DependencyProperty _balanceProperty = DependencyProperty.Register("Balance", typeof(string), typeof(DialPlate), new PropertyMetadata("--"));

        public string DumpEnergyProperty   //电量剩余度数，注意长度不建议长于8位,表盘会溢出,string型,默认值--
        {
            get {   return (string)GetValue(_dumpEnergyProperty); }
            set {   SetValue(_dumpEnergyProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DumpEnergyProperty")); }
        }
        private static readonly DependencyProperty _dumpEnergyProperty = DependencyProperty.Register("Balance", typeof(string), typeof(DialPlate), new PropertyMetadata("--"));

        public event PropertyChangedEventHandler PropertyChanged;

        private void BGCycle_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            CanvasGradientStop[] gradientStop = new CanvasGradientStop[2];
            gradientStop[0] = new CanvasGradientStop();
            gradientStop[0].Color = Color.FromArgb(180, 18, 208, 255);
            gradientStop[0].Position = 0f;

            gradientStop[1] = new CanvasGradientStop();
            gradientStop[1].Color = Color.FromArgb(0, 255, 255, 255);
            gradientStop[1].Position = 1f;

            CanvasRadialGradientBrush brush = new CanvasRadialGradientBrush(sender, gradientStop);
            brush.RadiusX = 200;
            brush.RadiusY = 200;
            brush.Center = new Vector2(200, 200);

            args.DrawingSession.FillCircle(200, 200, 200, brush);

        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.BGCycle.RemoveFromVisualTree();
            this.BGCycle = null;
        }
    }
    public class PercentValueToPoint : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return GetPoint((double)value, 202);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
        public Point GetPoint(double percent, double radius = 202)
        {
            double percentRad = percent / 100 * Math.PI;
            Point temp = new Point((1 - Math.Cos(percentRad)) * radius + 200 - radius, -radius * Math.Sin(percentRad) + 200);                //Math.Cos和Math.sin传入的值是弧度制的，传角度不行，别问我怎么知道的，我不会说我调试了半个小时才找到错在哪
            return temp;
        }
    }
    public class PercentValueToPoint2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return GetPoint((double)value, 205);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
        public Point GetPoint(double percent, double radius = 205)
        {
            double percentRad = percent / 100 * Math.PI;
            Point temp = new Point((1 - Math.Cos(percentRad)) * radius + 200 - radius, -radius * Math.Sin(percentRad) + 200);                //Math.Cos和Math.sin传入的值是弧度制的，传角度不行，别问我怎么知道的，我不会说我调试了半个小时才找到错在哪
            return temp;
        }
    }
}
