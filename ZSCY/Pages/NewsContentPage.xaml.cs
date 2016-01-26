using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewsContentPage : Page
    {
        public NewsContentPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var NewsItem = (NewsList)e.Parameter;
            if (NewsItem.Content == "加载中...")
                getContent(NewsItem.Articleid);

            TitleTextBlock.Text = NewsItem.Title;
            //ContentTextBlock.Text = NewsItem.Content_all;

            if (NewsItem.Content_all != "")
            {
                JObject newsContentobj = JObject.Parse(NewsItem.Content_all);
                if (Int32.Parse(newsContentobj["state"].ToString()) == 200)
                {
                    ContentWebView.NavigateToString((JObject.Parse(newsContentobj["data"].ToString()))["content"].ToString());

                    JArray AnnexListArray = Utils.ReadJso(newsContentobj["data"].ToString(), "annex");
                    if (AnnexListArray != null)
                    {
                        ObservableCollection<NewsContentList.Annex> annexList = new ObservableCollection<NewsContentList.Annex>();
                        for (int i = 0; i < AnnexListArray.Count; i++)
                        {
                            NewsContentList.Annex annex = new NewsContentList.Annex();
                            annex.GetAttribute((JObject)AnnexListArray[i]);
                            if (annex.name != "")
                            {
                                Uri Anneximg;
                                if (annex.name.IndexOf(".zip") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_zip.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".rar") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_rar.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".pdf") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_pdf.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".doc") != -1 || annex.name.IndexOf(".docx") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_doc.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".xls") != -1 || annex.name.IndexOf(".xlsx") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_xls.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".ppt") != -1 || annex.name.IndexOf(".pptx") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_ppt.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".jpg") != -1 || annex.name.IndexOf(".png") != -1 || annex.name.IndexOf(".gif") != -1 || annex.name.IndexOf(".bmp") != -1 || annex.name.IndexOf(".jpeg") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_image.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".mp4") != -1 || annex.name.IndexOf(".rmvb") != -1 || annex.name.IndexOf(".avi") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_video.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".mp3") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_music.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".apk") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_apk.png", UriKind.Absolute);
                                }
                                else
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_other.png", UriKind.Absolute);
                                }

                                annexList.Add(new NewsContentList.Annex { name = annex.name, address = annex.address, Anneximg = Anneximg });
                                commandBar.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                commandBar.Visibility = Visibility.Collapsed;
                                break;
                            }
                        }
                        AnnexListView.ItemsSource = annexList;
                    }
                    else
                        commandBar.Visibility = Visibility.Collapsed;
                }
            }
            if (NewsItem.Read != "")
            {
                DateReadTextBlock.Text = "发布时间:" + NewsItem.Date + " 阅读人数:" + NewsItem.Read;
            }
            else
            {
                DateReadTextBlock.Text = "发布时间:" + NewsItem.Date;
            }


            HardwareButtons.BackPressed += HardwareButtons_BackPressed;//注册重写后退按钮事件
            UmengSDK.UmengAnalytics.TrackPageStart("NewsContentPage");
        }

        private async void getContent(string Articleid)
        {
            List<KeyValuePair<String, String>> contentparamList = new List<KeyValuePair<String, String>>();
            contentparamList.Add(new KeyValuePair<string, string>("type", "jwzx"));
            contentparamList.Add(new KeyValuePair<string, string>("articleid", Articleid));
            string newsContent = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/home/news/searchcontent", contentparamList);
            //Debug.WriteLine("newsContent->" + newsContent);
            if (newsContent != "")
            {
                JObject newsContentobj = JObject.Parse(newsContent);
                if (Int32.Parse(newsContentobj["state"].ToString()) == 200)
                {
                    string content = (JObject.Parse(newsContentobj["data"].ToString()))["content"].ToString();
                    ContentWebView.NavigateToString(content);
                }
            }
        }

        private async void AnnexListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string uri = ((NewsContentList.Annex)e.ClickedItem).address;
                if (uri.IndexOf("cyxbsMobile") != -1)
                {
                    uri = await Util.NetWork.getHttpWebRequest((((NewsContentList.Annex)e.ClickedItem).address.ToString()), PostORGet: 1, fulluri: true);
                    Debug.WriteLine(uri);
                    uri = uri.Replace("\"", "");
                    uri = uri.Replace("\\", "");
                    Debug.WriteLine(uri);
                    //uri = "http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/jwzxnews/10947.xlsx";
                }
                await Launcher.LaunchUriAsync(new Uri(uri));
            }
            catch (Exception)
            {
                await new MessageDialog("附件地址异常").ShowAsync();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageEnd("NewsContentPage");
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
        }
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)//重写后退按钮，如果要对所有页面使用，可以放在App.Xaml.cs的APP初始化函数中重写。
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }
        }

        private void DownloadAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AnnexListView.SelectedIndex = -1;
            DownloadFlyout.ShowAt(page);
        }
    }
}
