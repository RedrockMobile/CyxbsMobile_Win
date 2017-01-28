using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Models.RemindModels;

namespace ZSCY_Win10.Models.RemindModels
{
    public class RemindSystemModel : RemindModel
    {
        private Guid _Id;

        public Guid Id
        {
            get
            {
                return _Id;
            }

            set
            {
                _Id = value;
            }
        }
    }
}
