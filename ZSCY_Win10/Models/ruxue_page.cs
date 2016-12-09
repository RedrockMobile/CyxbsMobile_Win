using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY.Models
{
    public class ruxue_page : BaseModel
    {
        //    private string anquan;
        //    private string ruxue;
        //    private string jiangxuejin;
        //    private string xueshengshouce;
        //    public string Anquan
        //    {
        //        get
        //        {
        //            return anquan;
        //        }

        //        set
        //        {
        //            anquan = value;
        //            RaisePropertyChanged(nameof(Anquan));
        //        }
        //    }

        //    public string Ruxue
        //    {
        //        get
        //        {
        //            return ruxue;
        //        }

        //        set
        //        {
        //            ruxue = value;
        //            RaisePropertyChanged(nameof(Ruxue));

        //        }
        //    }

        //    public string Jiangxuejin
        //    {
        //        get
        //        {
        //            return jiangxuejin;
        //        }

        //        set
        //        {
        //            jiangxuejin = value;
        //            RaisePropertyChanged(nameof(Jiangxuejin));

        //        }
        //    }

        //    public string Xueshengshouce
        //    {
        //        get
        //        {
        //            return xueshengshouce;
        //        }

        //        set
        //        {
        //            xueshengshouce = value;
        //            RaisePropertyChanged(nameof(Xueshengshouce));
        //        }}
        private string []icon;

        public string []Icon
        {
            get
            {
                return icon;
            }

            set
            {
                icon = value;
                RaisePropertyChanged(nameof(Icon));
            }
        }
    }
}
