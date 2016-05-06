using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Common;
using ZSCY_Win10.Data.Community;

namespace ZSCY_Win10.ViewModels.Community
{
    public class CommunityMyContentViewModel : ViewModelBase
    {
        private MyFeed myVar;

        public MyFeed Item
        {
            get { return myVar; }
            set { myVar = value; OnPropertyChanged(nameof(Item)); }
        }

    }
}
