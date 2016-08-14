using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY.Models
{
    class SubjectRatio
    {
        public string Hardest { set; get; }
        public string Harder { set; get; }
        public string Hard { set; get; }
        public int HardestRatio { set; get; }
        public int HarderRatio { set; get; }
        public int HardRatio { set; get; }
        public SubjectRatio(string est,string er,string H,int estR,int erR,int HR)
        {
            Hardest = est;
            Harder = er;
            Hard = H;
            HardestRatio = estR;
            HarderRatio = erR;
            HardRatio = HR;
         }
    }
}
