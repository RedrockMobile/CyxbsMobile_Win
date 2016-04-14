using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ZSCY_Win10.Data.Community
{
    public class Img
    {
        public string ImgSrc { get; set; }
        public string ImgSmallSrc { get; set; }

        public void GetAttributes(JObject imgJObjcet)
        {
            ImgSrc = imgJObjcet["img_src"].ToString();
            ImgSmallSrc = imgJObjcet["img_small_src"].ToString();
            if (ImgSmallSrc == "")
            {
                ImgSmallSrc = "ms-appx:///Assets/StoreLogo.scale-400.png";
                Debug.WriteLine("没有图片");
            }
            else
            {
                Debug.WriteLine(ImgSmallSrc);
            }
        }
    }
}
