using System.Collections.ObjectModel;

namespace ZSCY.Data
{
    public class uIdList
    {
        public string uId { get; set; }
        public string uName { get; set; }
    }

    public class AuIdList
    {
        public ObservableCollection<uIdList> muIdList = new ObservableCollection<uIdList>();
        //public int week { get; set; }
    }

    public class FreeList
    {
        public int weekday { get; set; }
        public int vis { get; set; }
        public int time { get; set; }
    }
}