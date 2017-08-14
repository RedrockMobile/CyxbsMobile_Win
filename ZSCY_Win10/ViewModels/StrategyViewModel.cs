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
using ZSCY_Win10.Models;

namespace ZSCY_Win10.ViewModels
{
    public class StrategyViewModel : BasePageViewModel
    {
        private ObservableCollection<SchoolBuildings.Datum> schoolbuildings;
        public ObservableCollection<SchoolBuildings.Datum> SchoolBuildings
        {
            get
            {
                return schoolbuildings;
            }
            set
            {
                schoolbuildings = value;
                RaisePropertyChanged(nameof(SchoolBuildings));
            }
        }

        private ObservableCollection<Dormitory.Datum> dormitory;
        public ObservableCollection<Dormitory.Datum> Dormitory
        {
            get
            {
                return dormitory;
            }
            set
            {
                dormitory = value;
                RaisePropertyChanged(nameof(Dormitory));
            }
        }
        private ObservableCollection<Canteen.Datum> canteen;
        public ObservableCollection<Canteen.Datum> Canteen
        {
            get
            {
                return canteen;
            }
            set
            {
                canteen = value;
                RaisePropertyChanged(nameof(Canteen));
            }
        }
        private ObservableCollection<DailyLife.Datum> dailylife;
        public ObservableCollection<DailyLife.Datum> DailyLife
        {
            get
            {
                return dailylife;
            }
            set
            {
                dailylife = value;
                RaisePropertyChanged(nameof(DailyLife));
            }

        }
        private ObservableCollection<BeautyInNear.Datum> beautyinnear;
        public ObservableCollection<BeautyInNear.Datum> BeautyInNear
        {
            get
            {
                return beautyinnear;
            }
            set
            {
                beautyinnear = value;
                RaisePropertyChanged(nameof(BeautyInNear));
            }
        }
        private ObservableCollection<Eat.Datum> eat;
        public ObservableCollection<Eat.Datum> Eat
        {
            get
            {
                return eat;
            }
            set
            {
                eat = value;
                RaisePropertyChanged(nameof(Eat));
            }
        }
        private double _page_height;
        private double _page_width;

        public double Page_Height
        {
            get
            {
                return _page_height;
            }
            set
            {
                Photo_Height = _page_height = value;
                RaisePropertyChanged(nameof(Page_Height));
            }
        }

        public double Page_Width
        {
            get
            {
                return _page_width;
            }
            set
            {
                Photo_Width = _page_width = value;
                RaisePropertyChanged(nameof(Page_Width));
            }
        }


        public double Photo_Height
        {
            get
            {
                return (Photo_Width / 2.0);
            }
            set
            {
                RaisePropertyChanged(nameof(Photo_Height));
            }
        }

        public double Photo_Width
        {
            get
            {
                return ((_page_width - 40.0));
            }
            set
            {
                RaisePropertyChanged(nameof(Photo_Width));
            }
        }
    }

}

