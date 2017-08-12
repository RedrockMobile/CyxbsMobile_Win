using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Models.RemindModels;

namespace ZSCY_Win10.ViewModels.Remind
{
    public class WeeksPageViewModel : BasePageViewModel
    {

        public WeeksPageViewModel()
        {
            WeekNumClass.NextId = 1;
            WeekNumList = new ObservableCollection<WeekNumClass>();
            for (int i = 0; i < 20; i++)
            {
                WeekNumList.Add(new WeekNumClass());
            }
        }
        private ObservableCollection<WeekNumClass> _WeekNumList;
        public void SelectItem(int index, bool isReload = false)
        {
            WeekNumList[index].SelectItem();
            if (!isReload)
            {
                if (WeekNumList[index].IsSelected)
                {
                    App.SelWeekList.Add(index);
                }
                else
                {
                    App.SelWeekList.RemoveAt(App.SelWeekList.FindIndex(x => x == index));
                }
            }
        }
        public ObservableCollection<WeekNumClass> WeekNumList
        {
            get
            {
                return _WeekNumList;
            }

            set
            {
                _WeekNumList = value;
                RaisePropertyChanged(nameof(WeekNumList));
            }
        }
    }
}
