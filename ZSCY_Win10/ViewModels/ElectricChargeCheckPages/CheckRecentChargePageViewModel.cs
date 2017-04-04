using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.ViewModels.ElectricChargeCheckPages
{
    public class CheckRecentChargePageViewModel
    {
        public CheckRecentChargePageViewModel(params string[] strs)
        {
            foreach (string str in strs)
            {
                ChargeData.Add(new MonthCharge(str));
            }
        }
        public List<MonthCharge> ChargeData = new List<MonthCharge>();
    }
    public class MonthCharge
    {
        public MonthCharge(string str)
        {
            Month = str;
        }
        public string Month;
        public double Charge = 0;
        public double KiloWatt = 0;
        public double KiloWattStartsAt;
        public double KiloWattEndsAt;

    }
}
