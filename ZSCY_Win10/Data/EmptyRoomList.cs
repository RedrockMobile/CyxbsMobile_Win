using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ZSCY.Data
{
    internal class EmptyRoomList
    {
        public string Room { get; set; }
        public string[] RoomArray { get; set; }

        public string Floor { get; set; }
        public List<string> Rooms { get; set; }

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