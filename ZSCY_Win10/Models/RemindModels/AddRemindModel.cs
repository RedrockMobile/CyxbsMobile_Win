using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ZSCY_Win10.Models.RemindModels
{
    public class AddRemindModel : RemindBaseModel
    {
        public AddRemindModel()
        {
        }
        private string _Title;
        private string _Content;
        private string _DayAndClass;
        private string _WeekNum;
        private List<_BeforeTimeClass> _BeforeTime;

        public string DayAndClass
        {
            get
            {
                return _DayAndClass;
            }

            set
            {
                _DayAndClass = value;
                OnProperChanged(nameof(DayAndClass));
            }
        }

        public string WeekNum
        {
            get
            {
                return _WeekNum;
            }

            set
            {
                _WeekNum = value;
                OnProperChanged(nameof(WeekNum));
            }
        }

        public List<_BeforeTimeClass> BeforeTime
        {
            get
            {
                return _BeforeTime;
            }

            set
            {
                _BeforeTime = value;
            }
        }

        public string Title
        {
            get
            {
                return _Title;
            }

            set
            {
                _Title = value;
                OnProperChanged(nameof(Title));
            }
        }

        public string Content
        {
            get
            {
                return _Content;
            }

            set
            {
                _Content = value;
                OnProperChanged(nameof(Content));
            }
        }

        public class _BeforeTimeClass:INotifyPropertyChanged
        {
            private string _BeforeTimeString;
            private TimeSpan _BeforeTime;
            private Visibility _IconVisibility;

            public event PropertyChangedEventHandler PropertyChanged;

            public string BeforeTimeString
            {
                get
                {
                    return _BeforeTimeString;
                }

                set
                {
                    _BeforeTimeString = value;
                }
            }

            public TimeSpan BeforeTime
            {
                get
                {
                    return _BeforeTime;
                }

                set
                {
                    _BeforeTime = value;
                }
            }

            public Visibility IconVisibility
            {
                get
                {
                    return _IconVisibility;
                }

                set
                {
                    _IconVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IconVisibility)));
                }
            }
        }

    }
}
