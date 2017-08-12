using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;
using ZSCY.Models;

namespace ZSCY_Win10.Models.RemindModels
{
    public class WeekNumClass : RemindBaseModel
    {
        public WeekNumClass()
        {
            WeekNum = NextId++;
            IsSelected = false;
            FontColor = new SolidColorBrush(Color.FromArgb(255, 89, 89, 89));
            BackgroundColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }
        public static DateTime OneWeek()
        {
            ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
            int num = int.Parse(appSetting.Values["nowWeek"].ToString());
            int day = Convert.ToInt32(DateTime.Today.DayOfWeek);
            int today = day == 0 ? 7 : day;
            int totalDay = (num - 1) * 7 + today - 1;
            DateTime oneWeekFrist = DateTime.Now - DateTime.Now.TimeOfDay;
           oneWeekFrist= oneWeekFrist.AddDays(-totalDay);
#if DEBUG
            oneWeekFrist = new DateTime(2017, 2, 27);
#endif
            return oneWeekFrist;
        }
        public void SelectItem()
        {
            if (!IsSelected)
            {
                IsSelected = true;
                FontColor.Color = Color.FromArgb(255, 255, 254, 254);
                BackgroundColor.Color = Color.FromArgb(255, 65, 162, 255);
            }
            else
            {
                IsSelected = false;
                FontColor.Color = Color.FromArgb(255, 89, 89, 89);
                BackgroundColor.Color = Color.FromArgb(255, 255, 255, 255);
            }
        }
        private int _WeekNum;
        private bool _IsSelected;
        private SolidColorBrush _FontColor;
        private SolidColorBrush _BackgroundColor;
        public static int NextId = 1;
        public int WeekNum
        {
            get
            {
                return _WeekNum;
            }

            set
            {
                _WeekNum = value;
            }
        }

        public SolidColorBrush FontColor
        {
            get
            {
                return _FontColor;
            }
            set
            {
                _FontColor = value;
                //OnProperChanged(nameof(FontColor));
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                _BackgroundColor = value;
                //OnProperChanged(nameof(BackgroundColor));
            }
        }

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }

            set
            {
                _IsSelected = value;
            }
        }
    }
}
