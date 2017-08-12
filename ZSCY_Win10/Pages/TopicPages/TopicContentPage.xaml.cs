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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Controls;
using ZSCY_Win10.Models.Topic;
using ZSCY_Win10.Models.TopicModels;
using ZSCY_Win10.Pages.CommunityPages;
using ZSCY_Win10.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZSCY_Win10.Pages.TopicPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TopicContentPage : Page
    {
        public int Topic_id;
        private static string resourceName = "ZSCY";
        public Topic para = new Topic();
        ObservableCollection<Articles> al = new ObservableCollection<Articles>();

        public TopicContentPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width < 800)
                    state = "VisualState000";
                if (e.NewSize.Width > 800)
                {
                    state = "VisualState800";
                }
                VisualStateManager.GoToState(this, state, true);
                cutoffLine.Y2 = e.NewSize.Height;
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = e.Parameter;
            if (item is Topic)
            {
                para = item as Topic;
                Topic_id = para.article_num;
                Title.Text = TopicTitle.Text = para.keyword;
                if (para.imgdata != "/Assets/base.jpg")
                    TopicImg.Source = new BitmapImage(new Uri(para.imgdata));
                else
                    TopicImg.Source = new BitmapImage(new Uri("ms-appx:///Assets/base.jpg", UriKind.Absolute));
                joinNum.Text = para.join_num.ToString();
                TopicContent.Text = para.contentdata;
            }
            GetTopicArticles(para.topic_id);
            TopicArticles.ItemsSource = al;
        }

        public async void GetTopicArticles(int topicid)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            if (credentialList[0] != null)
            {
                try
                {
                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
                    paramList.Add(new KeyValuePair<string, string>("topic_id", topicid.ToString()));
                    string Topictemp = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Topic/listTopicArticle", paramList);
                    JObject Tobj = JObject.Parse(Topictemp);
                    if (Int32.Parse(Tobj["status"].ToString()) == 200)
                    {
                        string content = Tobj["data"].ToString();
                        JObject _tobj = JObject.Parse(content);
                        TopicContent item = new TopicContent();
                        item.GetAttribute(_tobj);
                        foreach (var _item in item.articles)
                            al.Add(_item);
                    }
                }
                catch
                {
                    NotifyPopup notifyPopup = new NotifyPopup("网络异常 无法读取话题详情~");
                    notifyPopup.Show();
                }
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CommunityPage));
        }
        public Frame ArticlesFrame { get { return this.cframe; } }

        private void Join_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.cframe.Navigate(typeof(CommunityAddPage), para);
            ArticlesFrame.Visibility = Visibility.Visible;
        }

        private void likeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PhotoGrid_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void TopicArticles_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.cframe.Navigate(typeof(TopicArticlesConent), e.ClickedItem);
            ArticlesFrame.Visibility = Visibility.Visible;
        }

        private async void likeButton_Click_1(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            string num_id = b.Tag.ToString();
            string temp = "false";
            foreach (var item in al)
                if (item.article_id.ToString() == num_id)
                    if (item.is_my_Like == "true")
                    {
                        item.is_my_Like = temp = "false";
                        item.like_num =await LikeClick("7",item.article_id.ToString(),"false");
                    }
                    else
                    {
                        item.is_my_Like = temp = "true";
                        item.like_num =await LikeClick("7",item.article_id.ToString(),"true");
                    }
            //LLQNotifier.Default.Notify(new LikeButtonClickEvent() { is_like = temp, num_id = num_id });
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
    }
}
