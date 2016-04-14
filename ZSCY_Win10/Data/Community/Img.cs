using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ZSCY_Win10.Data.Community
{
    public class Img
    {
        public string ImgId { get; set; }
        public string ImgSmallSrc { get; set; }

        public Img GetAttributes(JObject imgJObjcet)
        {
            ImgId = imgJObjcet["img_id"].ToString();
            ImgSmallSrc = imgJObjcet["img_small_src"].ToString();
            return this;
        }
    }
}
