using SQLite.Net.Attributes;

namespace ZSCY_Win10.Data
{
    internal class XSQqqData
    {
        public XSQqqData()
        {
        }

        public XSQqqData(int ID, string college, string qq)
        {
            this.UID = ID;
            this.college = college;
            this.qq = qq;
        }

        [PrimaryKey]
        [AutoIncrement]
        [NotNull]
        public int UID { get; set; }

        public string college { get; set; }
        public string qq { get; set; }
    }
}