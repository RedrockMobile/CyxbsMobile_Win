﻿using SQLite.Net.Attributes;

namespace ZSCY_Win10.Data
{
    internal class LXQqqData
    {
        public LXQqqData()
        {
        }

        public LXQqqData(int ID, string area, string qq)
        {
            this.UID = ID;
            this.area = area;
            this.qq = qq;
        }

        [PrimaryKey]
        [AutoIncrement]
        [NotNull]
        public int UID { get; set; }

        public string area { get; set; }
        public string qq { get; set; }
    }
}