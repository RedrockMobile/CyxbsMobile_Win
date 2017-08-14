using System.Collections.ObjectModel;

namespace ZSCY.Models
{
    public class SexRatio
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
            public string college { get; set; }
            public string MenRatio { get; set; }
            public string WomenRatio { get; set; }
        }
    }
}