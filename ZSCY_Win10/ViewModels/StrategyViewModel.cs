using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.IO;
using ZSCY.Models;

namespace ZSCY_Win10.ViewModels
{
     public class StrategyViewModel:BasePageViewModel
    {
       
        private ObservableCollection<StrategyHeader> header;

        //private ruxue_page ruxue;
        public ObservableCollection<StrategyHeader> Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
                RaisePropertyChanged(nameof(Header));
            }
        }
        private Allstring allQQInfo;
        public Allstring AllQQInfo
        {
            get
            {
                return allQQInfo;
            }

            set
            {
                allQQInfo = value;
                RaisePropertyChanged(nameof(AllQQInfo));
            }
        }
        private ObservableCollection<qinshiIntroduce> qsIntroduce;


        public ObservableCollection<qinshiIntroduce> QsIntroduce
        {
            get
            {
                return qsIntroduce;
            }

            set
            {
                qsIntroduce = value;
                RaisePropertyChanged(nameof(QsIntroduce));
            }
        }

        private ObservableCollection<richangshenghuo> richangContent;
        public ObservableCollection<richangshenghuo> RichangContent
        {
            get
            {
                return richangContent;
            }

            set
            {
                richangContent = value;
                RaisePropertyChanged(nameof(richangshenghuo));
            }
        }
        private ObservableCollection<zhoubianmeijing> mjContent;

        public ObservableCollection<zhoubianmeijing> MjContent
        {
            get
            {
                return mjContent;
            }

            set
            {
                mjContent = value;
                RaisePropertyChanged(nameof(MjContent));
            }
        }
        private ObservableCollection<zhoubianmeishi> msContent;
        public ObservableCollection<zhoubianmeishi> MsContent
        {
            get
            {
                return msContent;
            }

            set
            {
                msContent = value;
                RaisePropertyChanged(nameof(MsContent));
            }
        }



        private ObservableCollection<qindan_content> qdContent;
        public ObservableCollection<qindan_content> QdContent
        {
            get
            {
                return qdContent;
            }

            set
            {
                qdContent = value;
                RaisePropertyChanged(nameof(qdContent));
            }
        }
        /// <summary>
        /// 放缩键图标
        /// </summary>
        private ObservableCollection<string> icon;

        public ObservableCollection<string> Icon
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
        /// <summary>
        /// 缩放文本
        /// </summary>
        private ObservableCollection<string> text;
        public ObservableCollection<string> Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }
        //public ruxue_page Ruxue
        //{
        //    get
        //    {
        //        return ruxue;
        //    }

        //    set
        //    {
        //        ruxue = value;
        //        RaisePropertyChanged(nameof(Ruxue));
        //    }
        //}
        private string anquan;
        private string ruxue;
        private string jiangxuejin;
        private string xueshengshouce;
        public string Anquan
        {
            get
            {
                return anquan;
            }

            set
            {
                anquan = value;
                RaisePropertyChanged(nameof(Anquan));
            }
        }

        public string Ruxue
        {
            get
            {
                return ruxue;
            }

            set
            {
                ruxue = value;
                RaisePropertyChanged(nameof(Ruxue));

            }
        }

        public string Jiangxuejin
        {
            get
            {
                return jiangxuejin;
            }

            set
            {
                jiangxuejin = value;
                RaisePropertyChanged(nameof(Jiangxuejin));

            }
        }

        public string Xueshengshouce
        {
            get
            {
                return xueshengshouce;
            }

            set
            {
                xueshengshouce = value;
                RaisePropertyChanged(nameof(Xueshengshouce));
            }
        }

    }

}

