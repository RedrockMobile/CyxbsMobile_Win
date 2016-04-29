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
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY_Win10.Pages.CommunityPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CommunityContentPage : Page
    {
        string article_id = ""; //OnNavigatedTo参数获取
        string type_id = ""; //OnNavigatedTo参数获取
        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        ObservableCollection<Mark> markList = new ObservableCollection<Mark>();
        bool isMark2Peo = false;//是否有回复某人

        public CommunityContentPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            markListView.ItemsSource = markList;
            article_id = "185";
            type_id = "5";
            getMark();
        }

        private async void getMark()
        {
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("article_id", article_id));
            paramList.Add(new KeyValuePair<string, string>("type_id", type_id));
            paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
            string mark = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/ArticleRemark/getremark", paramList);
            Debug.WriteLine(mark);

            if (mark != "")
            {
                JObject obj = JObject.Parse(mark);
                if (Int32.Parse(obj["state"].ToString()) == 200)
                {
                    markList.Clear();
                    JArray markListArray = Utils.ReadJso(mark);

                    if (markListArray.Count != 0)
                    {
                        NoMarkGrid.Visibility = Visibility.Collapsed;
                        for (int i = 0; i < markListArray.Count; i++)
                        {
                            Mark Markitem = new Mark();
                            Markitem.GetListAttribute((JObject)markListArray[i]);
                            markList.Add(Markitem);
                        }
                    }
                    else
                    {
                        NoMarkGrid.Visibility = Visibility.Visible;
                    }

                }
            }
        }

        private void sendMarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sendMarkTextBox.Text.Length > 0)
            {
                sendMarkButton.IsEnabled = true;
            }
            else
            {
                sendMarkButton.IsEnabled = false;
            }
        }

        private async void sendMarkButton_Click(object sender, RoutedEventArgs e)
        {
            sendMarkButton.IsEnabled = false;
            sendMarkProgressRing.Visibility = Visibility.Visible;
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("article_id", article_id));
            paramList.Add(new KeyValuePair<string, string>("type_id", type_id));
            paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("content", sendMarkTextBox.Text));
            string sendMark = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/ArticleRemark/postremarks", paramList);
            Debug.WriteLine(sendMark);

            if (sendMark != "")
            {
                JObject obj = JObject.Parse(sendMark);
                if (Int32.Parse(obj["state"].ToString()) == 200)
                {
                    Utils.Toast("评论成功");
                    sendMarkTextBox.Text = "";
                    getMark();
                }
                else
                {
                    Utils.Toast("评论失败");
                }
            }
            else
            {
                Utils.Toast("评论失败");
            }
            sendMarkProgressRing.Visibility = Visibility.Collapsed;
        }

        private void markListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickMarkItem = (Mark)e.ClickedItem;
            if (!isMark2Peo)
            {
                isMark2Peo = true;
            }
            else
            {
                sendMarkTextBox.Text = sendMarkTextBox.Text.Substring(sendMarkTextBox.Text.IndexOf(":") + 2);
            }
            sendMarkTextBox.Text = "回复 " + clickMarkItem.nickname + " : " + sendMarkTextBox.Text;
            sendMarkTextBox.Focus(FocusState.Keyboard);
            sendMarkTextBox.SelectionStart = sendMarkTextBox.Text.Length;
        }
    }
}
