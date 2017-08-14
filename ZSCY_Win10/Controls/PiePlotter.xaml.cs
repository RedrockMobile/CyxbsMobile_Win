using ZSCY.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ZSCY_Win10.Controls
{
    public sealed partial class PiePlotter : UserControl
    {
        #region Dependency Properties
        public static readonly DependencyProperty HoleSizeProperty = DependencyProperty.Register("HoleSize", typeof(double), typeof(PiePlotter), new PropertyMetadata(0.0));
        public double HoleSize      //内圆大小
        {
            get { return (double)GetValue(HoleSizeProperty); }
            set
            {
                SetValue(HoleSizeProperty, value);
            }
        }

        public static readonly DependencyProperty PieWidthProperty = DependencyProperty.Register("PieWidth", typeof(double), typeof(PiePlotter), new PropertyMetadata(0.0));
        public double PieWidth      //饼图宽度
        {
            get { return (double)GetValue(PieWidthProperty); }
            set
            {
                SetValue(PieWidthProperty, value);
            }
        }
        #endregion

        private List<PiePiece> piePieces = new List<PiePiece>();
        private PieDataItem CurrentItem;

        public PiePlotter()
        {
            this.InitializeComponent();
        }

        public void ShowPie()
        {
            List<PieDataItem> myCollectionView = (List<PieDataItem>)this.DataContext;
            if (myCollectionView == null)
                return;
            double halfWidth = PieWidth / 2;
            //double innerRadius = halfWidth * HoleSize;
            double innerRadius = 50;
            double total = 0;
            foreach (PieDataItem item in myCollectionView)
            {
                total += item.Value;
            }
            LayoutRoot.Children.Clear();
            piePieces.Clear();
            double accumulativeAngle = 0;
            foreach (PieDataItem item in myCollectionView)
            {
                bool selectedItem = item == CurrentItem;
                double wedgeAngle = item.Value * 360.0 / total;
                PiePiece piece = new PiePiece()
                {
                    Radius = halfWidth,
                    InnerRadius = innerRadius,
                    CentreX = halfWidth,
                    CentreY = halfWidth,
                    //PushOut = (selectedItem ? 0.5 : 0),
                    PushOut = (selectedItem ? 10 : 0),
                    WedgeAngle = wedgeAngle,
                    PieceValue = item.Value,
                    RotationAngle = accumulativeAngle,
                    Fill = item.Brush,
                    Tag = item
                };
                piece.Tapped += piece_Tapped;
                piePieces.Add(piece);
                LayoutRoot.Children.Add(piece);
                accumulativeAngle += wedgeAngle;
            }
            NotifyChange();
        }

        void piece_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PiePiece piePiece = sender as PiePiece;
            CurrentItem = piePiece.Tag as PieDataItem;
            ShowPie();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyChange([CallerMemberName]string property = null)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
