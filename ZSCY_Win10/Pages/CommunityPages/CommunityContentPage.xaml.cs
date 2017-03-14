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
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Service;
using ZSCY_Win10.Util;
using ZSCY_Win10.ViewModels.Community;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY_Win10.Pages.CommunityPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CommunityContentPage : Page
    {
        //object args;
        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        ObservableCollection<Mark> markList = new ObservableCollection<Mark>();
        bool isMark2Peo = false;//是否有回复某人
        string Mark2PeoNum = "0";
        CommunityContentViewModel ViewModel;
        List<Img> clickImgList = new List<Img>();
        int clickImfIndex = 0;
        int remarkPage = 0;
        double oldmarkScrollViewerOffset = 0;
        bool isfirst = true;
        bool issend = false;
        private static string resourceName = "ZSCY";

        public CommunityContentPage()
        {
            this.InitializeComponent();
            ViewModel = new CommunityContentViewModel();
            this.SizeChanged += (s, e) =>
            {
                if (Utils.getPhoneWidth() > 800)
                {
                    CommunityItemPhotoGrid.Margin = new Thickness(-400, 0, 0, 0);
                }
                else
                {
                    CommunityItemPhotoGrid.Margin = new Thickness(0);
                }
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            markListView.ItemsSource = markList;
            var item = e.Parameter;
            if (item is BBDDFeed)
            {
                ViewModel.BBDD = item as BBDDFeed;
                getMark();
            }
            if (item is HotFeed)
            {
                ViewModel.hotfeed = item as HotFeed;

                //BBDDFeed b = new BBDDFeed();
                //if (h.img != null)
                //{
                //    b.article_photo_src = new Img[h.img.Length];
                //    for (int i = 0; i < h.img.Length; i++)
                //    {
                //        b.article_photo_src[i] = h.img[i];
                //    }
                //}
                //b.id = h.article_id;
                //b.type_id = h.type_id;
                //b.num_id = h.num_id;
                //b.remark_num = h.remark_num;
                //b.like_num = h.like_num;
                //b.is_my_like = h.is_my_Like;
                //b.created_time = h.time;
                //b.content = h.content.contentbase == null ? h.content.content : h.content.contentbase.content;
                //b.nickname = h.nick_name;
                //b.photo_src = h.user_head;
                //ViewModel.BBDD = b;
                getMark();
            }

        }

        private async void getMark()
        {
            string id = "";
            string type_id = "";

            if (ViewModel.BBDD != null)
            {
                id = ViewModel.BBDD.id;
                type_id = ViewModel.BBDD.type_id;
            }
            else
            {
                id = ViewModel.hotfeed.article_id;
                type_id = ViewModel.hotfeed.type_id;
            }
            //TODO:未登陆时 获取评论不添加stuNum和idNum
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                paramList.Add(new KeyValuePair<string, string>("article_id", id));
                paramList.Add(new KeyValuePair<string, string>("type_id", type_id));
                if (credentialList.Count > 0)
                {
                    //paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
                    //paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
                    paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
                    paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
                }
            }
            catch { }
            paramList.Add(new KeyValuePair<string, string>("size", "15"));
            paramList.Add(new KeyValuePair<string, string>("page", remarkPage.ToString()));
            //string mark = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/ArticleRemark/getremark", paramList);
            string mark = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/NewArticleRemark/getremark", paramList);
            Debug.WriteLine(mark);
            try
            {
                if (mark != "")
                {
                    JObject obj = JObject.Parse(mark);
                    if (Int32.Parse(obj["state"].ToString()) == 200)
                    {
                        //markList.Clear();
                        JArray markListArray = Utils.ReadJso(mark);
                        if (markListArray.Count != 0)
                        {
                            isfirst = false;
                            NoMarkGrid.Visibility = Visibility.Collapsed;
                            if (ViewModel.BBDD != null)
                            {

                                //ViewModel.BBDD.remark_num = markListArray.Count.ToString();
                                if (type_id == "5")
                                {
                                    MyFeed x = await CommunityMyContentService.GetFeed(int.Parse(type_id), id);
                                    ViewModel.BBDD.remark_num = x.remark_num;
                                }
                            }
                            else
                            {
                                if (type_id == "6")
                                {
                                    //HotFeed x = await CommunityMyContentService.GetHotFeed(int.Parse(type_id), id);
                                    //ViewModel.hotfeed.remark_num+=1;
                                }
                                //ViewModel.hotfeed.remark_num = markListArray.Count.ToString();
                            }
                            //if (args is HotFeed)
                            //{
                            //    HotFeed h = args as HotFeed;
                            //    h.remark_num = ViewModel.BBDD.remark_num;
                            //}
                            for (int i = 0; i < markListArray.Count; i++)
                            {
                                Mark Markitem = new Mark();
                                Markitem.GetListAttribute((JObject)markListArray[i]);
                                markList.Add(Markitem);
                            }
                            remarkPage++;
                        }
                        else if (isfirst)
                        {
                            NoMarkGrid.Visibility = Visibility.Visible;
                        }
                        issend = false;
                    }
                }
            }
            catch (Exception) { }
        }
        int mark2PeoNameNum = 0;//回复者名字的数量   
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
            if (mark2PeoNameNum!=0&&sendMarkTextBox.Text.Length<mark2PeoNameNum+6)
            {
                sendMarkTextBox.Text = "";
                isMark2Peo = false;
                mark2PeoNameNum = 0;
            }
           
        }

        private async void sendMarkButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO:未登陆时 不能评论 
            //if (appSetting.Values.ContainsKey("idNum"))
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

                    if (ViewModel.BBDD != null)
                    {
                        id = ViewModel.BBDD.id;
                        type_id = ViewModel.BBDD.type_id;
                    }
                    else
                    {
                        id = ViewModel.hotfeed.article_id;
                        type_id = ViewModel.hotfeed.type_id;
                    }
                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("article_id", id));
                    paramList.Add(new KeyValuePair<string, string>("type_id", type_id));
                    //paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
                    //paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
                    paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
                    paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
                    paramList.Add(new KeyValuePair<string, string>("content", sendMarkTextBox.Text));
                    paramList.Add(new KeyValuePair<string, string>("answer_user_id", Mark2PeoNum));
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
                                issend = true;
                                sendMarkTextBox.Text = "";
                                markList.Clear();
                                remarkPage = 0;
                                getMark();
                                if (type_id == "6")
                                    ViewModel.hotfeed.remark_num = (int.Parse(ViewModel.hotfeed.remark_num) + 1).ToString();
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
                    isMark2Peo = false;
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

        private void markListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickMarkItem = (Mark)e.ClickedItem;
            mark2PeoNameNum = clickMarkItem.nickname.Length;
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                if (!isMark2Peo)
                {
                    isMark2Peo = true;
                }
                else
                {
                    //@lj 好了·
                    sendMarkTextBox.Text = sendMarkTextBox.Text.Substring(sendMarkTextBox.Text.IndexOf(":") + 2);
                }
                //if (clickMarkItem.stunum != appSetting.Values["stuNum"].ToString())
                if (clickMarkItem.stunum != credentialList[0].UserName)
                {
                    sendMarkTextBox.Text = "回复 " + clickMarkItem.nickname + " : " + sendMarkTextBox.Text;
                    Mark2PeoNum = clickMarkItem.stunum;
                }
                else
                {
                    isMark2Peo = false;
                    Mark2PeoNum = "0";
                }
            }
            catch
            {
                if (!isMark2Peo)
                {
                    isMark2Peo = true;
                }
                else
                {
                    //@lj 好了·
                    sendMarkTextBox.Text = sendMarkTextBox.Text.Substring(sendMarkTextBox.Text.IndexOf(":") + 2);
                }
                isMark2Peo = false;
                Mark2PeoNum = "0";
            }
            sendMarkTextBox.Focus(FocusState.Keyboard);
            sendMarkTextBox.SelectionStart = sendMarkTextBox.Text.Length;
        }

        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO:未登陆时 不能点赞
            //if (appSetting.Values.ContainsKey("idNum"))
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                if (credentialList.Count > 0)
                {
                    var b = sender as Button;
                    string num_id = b.TabIndex.ToString();
                    Debug.WriteLine(num_id);
                    Debug.WriteLine("id " + num_id.Substring(2));
                    string like_num = "";
                    if (ViewModel.BBDD != null)
                    {
                        BBDDFeed bbddfeed = ViewModel.BBDD;
                        if (bbddfeed.is_my_like == "true" || bbddfeed.is_my_like == "True")
                        {
                            like_num = await CommunityFeedsService.setPraise(bbddfeed.type_id, num_id.Substring(2), false);
                            if (like_num != "")
                            {
                                bbddfeed.like_num = like_num;
                                bbddfeed.is_my_like = "false";
                                //if (args is HotFeed)
                                //{
                                //    HotFeed h = args as HotFeed;
                                //    h.like_num = like_num;
                                //    h.is_my_Like = "false";
                                //}
                            }
                        }
                        else
                        {
                            like_num = await CommunityFeedsService.setPraise(bbddfeed.type_id, num_id.Substring(2), true);
                            if (like_num != "")
                            {
                                bbddfeed.like_num = like_num;
                                bbddfeed.is_my_like = "true";
                                //if (args is HotFeed)
                                //{
                                //    HotFeed h = args as HotFeed;
                                //    h.like_num = like_num;
                                //    h.is_my_Like = "true";
                                //}
                            }
                        }
                    }

                    if (ViewModel.hotfeed != null)
                    {
                        HotFeed hotfeed = ViewModel.hotfeed;
                        if (hotfeed.is_my_Like == "true" || hotfeed.is_my_Like == "True")
                        {
                            like_num = await CommunityFeedsService.setPraise(hotfeed.type_id, num_id.Substring(2), false);
                            if (like_num != "")
                            {
                                hotfeed.like_num = like_num;
                                hotfeed.is_my_Like = "false";
                                //if (args is HotFeed)
                                //{
                                //    HotFeed h = args as HotFeed;
                                //    h.like_num = like_num;
                                //    h.is_my_Like = "false";
                                //}
                            }
                        }
                        else
                        {
                            like_num = await CommunityFeedsService.setPraise(hotfeed.type_id, num_id.Substring(2), true);
                            if (like_num != "")
                            {
                                hotfeed.like_num = like_num;
                                hotfeed.is_my_Like = "true";
                                //if (args is HotFeed)
                                //{
                                //    HotFeed h = args as HotFeed;
                                //    h.like_num = like_num;
                                //    h.is_my_Like = "true";
                                //}
                            }
                        }
                    }
                }
                else
                {
                    var msgPopup = new Data.loginControl("您还没有登录 无法点赞~");
                    msgPopup.LeftClick += (s, c) => { Frame rootFrame = Window.Current.Content as Frame; rootFrame.Navigate(typeof(LoginPage)); };
                    msgPopup.RightClick += (s, c) => { new MessageDialog("您可以先四处逛一逛~"); };
                    msgPopup.ShowWIndow();
                }
            }
            catch
            {
                var msgPopup = new Data.loginControl("您还没有登录 无法点赞~");
                msgPopup.LeftClick += (s, c) => { Frame rootFrame = Window.Current.Content as Frame; rootFrame.Navigate(typeof(LoginPage)); };
                msgPopup.RightClick += (s, c) => { new MessageDialog("您可以先四处逛一逛~"); };
                msgPopup.ShowWIndow();
            }
        }

        private void PhotoGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            Img img = e.ClickedItem as Img;
            GridView gridView = sender as GridView;
            clickImgList = ((Img[])gridView.ItemsSource).ToList();
            clickImfIndex = clickImgList.IndexOf(img);
            CommunityItemPhotoFlipView.ItemsSource = clickImgList;
            CommunityItemPhotoFlipView.SelectedIndex = clickImfIndex;
            if (Utils.getPhoneWidth() > 800)
            {
                CommunityItemPhotoGrid.Margin = new Thickness(-400, 0, 0, 0);
            }
            CommunityItemPhotoGrid.Visibility = Visibility.Visible;
            App.isPerInfoContentImgShow = true;
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            if (CommunityItemPhotoGrid.Visibility == Visibility.Visible)
            {
                CommunityItemPhotoGrid.Visibility = Visibility.Collapsed;
                App.isPerInfoContentImgShow = false;
                //if (Utils.getPhoneWidth() > 800)
                //{
                //    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                //    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                //}
                //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
        }

        private void CommunityItemPhoto_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CommunityItemPhotoGrid.Visibility = Visibility.Collapsed;
            App.isPerInfoContentImgShow = false;
            SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void contentScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!issend)
            {
                if (contentScrollViewer.VerticalOffset > (contentScrollViewer.ScrollableHeight - 200) && contentScrollViewer.ScrollableHeight != oldmarkScrollViewerOffset)
                {
                    oldmarkScrollViewerOffset = contentScrollViewer.ScrollableHeight;
                    Debug.WriteLine("mark继续加载");
                    getMark();
                }
            }
        }

        private void CommunityItemPhotoImage_Holding(object sender, HoldingRoutedEventArgs e)
        {
            //Debug.WriteLine("Holding");
            //savePic();
        }

        private void CommunityItemPhotoImage_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            Debug.WriteLine("RightTapped");
            savePic();
        }

        private async void savePic()
        {
            var dig = new MessageDialog("是否保存此图片");
            var btnOk = new UICommand("是");
            dig.Commands.Add(btnOk);
            var btnCancel = new UICommand("否");
            dig.Commands.Add(btnCancel);
            var result = await dig.ShowAsync();
            if (null != result && result.Label == "是")
            {
                Debug.WriteLine("保存图片");
                bool saveImg = await NetWork.downloadFile(((Img)CommunityItemPhotoFlipView.SelectedItem).ImgSrc, "picture", ((Img)CommunityItemPhotoFlipView.SelectedItem).ImgSrc.Replace("http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/photo/", ""));
                if (saveImg)
                {
                    Utils.Toast("图片已保存到 \"保存的图片\"", "SavedPictures");
                }
                else
                {
                    Utils.Toast("图片保存遇到了麻烦");
                }
            }
            else if (null != result && result.Label == "否")
            {
            }
        }
        private void CommunityItemPhotoImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            DependencyObject x = VisualTreeHelper.GetParent(sender as Image);
            Grid g = x as Grid;
            var z = g.Children[0];
            ProgressRing p = z as ProgressRing;
            p.IsActive = false;
        }
    }
}
