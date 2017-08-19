namespace ZSCY.Models
{
    //就业率
    public class WorkRootobject
    {
        public int Status { get; set; }
        public string Info { get; set; }
        public string Version { get; set; }
        public WorkDatum[] Data { get; set; }
    }

    public class WorkDatum
    {
        public string company { get; set; }
        public string peoples { get; set; }
    }

}