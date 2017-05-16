using LLQ;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Controls;
using ZSCY_Win10.Models.TopicModels;
using ZSCY_Win10.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZSCY_Win10.Pages.TopicPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TopicArticlesConent : Page
    {
        ObservableCollection<Articles> al = new ObservableCollection<Articles>();
        ObservableCollection<TopicRemark> rl = new ObservableCollection<TopicRemark>();
        Articles para = new Articles();
        int Articles_id = 0;
        private static string resourceName = "ZSCY";

        public TopicArticlesConent()
        {
            this.InitializeComponent();
            //监听注册
            LLQNotifier.Default.Register(this);
        }
        //[SubscriberCallback(typeof(LikeButtonClickEvent), NotifyPriority.Lowest, ThreadMode.Current)]
        //public void LikeChanged(LikeButtonClickEvent Event)
        //{
        //    //if (Event.num_id == al[0].article_id.ToString())
        //    //{
        //    //    if (Event.is_like == "true")
        //    //    {
        //    //        int temp = int.Parse(al[0].like_num);
        //    //        al[0].like_num = (++temp).ToString();
        //    //        al[0].is_my_Like = "true";
        //    //    }
        //    //    else if (Event.is_like == "false")
        //    //    {
        //    //        int temp = int.Parse(al[0].like_num);
        //    //        al[0].like_num = (--temp).ToString();
        //    //        al[0].is_my_Like = "false";
        //    //    }
        //    //}
        //}
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = e.Parameter;
            if (item is Articles)
            {
                Articles_id = (item as Articles).article_id;
                al.Add(item as Articles);
            }
            TopicArticles.ItemsSource = al;
            GetRemark();
        }

        public async void GetRemark()
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            if (credentialList[0] != null)
            {
                try
                {
                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("type_id", "7"));
                    paramList.Add(new KeyValuePair<string, string>("article_id", Articles_id.ToString()));
                    string Topictemp = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/NewArticleRemark/getremark", paramList);
                    JObject Tobj = JObject.Parse(Topictemp);
                    if (Int32.Parse(Tobj["status"].ToString()) == 200)
                    {
                        JArray TopicArray = Utils.ReadJso(Topictemp);
                        for (int i = 0; i < TopicArray.Count; i++)
                        {
                            TopicRemark item = new TopicRemark();
                            item.GetAttribute((JObject)TopicArray[i]);
                            rl.Add(item);
                        }
                    }
                    if (rl.Count != 0)
                        markListView.ItemsSource = rl;
                    else
                    {
                        markListView.Visibility = Visibility.Collapsed;
                        NoMarkGrid.Visibility = Visibility.Visible;
                    }
                }
                catch
                {
                    NotifyPopup notifyPopup = new NotifyPopup("网络异常 无法读取话题详情~");
                    notifyPopup.Show();
                }
            }
        }

        private void TopicItemPhotoImage_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private async void sendMarkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                if (credentialList.Count > 0)
                {
                    sendMarkButton.IsEnabled = false;
                    sendMarkProgressRing.Visibility = Visibility.Visible;
                    string id = "";
                    string type_id = "";

                    id = al[0].article_id.ToString();
                    type_id = "7";

                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("article_id", id));
                    paramList.Add(new KeyValuePair<string, string>("type_id", type_id));
                    paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
                    paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
                    paramList.Add(new KeyValuePair<string, string>("content", sendMarkTextBox.Text));
                    paramList.Add(new KeyValuePair<string, string>("answer_user_id", "0"));
                    string sendMark = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/ArticleRemark/postremarks", paramList);
                    Debug.WriteLine(sendMark);
                    try
                    {
                        if (sendMark != "")
                        {
                            JObject obj = JObject.Parse(sendMark);
                            if (Int32.Parse(obj["state"].ToString()) == 200)
                            {
                                Utils.Toast("评论成功");
                                sendMarkTextBox.Text = "";
                                rl.Clear();
                                GetRemark();
                                int temp = int.Parse(al[0].remark_num);
                                al[0].remark_num = (++temp).ToString();
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
                    catch (Exception) { }
                }
                else
                {
                    var msgPopup = new Data.loginControl("您还没有登录 无法评论帖子~");
                    msgPopup.LeftClick += (s, c) => { Frame rootFrame = Window.Current.Content as Frame; rootFrame.Navigate(typeof(LoginPage)); };
                    msgPopup.RightClick += (s, c) => { new MessageDialog("您可以先四处逛一逛~"); };
                    msgPopup.ShowWIndow();
                }
            }
            catch
            {
                var msgPopup = new Data.loginControl("您还没有登录 无法评论帖子~");
                msgPopup.LeftClick += (s, c) => { Frame rootFrame = Window.Current.Content as Frame; rootFrame.Navigate(typeof(LoginPage)); };
                msgPopup.RightClick += (s, c) => { new MessageDialog("您可以先四处逛一逛~"); };
                msgPopup.ShowWIndow();
            }
        }

        private async void likeButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            string num_id = b.Tag.ToString();
            string temp = "false";
            foreach (var item in al)
                if (item.article_id.ToString() == num_id)
                    if (item.is_my_Like == "true")
                    {
                        item.is_my_Like = temp = "false";
                        item.like_num = await LikeClick("7", item.article_id.ToString(), "false");
                    }
                    else
                    {
                        item.is_my_Like = temp = "true";
                        item.like_num = await LikeClick("7", item.article_id.ToString(), "true");
                    }
            //Debug.WriteLine(num_id); 
        }

        public async Task<string> LikeClick(string type_id, string article_id, string yOn)
        {
            string likenum = "";
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
            paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
            paramList.Add(new KeyValuePair<string, string>("type_id", type_id));
            paramList.Add(new KeyValuePair<string, string>("article_id", article_id));
            string temp = "";
            if (yOn == "true")
                temp = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Praise/addone", paramList);
            else
                temp = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Praise/cancel", paramList);

            if (temp != "")
            {
                JObject obj = JObject.Parse(temp);
                if (obj["state"].ToString() == "200")
                {
                    likenum = obj["like_num"].ToString();
                }
                else
                    likenum = "";
            }
            else
                likenum = "";
            return likenum;
        }

        private void PhotoGrid_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

    }
}
