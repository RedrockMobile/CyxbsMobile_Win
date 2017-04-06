using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace ZSCY_Win10.ViewModels.ElectricChargeCheckPages
{
    public class CheckRecentChargePageViewModel
    {
        public CheckRecentChargePageViewModel(params MonthCharge[] mcs)
        {
            foreach (MonthCharge mc in mcs)
            {
                ChargeData.Add(mc);
            }
        }
        public List<MonthCharge> ChargeData = new List<MonthCharge>();
    }
    public class MonthCharge
    {
        public MonthCharge(string month, double charge, double kiloWatt, double kiloWattStartsAt, double kiloWattEndsAt, Point sp, PointCollection points)
        {
            Month = month;
            Charge = charge;
            KiloWatt = kiloWatt;
            KiloWattStartsAt = kiloWattStartsAt;
            KiloWattEndsAt = kiloWattEndsAt;
            StartPoint = sp;
            Points = points;
        }
        public string Month;
        public double Charge = 0;
        public double KiloWatt = 0;
        public double KiloWattStartsAt;
        public double KiloWattEndsAt;
        public Point StartPoint;
        public PointCollection Points;
    }
}
