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
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Data;
using ZSCY_Win10.Models.TopicModels;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY_Win10.Pages.CommunityPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CommunityAddPage : Page
    {
        ObservableCollection<CommunityImageList> imageList = new ObservableCollection<CommunityImageList>();
        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        private static string resourceName = "ZSCY";
        Topic para;

        public CommunityAddPage()
        {
            this.InitializeComponent();
            init();
            this.SizeChanged += (s, e) =>
            {
                if (Utils.getPhoneWidth() > 850)
                    commandbar.Margin = new Thickness(400 + 49, 0, 0, 0);
                else
                {
                    if (!App.showpane)
                    {
                        //侧边关
                        commandbar.Margin = new Thickness(0);
                    }
                    else
                    {
                        //侧边开
                        commandbar.Margin = new Thickness(48, 0, 0, 0);
                    }
                }

            };
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = e.Parameter;
            if (item is Topic)
            {
                para = item as Topic;
                addContentTextBox.Text = para.keyword;
            }
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            commandbar.Visibility = Visibility.Collapsed;
        }

        private async void init()
        {
            addImgGridView.ItemsSource = imageList;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///../Assets/CommunityAddImg.png"));
            if (file != null)
            {
                file2SoftwareBitmapSource(file, 0, true);
            }
        }

        private async void addImgGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (((CommunityImageList)e.ClickedItem).imgName == "CommunityAddImg.png")
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".bmp");
                openPicker.ContinuationData["Operation"] = "img";
                IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();
                if (files.Count + imageList.Count > 10)
                {
                    Utils.Message("哦吼~最多只能上传9张图哦");
                }
                else
                {
                    if (files != null)
                    {
                        foreach (var item in imageList)
                        {
                            foreach (var file in files)
                            {
                                if (item.imgPath == file.Path)
                                {
                                    Utils.Message("此图片已添加，换一张吧 ^_^");
                                    return;
                                }
                            }
                        }
                        foreach (var file in files)
                        {
                            file2SoftwareBitmapSource(file, imageList.Count - 1);
                        }
                        if (files.Count + imageList.Count >= 10)
                        {
                            imageList.RemoveAt(imageList.Count - 1);
                        }
                    }
                }
            }
            else
            {
                var dig = new MessageDialog("是否删除此照片", "哎呀~你要删了我么？");
                var btnOk = new UICommand("是的，不要了");
                dig.Commands.Add(btnOk);
                var btnCancel = new UICommand("不，我还要");
                dig.Commands.Add(btnCancel);
                var result = await dig.ShowAsync();
                if (null != result && result.Label == "是的，不要了")
                {
                    imageList.Remove((CommunityImageList)e.ClickedItem);
                    if (imageList.Count == 8)
                    {
                        StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///../Assets/CommunityAddImg.png"));
                        if (file != null)
                        {
                            file2SoftwareBitmapSource(file, imageList.Count);
                        }
                    }
                }
                else if (null != result && result.Label == "不，我还要")
                {
                }
            }
        }

        private async void file2SoftwareBitmapSource(StorageFile file, int index = 0, bool isAddImage = false)
        {
            if (!isAddImage)
            {
                IStorageFolder applicationFolder = ApplicationData.Current.TemporaryFolder;
                await file.CopyAndReplaceAsync(await applicationFolder.CreateFileAsync(file.Name, CreationCollisionOption.ReplaceExisting));
            }

            SoftwareBitmapSource source = new SoftwareBitmapSource();
            SoftwareBitmap sb = null;
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                sb = softwareBitmap;
            }
            await source.SetBitmapAsync(sb);
            imageList.Insert(index, new CommunityImageList { imgUri = source, imgName = file.Name, imgPath = file.Path, imgAppPath = "ms-appdata:///Temp/" + file.Name });
        }

        private async void addArticleAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            addArticleAppBarButton.IsEnabled = false;
            string imgPhoto_src = "";
            string imgThumbnail_src = "";
            addProgressBar.Visibility = Visibility.Visible;
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            if (imageList.Count > 1)
            {
                int count = 0;
                if (imageList.Count == 9)
                {
                    if (imageList[8].imgName != "CommunityAddImg.png")
                    {
                        count = 9;
                    }
                    else
                    {
                        count = 8;
                    }
                }
                else
                {
                    count = imageList.Count - 1;
                }
                addProgressBar.IsIndeterminate = false;
                addProgressBar.Maximum = count;
                addProgressBar.Value = 0;
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        //string imgUp = await NetWork.headUpload(appSetting.Values["stuNum"].ToString(), imageList[i].imgAppPath, "http://hongyan.cqupt.edu.cn/cyxbsMobile/index.php/Home/Photo/uploadArticle", false);
                        string imgUp = await NetWork.headUpload(credentialList[0].UserName, imageList[i].imgAppPath, "http://hongyan.cqupt.edu.cn/cyxbsMobile/index.php/Home/Photo/uploadArticle", false);
                        if (imgUp != "" && imgUp.IndexOf("Request Entity Too Large") == -1)
                        {

                            JObject obj = JObject.Parse(imgUp);
                            if (Int32.Parse(obj["state"].ToString()) == 200)
                            {
                                string a = obj["data"].ToString();
                                JObject objdata = JObject.Parse(obj["data"].ToString());
                                //appSetting.Values["headimgdate"] = objdata["date"].ToString();
                                imageList[i].imgPhoto_src = objdata["photosrc"].ToString();
                                imageList[i].imgThumbnail_src = objdata["thumbnail_src"].ToString();

                                imgPhoto_src += ("," + objdata["photosrc"].ToString());
                                imgThumbnail_src += ("," + objdata["thumbnail_src"].ToString());
                                imgPhoto_src = imgPhoto_src.Replace("http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/photo/", "");
                                imgThumbnail_src = imgThumbnail_src.Replace("http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/photo/thumbnail/", "");
                            }

                        }
                        else if (imgUp.IndexOf("Request Entity Too Large") != -1)
                        {
                            Debug.WriteLine("第" + (int)(i + 1) + "张图片太大");
                            Utils.Toast("第" + (int)(i + 1) + "张图片超出4M限制");
                            addArticleAppBarButton.IsEnabled = true;
                            addProgressBar.Visibility = Visibility.Collapsed;
                            return;
                        }
                        else
                        {
                            Debug.WriteLine("图片上传失败");
                            Utils.Toast("发表失败");
                            addArticleAppBarButton.IsEnabled = true;
                            addProgressBar.Visibility = Visibility.Collapsed;
                            return;
                        }
                        addProgressBar.Value += 1;
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("图片上传失败");
                        addArticleAppBarButton.IsEnabled = true;
                        addProgressBar.Visibility = Visibility.Collapsed;
                        return;
                    }
                }
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        StorageFile imgfile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(imageList[i].imgAppPath));
                        imgfile.DeleteAsync();
                    }

                }
                catch (Exception) { }
                if (imgPhoto_src != "")
                {
                    imgPhoto_src = imgPhoto_src.Substring(1);
                }
                if (imgThumbnail_src != "")
                {
                    imgThumbnail_src = imgThumbnail_src.Substring(1);
                }
            }
            addProgressBar.IsIndeterminate = true;


            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            if (para != null)
            {
                paramList.Add(new KeyValuePair<string, string>("type_id", "7")); //rua
                paramList.Add(new KeyValuePair<string, string>("topic_id", para.topic_id.ToString())); //rua
            }
            else
                paramList.Add(new KeyValuePair<string, string>("type_id", "5")); //现在只有哔哔叨叨
            paramList.Add(new KeyValuePair<string, string>("title", addContentTextBox.Text));
            //paramList.Add(new KeyValuePair<string, string>("user_id", appSetting.Values["Community_people_id"].ToString())); //记得改了
            paramList.Add(new KeyValuePair<string, string>("content", addContentTextBox.Text));
            paramList.Add(new KeyValuePair<string, string>("photo_src", imgPhoto_src));
            paramList.Add(new KeyValuePair<string, string>("thumbnail_src", imgThumbnail_src));
            //paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
            //paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
            paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
            string ArticleUp = "";
            if (para != null)
            {
                ArticleUp = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Topic/addTopicArticle", paramList);
            }
            else
                ArticleUp = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Article/addArticle", paramList);
            Debug.WriteLine(ArticleUp);
            try
            {
                if (ArticleUp != "")
                {
                    JObject obj = JObject.Parse(ArticleUp);
                    if (Int32.Parse(obj["state"].ToString()) == 200)
                    {
                        Utils.Toast("发表成功");
                        Frame rootFrame = Window.Current.Content as Frame;
                        //addTitleTextBox.Text = "";
                        addContentTextBox.Text = "";
                        if (imageList.Count > 1)
                        {
                            for (int i = 0; i < imageList.Count - 1; i++)
                            {
                                imageList.RemoveAt(i);
                            }
                        }
                        page.Visibility = Visibility.Collapsed;
                        commandbar.Visibility = Visibility.Collapsed;
                        SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                        //ee.Handled = false;
                        //Frame.GoBack();
                    }
                }
            }
            catch (Exception) { }
            addArticleAppBarButton.IsEnabled = true;
            addProgressBar.Visibility = Visibility.Collapsed;

        }


        private void addTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (addContentTextBox.Text != "")
                addArticleAppBarButton.IsEnabled = true;
            else
                addArticleAppBarButton.IsEnabled = false;

        }

        private void addContentTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Debug.WriteLine("enter");
                if (!addContentTextBox.Text.EndsWith("\r\n") && addContentTextBox.Text != "")
                    addContentTextBox.Text += "\r\n";
                addContentTextBox.SelectionStart = addContentTextBox.Text.Length;
            }

        }
    }
}
