using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Models.RemindModels
{
    [Table("RemindList")]
    public class DataBaseModel:RemindBaseModel
    {


        [Column("num")]
        [PrimaryKey]
        [AutoIncrement()]
        public int Num { get; set; }
        /// <summary>
        /// 网络获取的id，用于删改数据库和网络备份
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 每个通知的id，用于删和修改系统提醒
        /// </summary>
        public string Id_system { get; set; }
        public string json { get; set; }

    }
}
