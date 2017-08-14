namespace ZSCY.Models
{
    //男女比例
    public class SexRootobject
    {
        public int Status { get; set; }
        public string Info { get; set; }
        public string Version { get; set; }
        public SexDatum[] Data { get; set; }
    }

    public class SexDatum
    {
        public string college { get; set; }
        public string MenRatio { get; set; }
        public string WomenRatio { get; set; }
    }
}