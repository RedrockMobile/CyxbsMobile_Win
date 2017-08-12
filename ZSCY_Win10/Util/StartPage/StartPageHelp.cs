using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using ZSCY_Win10.Data.StartPage;
using ZSCY_Win10.Models.StartPageModels;

namespace ZSCY_Win10.Util.StartPage
{
    public class StartPageHelp
    {
        public readonly static string StartImageDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "StartImage.db");
        private string ImagePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "");
        private static StorageFolder _ImagesPath;
        public static StorageFolder ImagesPath
        {
            get => _ImagesPath;
            private set => _ImagesPath = value;
        }
        static StartPageHelp()
        {
            GetDBPath();
        }

        private static async void GetDBPath()
        {
            ImagesPath = await ApplicationData.Current.LocalFolder.CreateFolderAsync("images_cache", CreationCollisionOption.OpenIfExists);
        }
        public static bool HasImage()
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), StartPageHelp.StartImageDB))
            {
                if (conn.Table<Database>().Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns>图片缓存路径</returns>
        public async static Task DownloadPictrue(string url, string name)
        {
            WriteableBitmap wb = await GetWriteableBitmapAsync(url);
            await SaveImageAsync(wb, name);
        }
        #region 数据库方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataItem"></param>
        /// <returns>true：数据库中无此数据，并已经插入</returns>
        public static bool InserDatabase(Database dataItem)
        {
            bool isSuccess = true;
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), StartImageDB))
            {
                conn.CreateTable<Database>();
                var list = conn.Table<Database>().Where(x => x.Id == dataItem.Id);
                isSuccess = (list.Count() > 0) ? false : true;
                if (isSuccess)
                {
                    conn.Insert(dataItem);
                }
            }
            return isSuccess;
        }
        public static bool GetImageFromDB(ref Database databaseTemp)
        {
            bool isSuccess = false;
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), StartImageDB))
            {
                try
                {

                    var list = conn.Table<Database>();
                    var array = list.ToList();
                    array.Sort((x, y) =>
                    {
#if DEBUG
                        return Convert.ToDateTime(x.StartTime) > Convert.ToDateTime(y.StartTime) ? -1 : 1;

#else
                        return Convert.ToDateTime(x.StartTime) > Convert.ToDateTime(y.StartTime) ? 1 : -1;

#endif
                    });
                    Debug.WriteLine(array.Count() + list.Count());
                    foreach (var item in array)
                    {
                        Debug.WriteLine(item);
#if DEBUG
                        databaseTemp = item;
                        isSuccess = true;

#else

                        if (DateTime.Now < Convert.ToDateTime(item.StartTime).AddDays(2) && DateTime.Now > Convert.ToDateTime(item.StartTime))
                        {
                            databaseTemp = item;
                            isSuccess = true;
                        }
                        else
                        {
                            if (DateTime.Now > Convert.ToDateTime(item.StartTime))
                            {
                                File.Delete(item.Url);
                                DeleteDatabase(item);
                            }
                        }
#endif
                    }
                }
                catch (Exception ex)
                {

                    conn.CreateTable<Database>();
                }


            }
            return isSuccess;
        }
        public static void DeleteDatabase(Database dataItem)
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), StartImageDB))
            {
                conn.Delete(dataItem);
            }
        }

        #endregion
        #region 图片缓存辅助方法
        public static DateTime GetTime(string time)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = Convert.ToDateTime(time);
            }
            catch (Exception)
            {
                Debug.WriteLine("图片时间格式不符合xxxx-xx-xx hh:mm:ss");
            }
            return dt;
        }
        private async static Task<IBuffer> getBufferHttp(string url)
        {

            Windows.Web.Http.HttpClient http = new Windows.Web.Http.HttpClient();
            var response = await http.GetBufferAsync(new Uri(url));
            return response;

        }

        private async static Task<WriteableBitmap> GetWriteableBitmapAsync(string url)
        {
            try
            {
                IBuffer buffer = await getBufferHttp(url);
                if (buffer != null)
                {
                    BitmapImage bi = new BitmapImage();
                    WriteableBitmap wb = null;
                    Stream stream1;
                    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                    {
                        stream1 = stream.AsStreamForWrite();
                        await stream1.WriteAsync(buffer.ToArray(), 0, (int)buffer.Length);
                        await stream1.FlushAsync();
                        stream.Seek(0);
                        await bi.SetSourceAsync(stream);
                        wb = new WriteableBitmap(bi.PixelWidth, bi.PixelHeight);
                        stream.Seek(0);
                        await wb.SetSourceAsync(stream);
                        return wb;
                    }
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }

        private async static Task SaveImageAsync(WriteableBitmap image, string filename)
        {
            try
            {
                if (image == null)
                {
                    return;
                }
                Guid BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
                if (filename.EndsWith("jpg"))
                    BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
                else if (filename.EndsWith("png"))
                    BitmapEncoderGuid = BitmapEncoder.PngEncoderId;
                else if (filename.EndsWith("bmp"))
                    BitmapEncoderGuid = BitmapEncoder.BmpEncoderId;
                else if (filename.EndsWith("tiff"))
                    BitmapEncoderGuid = BitmapEncoder.TiffEncoderId;
                else if (filename.EndsWith("gif"))
                    BitmapEncoderGuid = BitmapEncoder.GifEncoderId;
                var file = await ImagesPath.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                    Stream pixelStream = image.PixelBuffer.AsStream();
                    byte[] pixels = new byte[pixelStream.Length];
                    await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                              (uint)image.PixelWidth,
                              (uint)image.PixelHeight,
                              96.0,
                              96.0,
                              pixels);
                    await encoder.FlushAsync();
                }
            }
            catch
            {
                return;
            }

        }
        #endregion
    }
}
