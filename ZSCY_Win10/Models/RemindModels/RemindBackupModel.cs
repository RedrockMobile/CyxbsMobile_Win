using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Models.RemindModels
{
    [DataContract]
    public class RemindBackupModel:RemindModel
    {

        ///<summary>
        /// timestamp+4位随机
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name ="time")]
        public int? Time { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "stuNum")]
        public string StuNum { get; set; }

        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "idNum")]
        public string IdNum { get; set; }

        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "date")]
        public List<DateModel> DateItems { get; set; }

    }
}
