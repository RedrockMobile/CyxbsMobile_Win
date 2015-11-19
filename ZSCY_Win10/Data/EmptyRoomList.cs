using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY.Data
{
    class EmptyRoomList
    {
        public string Room { get; set; }
        public string[] RoomArray { get; set; }

        public void GetAttribute(JObject EmptyRoomDetailJObject)
        {
            var emptyRoom = JArray.Parse(EmptyRoomDetailJObject["data"].ToString());
            string[] temp = new string[emptyRoom.Count];
            for (int i = 0; i < emptyRoom.Count; ++i)
            {
                temp[i] = emptyRoom[i].ToString();
            }
            RoomArray = temp;
        }
    }
}
