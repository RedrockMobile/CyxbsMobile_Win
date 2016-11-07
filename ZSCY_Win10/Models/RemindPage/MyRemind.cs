using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Models.RemindPage
{
    public class MyRemind
    {
        public Guid Id { get; set; }
        public DateTime RemindTime { get; set; }
        public string RemindTitle { get; set; }
        public string RemindContent { get; set; }

    }
}
