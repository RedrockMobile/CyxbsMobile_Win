using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235


namespace ZSCY_Win10.Resouces
{
    public sealed class LineChart : Control
    {
        public LineChart()
        {
            this.DefaultStyleKey = typeof(LineChart);

        }


        public double ChartHeight
        {
            get { return (double)GetValue(ChartHeightProperty); }
            set { SetValue(ChartHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChartHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChartHeightProperty =
            DependencyProperty.Register("ChartHeight", typeof(double), typeof(LineChart), new PropertyMetadata(0));

        public double ChartWidth
        {
            get { return (double)GetValue(ChartWidthProperty); }
            set { SetValue(ChartWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChartWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChartWidthProperty =
            DependencyProperty.Register("ChartWidth", typeof(double), typeof(LineChart), new PropertyMetadata(0));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LineChart), new PropertyMetadata("ItemTitle"));

        public string Label
        {
            get
            {
                //return ((string)GetValue(LabelProperty) + "人");
                return ((string)GetValue(LabelProperty));
            }
            set
            {
                SetValue(LabelProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(LineChart), new PropertyMetadata("ItemLabel"));

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(LineChart), new PropertyMetadata(0));

        public double ActualValue
        {
            get { return (double)GetValue(ActualValueProperty); }
            set { SetValue(ActualValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActualValueProperty =
            DependencyProperty.Register("ActualValue", typeof(double), typeof(LineChart), new PropertyMetadata(0));
    }
}
