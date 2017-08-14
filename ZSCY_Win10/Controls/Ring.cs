using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace ZSCY_Win10.Controls
{
    public sealed class Ring : Control
    {
        private static DoubleCollection fuck = new DoubleCollection();
        public Ring()
        {
            this.DefaultStyleKey = typeof(Ring);
        }
        public static readonly DependencyProperty HoleWidthProperty = DependencyProperty.Register("HoleWidth", typeof(double),typeof(Ring), new PropertyMetadata(0, OnHoleWidthChanged));
        public double HoleWidth
        {
            get { return (double)GetValue(HoleWidthProperty); }
            set { SetValue(HoleWidthProperty, value); }
        }
        public static readonly DependencyProperty RatioProperty = DependencyProperty.Register("Ratio", typeof(double),typeof(Ring), new PropertyMetadata(.0, OnRatioChanged));
        public double Ratio
        {
            get { return (double)GetValue(RatioProperty); }
            set { SetValue(RatioProperty, value); }
        }
        public static readonly DependencyProperty DashProperty = DependencyProperty.Register("Dash", typeof(DoubleCollection),typeof(Ring), new PropertyMetadata(fuck));
        public DoubleCollection Dash
        {
            get { return (DoubleCollection)GetValue(DashProperty); }
            set { SetValue(DashProperty, value); }
        }
        public static readonly DependencyProperty RingForegroundColorProperty = DependencyProperty.Register("RingForegroundColor", typeof(Brush),typeof(Ring), new PropertyMetadata(null));
        public Brush RingForegroundColor
        {
            get { return (Brush)GetValue(RingForegroundColorProperty); }
            set { SetValue(RingForegroundColorProperty, value); }
        }
        public static readonly DependencyProperty RingBackgroundColorProperty = DependencyProperty.Register("RingBackgroundColor", typeof(Brush),typeof(Ring), new PropertyMetadata(null));
        public Brush RingBackgroundColor
        {
            get { return (Brush)GetValue(RingBackgroundColorProperty); }
            set { SetValue(RingBackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty BreadthProperty = DependencyProperty.Register("Breadth", typeof(double),typeof(Ring), new PropertyMetadata(20.0));
        public double Breadth
        {
            get { return (double)GetValue(BreadthProperty); }
            set { SetValue(BreadthProperty, value); }
        }
        private static void OnRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ring ring = d as Ring;
            //ring.RatioText = (ring.Ratio * 100).ToString("0.00") + "%";
            double cirLength = (ring.Width - ring.Breadth) * 3.14 * ring.Ratio / ring.Breadth;
            DoubleCollection dc = new DoubleCollection();
            dc.Add(cirLength);
            dc.Add(double.MaxValue);
            ring.Dash = dc;
        }

        private static void OnHoleWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ring ring = d as Ring;
            ring.Width = ring.Height = ring.HoleWidth;
        }
    }
}
