using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ZSCY_Win10.Models.RemindPage
{

    [DataContract]
    public class DateItemModel
    {
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "week")]
        public string Week { get; set; }
        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "class")]
        public string Class { get; set; }

        ///<summary>
        /// 
        /// </summary>
        [DataMember(Name = "day")]
        public string Day { get; set; }

    }



    [DataContract]
    public class MyRemind : BaseModel
    {
        public Guid Id_system { get; set; }
        public DateTimeOffset time { get; set; }
        private string classDay;
        private string totalWeek;
        private Visibility dot;
        private Visibility rewrite;
        private Visibility deleteIcon;
        public string Tag { get; set; }

        public Visibility Rewrite
        {
            get
            {
                return rewrite;
            }

            set
            {
                rewrite = value;
                RaisePropertyChanged(nameof(Rewrite));
            }
        }

        public Visibility DeleteIcon
        {
            get
            {
                return deleteIcon;
            }

            set
            {
                deleteIcon = value;
                RaisePropertyChanged(nameof(DeleteIcon));
            }
        }

        public Visibility Dot
        {
            get
            {
                return dot;
            }

            set
            {
                dot = value;
                RaisePropertyChanged(nameof(Dot));
            }
        }


        ///<summary>
        /// timestamp+4位随机
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

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
        public List<DateItemModel> DateItems { get; set; }
        private string _title;
        private string _content;
        ///<summary>
        /// 相对上课的时间，单位分钟，如1-2课上课，7.45提醒，time应为15，如果设置为8.05提醒，则设置为-5
        /// </summary>
        [DataMember(Name = "time")]
        public string Time { get; set; }

        ///<summary>
        /// 事项的标题，不能为空
        /// </summary>
        [DataMember(Name = "title")]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        ///<summary>
        /// 事项的具体内容,可为空
        /// </summary>
        [DataMember(Name = "content")]
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                RaisePropertyChanged(nameof(Content));
            }
        }

        ///<summary>
        /// 当前学期，不传默认本学期 格式 本学期为201620171 下学期为201620172 
        /// </summary>
        [DataMember(Name = "term")]
        public string Term { get; set; }

        public string ClassDay
        {
            get
            {
                return classDay;
            }

            set
            {
                classDay = value;
                RaisePropertyChanged(ClassDay);
            }
        }

        public string TotalWeek
        {
            get
            {
                return totalWeek;
            }

            set
            {
                totalWeek = value;
                RaisePropertyChanged(TotalWeek);
            }
        }
    }

    public class AddRemindReturn
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
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }

   

}
