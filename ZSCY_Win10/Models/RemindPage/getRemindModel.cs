using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace ZSCY_Win10.Models.RemindPage
{
    [DataContract]
    public class RemindItemTime
    {
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "class")]
        public int Class { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "day")]
        public int Day { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "week")]
        public List<int> WeekItems { get; set; }
    }
    [DataContract]
    public class GetDataItemModel
    {
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "id")]
        public long Id { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "time")]
        public string Time { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "content")]
        public string Content { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "date")]
        public List<RemindItemTime> DateItems { get; set; }
    }
    [DataContract]
    public class GetRemindModel
    {
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "status")]
        public int Status { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "info")]
        public string Info { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "term")]
        public int Term { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "stuNum")]
        public long StuNum { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "data")]
        public List<GetDataItemModel> DataItems { get; set; }
    }
}
