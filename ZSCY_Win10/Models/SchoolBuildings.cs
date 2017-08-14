using System.Collections.ObjectModel;

namespace ZSCY_Win10.Models
{
    public class SchoolBuildings
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
            public string title { get; set; }
            public string content { get; set; }
            public string[] url { get; set; }
        }
    }
}