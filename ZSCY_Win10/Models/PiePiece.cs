using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ZSCY.Models
{
    class PiePiece:Path
    {
        #region
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("RadiusProperty", typeof(double), typeof(PiePiece), new PropertyMetadata(0.0));
        public double Radius        //饼图半径
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }



        public static readonly DependencyProperty PushOutProperty = DependencyProperty.Register("PushOutProperty", typeof(double), typeof(PiePiece), new PropertyMetadata(0.0));
        public double PushOut       //距离饼图中心的距离
        {
            get { return (double)GetValue(PushOutProperty); }
            set { SetValue(PushOutProperty, value); }
        }



        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadiusProperty", typeof(double), typeof(PiePiece), new PropertyMetadata(0.0));
        public double InnerRadius       //内圆半径
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }



        public static readonly DependencyProperty WedgeAngleProperty = DependencyProperty.Register("WedgeAngleProperty", typeof(double), typeof(PiePiece), new PropertyMetadata(0.0));
        public double WedgeAngle        //饼图片形的角度
        {
            get { return (double)GetValue(WedgeAngleProperty); }
            set
            {
                SetValue(WedgeAngleProperty, value);
                this.Percentage = (value / 360.0);
            }
        }



        public static readonly DependencyProperty RotationAngleProperty = DependencyProperty.Register("RotationAngleProperty", typeof(double), typeof(PiePiece), new PropertyMetadata(0.0));
        public double RotationAngle     //旋转的角度
        {
            get { return (double)GetValue(RotationAngleProperty); }
            set { SetValue(RotationAngleProperty, value); }
        }



        public static readonly DependencyProperty CentreXPropety = DependencyProperty.Register("CentreXPropety", typeof(double), typeof(PiePiece), new PropertyMetadata(0.0));
        public double CentreX       //X的坐标
        {
            get { return (double)GetValue(CentreXPropety); }
            set { SetValue(CentreXPropety, value); }
        }



        public static readonly DependencyProperty CentreYPropety = DependencyProperty.Register("CentreYPropety", typeof(double), typeof(PiePiece), new PropertyMetadata(0.0));
        public double CentreY       //Y的坐标
        {
            get { return (double)GetValue(CentreYPropety); }
            set { SetValue(CentreYPropety, value); }
        }



        public static readonly DependencyProperty PercentageProperty = DependencyProperty.Register("PercentageProperty", typeof(double), typeof(PiePiece), new PropertyMetadata(0.0));
        public double Percentage       //饼图片形所占饼图的百分比
        {
            get { return (double)GetValue(PercentageProperty); }
            private set { SetValue(PercentageProperty, value); }
        }



        public static readonly DependencyProperty PieceValueProperty = DependencyProperty.Register("PieceValueProperty", typeof(double), typeof(PiePiece), new PropertyMetadata(0.0));
        public double PieceValue       //该饼图形所代表的数值
        {
            get { return (double)GetValue(PieceValueProperty); }
            set { SetValue(PieceValueProperty, value); }
        }



        #endregion
        public PiePiece()
        {
            CreatePathData(0, 0);
        }
        private double lastWidth = 0;
        private double lastHeight = 0;
        private PathFigure figure;
        private void AddPoint(double x, double y)        //在图形中添加一个点
        {
            LineSegment segment = new LineSegment();
            segment.Point = new Point(x + 0.5 * StrokeThickness, y + 0.5 * StrokeThickness);
            figure.Segments.Add(segment);
        }
        private void AddLine(Point point)       //在图形中添加一条线段
        {
            LineSegment segment = new LineSegment();
            segment.Point = point;
            figure.Segments.Add(segment);
        }
        private void AddArc(Point point, Size size, bool largeArc, SweepDirection sweepDirection)       //在图形中添加一条弧线
        {
            ArcSegment segment = new ArcSegment();
            segment.Point = point;
            segment.Size = size;
            segment.IsLargeArc = largeArc;
            segment.SweepDirection = sweepDirection;
            figure.Segments.Add(segment);
        }
        private void CreatePathData(double width, double height)
        {
            if (lastWidth == width && lastHeight == height)
                return;
            lastWidth = width;
            lastHeight = height;

            Point startPoint = new Point(CentreX, CentreY);

            Point innerArcStartPoint = ComputeCartesianCoordinate(RotationAngle, InnerRadius);
            innerArcStartPoint = Offset(innerArcStartPoint, CentreX, CentreY);
            Point innerArcEndPoint = ComputeCartesianCoordinate(RotationAngle + WedgeAngle, InnerRadius);
            innerArcEndPoint = Offset(innerArcEndPoint, CentreX, CentreY);
            Point outerArcStartPoint = ComputeCartesianCoordinate(RotationAngle, Radius);
            outerArcStartPoint = Offset(outerArcStartPoint, CentreX, CentreY);
            Point outerArcEndPoint = ComputeCartesianCoordinate(RotationAngle + WedgeAngle, Radius);
            outerArcEndPoint = Offset(outerArcEndPoint, CentreX, CentreY);

            bool largeArc = WedgeAngle > 180.0;

            if (PushOut > 0)
            {
                Point offset = ComputeCartesianCoordinate(RotationAngle + WedgeAngle / 2, PushOut);
                innerArcStartPoint = Offset(innerArcStartPoint, offset.X, offset.Y);
                innerArcEndPoint = Offset(innerArcEndPoint, offset.X, offset.Y);
                outerArcStartPoint = Offset(outerArcStartPoint, offset.X, offset.Y);
                outerArcEndPoint = Offset(outerArcEndPoint, offset.X, offset.Y);
            }

            Size outerArcSize = new Size(Radius, Radius);
            Size innerArcSize = new Size(InnerRadius, InnerRadius);

            var geometry = new PathGeometry();

            figure = new PathFigure();
            figure.StartPoint = innerArcStartPoint;
            AddLine(outerArcStartPoint);
            AddArc(outerArcEndPoint, outerArcSize, largeArc, SweepDirection.Clockwise);
            AddLine(innerArcEndPoint);
            AddArc(innerArcStartPoint, innerArcSize, largeArc, SweepDirection.Counterclockwise);

            figure.IsClosed = true;
            geometry.Figures.Add(figure);
            this.Data = geometry;
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            return availableSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            CreatePathData(finalSize.Width, finalSize.Height);
            return finalSize;
        }
        private Point Offset(Point point, double offsetX, double offsetY)
        {
            point.X += offsetX;
            point.Y += offsetY;
            return point;
        }
        private Point ComputeCartesianCoordinate(double angle, double radius)
        {
            double angleRad = (Math.PI / 180.0) * (angle - 90);
            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);
            return new Point(x, y);
        }
    }
}
