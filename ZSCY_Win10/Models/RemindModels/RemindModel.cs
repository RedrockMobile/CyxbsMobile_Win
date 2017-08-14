using System;
using System.Runtime.Serialization;

namespace ZSCY_Win10.Models.RemindModels
{
    [DataContract]
    public class RemindModel
    {
        private string _Title;
        private string _Content;
        private DateTime _RemindTime;

        [DataMember(Name = "title")]
        public string Title
        {
            get
            {
                return _Title;
            }

            set
            {
                _Title = value;
            }
        }

        [DataMember(Name = "content")]
        public string Content
        {
            get
            {
                return _Content;
            }

            set
            {
                _Content = value;
            }
        }

        public DateTime RemindTime
        {
            get
            {
                return _RemindTime;
            }

            set
            {
                _RemindTime = value;
            }
        }
    }
}