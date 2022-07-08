using System;
using System.Collections.ObjectModel;

namespace ZSCY_Win10.ViewModels
{
    class VolunteerPageViewModel : BasePageViewModel
    {
        public VolunteerPageViewModel()
        {
            this.year1 = DateTime.Now.Year;
            this.year2 = DateTime.Now.Year - 1;
            this.year3 = DateTime.Now.Year - 2;
            this.year4 = DateTime.Now.Year - 3;

            this.yearTime1 = 0;
            this.yearTime2 = 0;
            this.yearTime3 = 0;
            this.yearTime4 = 0;

            Windows.Foundation.Size size = new Windows.Foundation.Size();
            this._elementHeight = (int)size.Height;
            this._elementWidth = (int)size.Width;
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
        private int _elementWidth1;
        public int ElementWidth1
        {
            get { return _elementWidth1 - 100; }
            set
            {
                _elementWidth1 = value;
                RaisePropertyChanged(nameof(ElementWidth1));
            }
        }
        private int _elementHeight;
        public int ElementHeight
        {
            get { return _elementHeight - 100; }
            set
            {
                _elementHeight = value;
                RaisePropertyChanged(nameof(ElementHeight));
            }
        }

        private int year1;
        private double yearTime1;
        public int Year1
        {
            get { return year1; }
            set
            {
                year1 = value;
                RaisePropertyChanged(nameof(Year1));
            }
        }

        public double YearTime1
        {
            get { return yearTime1; }
            set
            {
                yearTime1 = value;
                RaisePropertyChanged(nameof(YearTime1));
            }
        }

        private int year2;
        public double yearTime2;
        public int Year2
        {
            get { return year2; }
            set
            {
                year2 = value;
                RaisePropertyChanged(nameof(Year2));
            }
        }

        public double YearTime2
        {
            get { return yearTime2; }
            set
            {
                yearTime2 = value;
                RaisePropertyChanged(nameof(YearTime2));
            }
        }

        private int year3;
        public double yearTime3;
        public int Year3
        {
            get { return year3; }
            set
            {
                year3 = value;
                RaisePropertyChanged(nameof(Year3));
            }
        }

        public double YearTime3
        {
            get { return yearTime3; }
            set
            {
                yearTime3 = value;
                RaisePropertyChanged(nameof(YearTime3));
            }
        }

        private int year4;
        public double yearTime4;
        public int Year4
        {
            get { return year4; }
            set
            {
                year4 = value;
                RaisePropertyChanged(nameof(Year4));
            }
        }

        public double YearTime4
        {
            get { return yearTime4; }
            set
            {
                yearTime4 = value;
                RaisePropertyChanged(nameof(YearTime4));
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
