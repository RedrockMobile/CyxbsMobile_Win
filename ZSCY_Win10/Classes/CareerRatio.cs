using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigData
{
    class CareerRatio
    {
        public double Employed;
        public double Abroad;
        public double Unemployed;
        public double FreeWork;
        public CareerRatio(double a, double b, double c, double d)
        {
            Employed = a;
            Abroad = b;
            Unemployed = c;
            FreeWork = d;
        }
    }
}
