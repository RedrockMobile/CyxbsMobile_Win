using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ZSCY_Win10.ViewModels
{
    class VolunteerPageViewModel:BasePageViewModel
    {
        public VolunteerPageViewModel()
        {
            this.year1 = DateTime.Now.Year;
            this.year2 = DateTime.Now.Year-1;
            this.year3 = DateTime.Now.Year-2;
            this.year4 = DateTime.Now.Year-3;
            this.year5 = DateTime.Now.Year-4;
            this.year6 = DateTime.Now.Year-5;
            this.year7 = DateTime.Now.Year-6;

            Windows.Foundation.Size size = new Windows.Foundation.Size();
            this._elementHeight =(int) size.Height;
            this._elementWidth= (int)size.Width;
            this._elementWidth = (int)size.Width;


        }

        public bool IsPullRefresh
        {
            get
            {
                return _isPullRefresh;
            }

            set
            {
                _isPullRefresh = value;
                RaisePropertyChanged(nameof(IsPullRefresh));
            }
        }

        bool _isPullRefresh = false;

        private int _elementWidth;
        public int ElementWidth
        {
            get { return _elementWidth; }
            set
            {
                _elementWidth = value;
                RaisePropertyChanged(nameof(ElementWidth));
            }
        }
        private double _line_y1;
        public double Line_Y1
        {
            get
            {
                return _line_y1;
            }
            set
            {
                _line_y1 = value;
                RaisePropertyChanged(nameof(Line_Y1));
            }
        }
        private double _line_y2;
        public double Line_Y2
        {
            get
            {
                return _line_y2;
            }
            set
            {
                _line_y2 = value;
                RaisePropertyChanged(nameof(Line_Y2));
            }
        }
        private double _line_y3;
        public double Line_Y3
        {
            get
            {
                return _line_y3;
            }
            set
            {
                _line_y3 = value;
                RaisePropertyChanged(nameof(Line_Y3));
            }
        }
        private double _line_y4;
        public double Line_Y4
        {
            get
            {
                return _line_y4;
            }
            set
            {
                _line_y4 = value;
                RaisePropertyChanged(nameof(Line_Y4));
            }
        }
        private double _line_y5;
        public double Line_Y5
        {
            get
            {
                return _line_y5;
            }
            set
            {
                _line_y5 = value;
                RaisePropertyChanged(nameof(Line_Y5));
            }
        }
        private double _line_y6;
        public double Line_Y6
        {
            get
            {
                return _line_y6;
            }
            set
            {
                _line_y6 = value;
                RaisePropertyChanged(nameof(Line_Y6));
            }
        }
        private double _line_y7;
        public double Line_Y7
        {
            get
            {
                return _line_y7;
            }
            set
            {
                _line_y7 = value;
                RaisePropertyChanged(nameof(Line_Y7));
            }
        }
        private int _elementWidth1;
        public int ElementWidth1
        {
            get { return _elementWidth1-100; }
            set
            {
                _elementWidth1 = value;
                RaisePropertyChanged(nameof(ElementWidth1));
            }
        }
        private int _elementHeight;
        public int ElementHeight
        {
            get { return _elementHeight-100; }
            set
            {
                _elementHeight = value;
                RaisePropertyChanged(nameof(ElementHeight));
            }
        }

        private int year1;
        public int Year1
        {
            get { return year1; }
            set
            {
                year1 = value;
                RaisePropertyChanged(nameof(Year1));
            }
        }

        private int year2;
        public int Year2
        {
            get { return year2; }
            set
            {
                year2 = value;
                RaisePropertyChanged(nameof(Year2));
            }
        }
        private int year3;
        public int Year3
        {
            get { return year3; }
            set
            {
                year3 = value;
                RaisePropertyChanged(nameof(Year3));
            }
        }
        private int year4;
        public int Year4
        {
            get { return year4; }
            set
            {
                year4 = value;
                RaisePropertyChanged(nameof(Year4));
            }
        }
        private int year5;
        public int Year5
        {
            get { return year5; }
            set
            {
                year5 = value;
                RaisePropertyChanged(nameof(Year5));
            }
        }
        private int year6;
        public int Year6
        {
            get { return year6; }
            set
            {
                year6 = value;
                RaisePropertyChanged(nameof(Year6));
            }
        }
        private int year7;
        public int Year7
        {
            get { return year7; }
            set
            {
                year7 = value;
                RaisePropertyChanged(nameof(Year7));
            }
        }

        private string elect_time;
        public string Elect_time
        {
            get { return elect_time; }
            set
            {
                elect_time = value;
                RaisePropertyChanged(nameof(Elect_time));
            }
        }

        private Models.VolunteerModel.Rootobject rootobject;
        public Models.VolunteerModel.Rootobject Rootobject
        {
            get { return rootobject; }
            set
            {
                rootobject = value;
                RaisePropertyChanged(nameof(Rootobject));
            }
        }
        private ObservableCollection<Models.VolunteerModel.Record> record_year1;
        public ObservableCollection<Models.VolunteerModel.Record> Record_year1
        {
            get { return record_year1; }
            set
            {
                record_year1 = value;
                RaisePropertyChanged(nameof(Record_year1));
            }

        }
        private ObservableCollection<Models.VolunteerModel.Record> record_year2;
        public ObservableCollection<Models.VolunteerModel.Record> Record_year2
        {
            get { return record_year2; }
            set
            {
                record_year2 = value;
                RaisePropertyChanged(nameof(Record_year2));
            }
        }

        private ObservableCollection<Models.VolunteerModel.Record> record_year3;
        public ObservableCollection<Models.VolunteerModel.Record> Record_year3
        {
            get { return record_year3; }
            set
            {
                record_year3 = value;
                RaisePropertyChanged(nameof(Record_year3));
            }
        }
        private ObservableCollection<Models.VolunteerModel.Record> record_year4;
        public ObservableCollection<Models.VolunteerModel.Record> Record_year4
        {
            get { return record_year4; }
            set
            {
                record_year4 = value;
                RaisePropertyChanged(nameof(Record_year4));
            }
        }
        private ObservableCollection<Models.VolunteerModel.Record> record_year5;
        public ObservableCollection<Models.VolunteerModel.Record> Record_year5
        {
            get { return record_year5; }
            set
            {
                record_year5 = value;
                RaisePropertyChanged(nameof(Record_year5));
            }
        }
        private ObservableCollection<Models.VolunteerModel.Record> record_year6;
        public ObservableCollection<Models.VolunteerModel.Record> Record_year6
        {
            get { return record_year6; }
            set
            {
                record_year6 = value;
                RaisePropertyChanged(nameof(Record_year6));
            }
        }
        private ObservableCollection<Models.VolunteerModel.Record> record_year7;
        public ObservableCollection<Models.VolunteerModel.Record> Record_year7
        {
            get { return record_year7; }
            set
            {
                record_year7 = value;
                RaisePropertyChanged(nameof(Record_year7));
            }
        }
        






        public string[] Time_display
        {
            get
            { return time_display; }
            set
            {
                time_display = value;
                RaisePropertyChanged(nameof(Time_display));
            }
        }

        private string[] time_display;
        private bool isopened;
        public bool Isopened
        {
            get
            {
                return isopened;
            }
            set
            {
                isopened = value;
                RaisePropertyChanged(nameof(Isopened));
            }
        }

    }
}
