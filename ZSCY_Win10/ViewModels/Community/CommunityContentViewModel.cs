using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Common;
using ZSCY_Win10.Data.Community;

namespace ZSCY_Win10.ViewModels.Community
{
    public class CommunityContentViewModel:ViewModelBase
    {
        private BBDDFeed bbdd;
        public BBDDFeed BBDD
        {
            get
            {
                return bbdd;
            }
            set
            {
                bbdd = value;
                OnPropertyChanged(nameof(BBDD));
            }
        }
    }
}
