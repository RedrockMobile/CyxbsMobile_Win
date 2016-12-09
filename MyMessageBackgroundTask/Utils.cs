using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.Web.Http;

namespace MyMessageBackgroundTask
{
    class Utils
    {
        /// <summary>
        /// Toast
        /// </summary>
        /// <param name="text"></param>
        public static async void Toast(string text)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            XmlNodeList elements = toastXml.GetElementsByTagName("text");
            elements[0].AppendChild(toastXml.CreateTextNode(text));
            ToastNotification toast = new ToastNotification(toastXml);
            //toast.Dismissed += toast_Dismissed;//消失
            //toast.Failed += toast_Failed;//消除
            ToastNotificationManager.CreateToastNotifier().Show(toast);
            //从通知中心删除
            await Task.Delay(3000);
            ToastNotificationManager.History.Clear();
        }

        public static async void actionsToast(string title, string content, string id)
        {
            string xml = "<toast>" +
                            "<visual>" +
                                "<binding template=\"ToastGeneric\">" +
                                    "<text></text>" +
                                    "<text></text>" +
                                "</binding>" +
                            "</visual>" +
                            "<actions>" +
                                "<input id=\"content\" type=\"text\" placeHolderContent=\"请输入评论\" />" +
                                "<action content = \"确定\" arguments = \"ok" + id + "\" activationType=\"background\" />" +
                                "<action content = \"取消\" arguments = \"cancel\" activationType=\"background\"/>" +
                            "</actions >" +
                         "</toast>";
            // 创建并加载XML文档
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList elements = doc.GetElementsByTagName("text");
            elements[0].AppendChild(doc.CreateTextNode(title));
            elements[1].AppendChild(doc.CreateTextNode(content));
            // 创建通知实例
            ToastNotification notification = new ToastNotification(doc);
            //// 显示通知
            //DateTime statTime = DateTime.Now.AddSeconds(10);  //指定应传递 Toast 通知的时间
            //ScheduledToastNotification recurringToast = new ScheduledToastNotification(doc, statTime);  //创建计划的 Toast 通知对象
            //ToastNotificationManager.CreateToastNotifier().AddToSchedule(recurringToast); //向计划中添加 Toast 通知
            ToastNotifier nt = ToastNotificationManager.CreateToastNotifier();
            nt.Show(notification);
        }



        /// <summary>
        /// 获取当前日期是今年的第几周
        /// </summary>
        /// <returns></returns>
        public static int GetWeekOfYear()
        {
            //一.找到第一周的最后一天（先获取1月1日是星期几，从而得知第一周周末是几）
            int firstWeekend = 7 - Convert.ToInt32(DateTime.Parse(DateTime.Today.Year + "-1-1").DayOfWeek);

            //二.获取今天是一年当中的第几天
            int currentDay = DateTime.Today.DayOfYear;

            //三.（今天 减去 第一周周末）/7 等于 距第一周有多少周 再加上第一周的1 就是今天是今年的第几周了
            //    刚好考虑了惟一的特殊情况就是，今天刚好在第一周内，那么距第一周就是0 再加上第一周的1 最后还是1
            return Convert.ToInt32(Math.Ceiling((currentDay - firstWeekend) / 7.0)) + 1;
        }




        /// <summary>
        ///UNICODE字符转为中文 
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public static string ConvertUnicodeStringToChinese(string unicodeString)
        {
            if (string.IsNullOrEmpty(unicodeString))
                return string.Empty;

            string outStr = unicodeString;

            Regex re = new Regex("\\\\u[0123456789abcdef]{4}", RegexOptions.IgnoreCase);
            MatchCollection mc = re.Matches(unicodeString);
            foreach (Match ma in mc)
            {
                outStr = outStr.Replace(ma.Value, ConverUnicodeStringToChar(ma.Value).ToString());
            }
            return outStr;
        }

        private static char ConverUnicodeStringToChar(string str)
        {
            char outStr = Char.MinValue;
            outStr = (char)int.Parse(str.Remove(0, 2), System.Globalization.NumberStyles.HexNumber);
            return outStr;
        }


        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="text"></param>
        public static async void Message(string text, string title = "错误")
        {
            try
            {
                await new MessageDialog(text, title).ShowAsync();
            }
            catch (Exception) { Debug.WriteLine("Utils,MessageDialog异常"); }
        }

        /// <summary>
        /// 屏幕高度
        /// </summary>
        /// <returns></returns>
        public static double getPhoneHeight()
        {
            return Window.Current.Bounds.Height;
        }

        /// <summary>
        /// 屏幕宽度
        /// </summary>
        /// <returns></returns>
        public static double getPhoneWidth()
        {
            return Window.Current.Bounds.Width;
        }

        /// <summary>
        /// 时间转时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTimeStamp(DateTimeOffset date, TimeSpan time)
        {
            DateTime nowdate = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, 0);
            TimeSpan ts = nowdate.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = new DateTime(1970, 1, 1, 8, 0, 0);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public static JArray ReadJso(string jsonstring, string sdata = "data")
        {
            if (jsonstring != "")
            {
                JObject obj = JObject.Parse(jsonstring);
                //if (Int32.Parse(obj[sstatus].ToString()) == 200)
                //{
                JObject jObject = (JObject)JsonConvert.DeserializeObject(jsonstring);
                try
                {
                    string json = jObject[sdata].ToString();
                    JArray jArray = (JArray)JsonConvert.DeserializeObject(json);
                    return jArray;
                }
                catch (Exception)
                {
                }
                return null;
                //}
                //else
                //{
                //    Message("请求失败", "失败");
                //    return null;
                //}
            }

            else
            {
                Message("网络错误！", "错误");
                return null;
            }
        }

        /// <summary>
        /// 获取星期
        /// </summary>
        /// <param name="mode">默认1.返回数字，2.返回中文</param>
        /// <returns></returns>
        public static string GetWeek(int mode = 1)
        {
            DateTimeOffset date = DateTimeOffset.Now;
            if (mode == 1)
                return ((Int16)date.DayOfWeek).ToString();
            else
                switch ((Int16)date.DayOfWeek)
                {
                    case 0:
                        return "日";
                    //break;你都return了还要什么break
                    case 1:
                        return "一";
                    //break;
                    case 2:
                        return "二";
                    //break;
                    case 3:
                        return "三";
                    //break;
                    case 4:
                        return "四";
                    //break;
                    case 5:
                        return "五";
                    //break;
                    case 6:
                        return "六";
                    //break;
                    default:
                        return "";
                }
        }

        /// <summary>
        /// 异步从网络下载图片
        /// </summary>
        /// <param name="outfileName">下载保存到本地的图片文件名</param>
        /// <param name="downloadUriString">图片uri</param>
        /// <param name="scaleSize">图片尺寸</param>
        /// <returns></returns>
        public static async Task DownloadAndScale(string outfileName, string downloadUriString, Size scaleSize)
        {
            try
            {
                Uri downLoadingUri = new Uri(downloadUriString);//创建uri对象
                HttpClient client = new HttpClient();//实例化httpclient对象
                using (var response = await client.GetAsync(downLoadingUri))
                {
                    var buffer = await response.Content.ReadAsBufferAsync();//从返回的数据中读取buffer
                    var memoryStream = new InMemoryRandomAccessStream();
                    await memoryStream.WriteAsync(buffer);//将buffer写入memorystream
                    await memoryStream.FlushAsync();//刷新
                    var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(memoryStream);//解密文件流
                    //确定图片大小
                    var bt = new Windows.Graphics.Imaging.BitmapTransform();
                    bt.ScaledWidth = (uint)scaleSize.Width;
                    bt.ScaledHeight = (uint)scaleSize.Height;
                    //得到像素数值
                    var pixelProvider = await decoder.GetPixelDataAsync(
                        decoder.BitmapPixelFormat, decoder.BitmapAlphaMode, bt,
                        ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.ColorManageToSRgb);


                    //下面保存图片
                    // Now that we have the pixel data, get the destination file
                    var localFolder = ApplicationData.Current.LocalFolder;
                    //var resultsFolder = await localFolder.CreateFolderAsync("Results", CreationCollisionOption.OpenIfExists);
                    var scaledFile = await localFolder.CreateFileAsync(outfileName, CreationCollisionOption.ReplaceExisting);
                    using (var scaledFileStream = await scaledFile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        var encoder = await BitmapEncoder.CreateAsync(
                            BitmapEncoder.JpegEncoderId, scaledFileStream);
                        var pixels = pixelProvider.DetachPixelData();
                        encoder.SetPixelData(
                            decoder.BitmapPixelFormat,
                            decoder.BitmapAlphaMode,
                            (uint)scaleSize.Width,
                            (uint)scaleSize.Height,
                            decoder.DpiX,
                            decoder.DpiY,
                            pixels
                            );
                        await encoder.FlushAsync();
                    }
                }
            }
            catch (Exception) { Debug.WriteLine("工具，图片异常"); }
        }
    }
}
