using System.Collections.ObjectModel;

namespace ZSCY_Win10.Models
{
    public class Canteen
    {
        public class Rootobject
        {
            public int Status { get; set; }
            public string Info { get; set; }
            public string Version { get; set; }
            public ObservableCollection<Datum> Data { get; set; }
        }

        public class Datum
        {
            public string name { get; set; }
            public string resume { get; set; }
            public ObservableCollection<string> url { get; set; }
        }
    }
}