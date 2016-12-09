using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigData
{
    public class SexRatio
    {
        public int Male { set; get; }
        public string MaleRatio { set; get; }
        public int Female { set; get; }
        public string FemaleRatio { set; get; }
        public SexRatio(int M,string MR,int F,string FR)
        {
            Male = M;
            MaleRatio = MR;
            Female = F;
            FemaleRatio = FR;
        }
    }
}
