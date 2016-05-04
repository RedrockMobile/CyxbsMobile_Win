using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Data.Community
{
    class PeoInfo
    {

        public PeoInfo(string nickname, string introduction, string phone, string qq)
        {
            this.nickname = nickname;
            this.introduction = introduction;
            this.phone = phone;
            this.qq = qq;
        }

        public string id { get; set; }
        public string stunum { get; set; }
        public string introduction { get; set; }
        public string username { get; set; }
        public string nickname { get; set; }
        public string gender { get; set; }
        public string photo_thumbnail_src { get; set; }
        public string photo_src { get; set; }
        public string updated_time { get; set; }
        public string phone { get; set; }
        public string qq { get; set; }
    }
}
