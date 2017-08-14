using System;

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