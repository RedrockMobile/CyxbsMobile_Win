using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Data.StartPage
{
    [Table("ImageList")]
    public class Database
    {
        [Column("key")]
        [PrimaryKey]
        [AutoIncrement()]
        public int Key { get; set; }
        public string Url { get; set; }
        public string StartTime { get; set; }
        public string Name { get; set; }
        public string TargetUrl { get; set; }
        /// <summary>
        /// 判断是否已缓存
        /// </summary>
        public string Id { get; set; }

    }
}
