using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace ZSCY_Win10.Data.Community
{
    public class Img : IFeeds
    {
        public string ImgSrc { get; set; }
        public string ImgSmallSrc { get; set; }

        public void GetAttributes(JObject imgJObjcet)
        {
            string img = imgJObjcet["img_src"].ToString();
            string imgsmall = imgJObjcet["img_small_src"].ToString();
            if (img == "")
            {
                ImgSrc = ImgSmallSrc = "ms-appx:///Assets/StoreLogo.scale-400.png";
                Debug.WriteLine("没有图片");
            }
            else
            {
                ImgSrc = img;
                ImgSmallSrc = imgsmall;
                Debug.WriteLine(ImgSmallSrc);
            }
        }
    }
}