using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Windows.UI.Core;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Animation;
using Windows.System;
using Windows.UI.Text;
using Windows.UI.Xaml.Media.Imaging;
using ZSCY_Win10;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FengCaiPage : Page
    {
        private int pivot_index;
        private int zuzhi_listview_index;
        private double[] pivotitem1_ver_offest;
        private ZSCY_Win10.ViewModels.FengCaiViewModel viewmodel;
        public static FengCaiPage fengcaipage;
        PaneThemeTransition PaneAnim = new PaneThemeTransition { Edge = EdgeTransitionLocation.Right };

        public FengCaiPage():base()
        {
            this.InitializeComponent();
            pivot_index = 0;
            zuzhi_listview_index = 0;

            viewmodel = new ZSCY_Win10.ViewModels.FengCaiViewModel();
            this.DataContext = viewmodel;
            fengcaipage = this;
            this.SizeChanged += FengCaiPage_SizeChanged;

            //手机物理返回键订阅事件
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += OnBackPressed;
            }
            else
            {
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += PC_BackRequested;
            }
            Transitions = new TransitionCollection();
            Transitions.Add(PaneAnim);
            ManipulationMode = ManipulationModes.TranslateX;

        }
        private void PC_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            FirstPage.firstpage.Second_Page_Back();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;

        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                await First_Step();
                pivotitem1_ver_offest = new double[viewmodel.ZuZhi.Count];
                Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    PivotItem1_Add_Content(1);
                });
                await Task.Delay(100);
                zuzhi_listview.SelectedIndex = pivot.SelectedIndex = 0;
            }
            PaneAnim.Edge = e.NavigationMode == NavigationMode.Back ? EdgeTransitionLocation.Left : EdgeTransitionLocation.Right;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            PaneAnim.Edge = e.NavigationMode != NavigationMode.Back ? EdgeTransitionLocation.Left : EdgeTransitionLocation.Right;
            base.OnNavigatingFrom(e);
        }

        private void FengCaiPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            viewmodel.Page_Width = e.NewSize.Width;
            viewmodel.Page_Height = e.NewSize.Height;
        }

        private async Task First_Step()
        {
            StorageFile file;
            string json = "";
            JObject json_object;

            #region 得到Header列表
            file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Json/fengcai_header_lists.json", UriKind.Absolute));
            json = await FileIO.ReadTextAsync(file);
            json_object = (JObject)JsonConvert.DeserializeObject(json);
            JArray headers = (JArray)json_object["header_lists"];
            ObservableCollection<Models.fengcaiheaders> header_lists = new ObservableCollection<Models.fengcaiheaders>();
            for (int i = 0; i < headers.Count; i++)
            {
                Models.fengcaiheaders item = new Models.fengcaiheaders();
                item.header = headers[i]["header"].ToString();
                header_lists.Add(item);
            }
            viewmodel.Header = header_lists;
            #endregion

            #region 得到组织列表
            file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Json/fengcai_zuzhi_lists.json", UriKind.Absolute));
            json = await FileIO.ReadTextAsync(file);
            json_object = (JObject)JsonConvert.DeserializeObject(json);
            JArray zuzhis = (JArray)json_object["zuzhi_lists"];
            ObservableCollection<Models.zuzhi> zuzhi_lists = new ObservableCollection<Models.zuzhi>();
            for (int i = 0; i < zuzhis.Count; i++)
            {
                Models.zuzhi item = new Models.zuzhi();
                item.zuzhi_name = zuzhis[i]["zuzhi"].ToString();
                zuzhi_lists.Add(item);
            }
            viewmodel.ZuZhi = zuzhi_lists;
            #endregion

            #region 得到组织介绍
            file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Json/fengcai_zuzhi_intro.json", UriKind.Absolute));
            json = await FileIO.ReadTextAsync(file);
            json_object = (JObject)JsonConvert.DeserializeObject(json);
            JArray zuzhi_intros = (JArray)json_object["zuzhi_intro"];
            ObservableCollection<Models.zuzhi_intro> intro_lists = new ObservableCollection<Models.zuzhi_intro>();
            for (int i = 0; i < zuzhi_intros.Count; i++)
            {
                Models.zuzhi_intro item = new Models.zuzhi_intro();
                item.zuzhi = new ObservableCollection<string>();
                JArray zuzhi_item = (JArray)zuzhi_intros[i]["zuzhi"];
                for (int j = 0; j < zuzhi_item.Count; j++)
                {
                    item.zuzhi.Add(zuzhi_item[j]["duanluo"].ToString());
                }
                intro_lists.Add(item);
            }
            viewmodel.Zuzhi_Intro = intro_lists;
            #endregion

            #region 得到原创重邮内容
            json = await ZSCY_Win10.Util.Request.YuanChuang_Request();
            if (json != null)
            {
                json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["Data"];
                ObservableCollection<Models.yuanchuang> yc_lists = new ObservableCollection<Models.yuanchuang>();
                for (int i = 0; i < data.Count; i++)
                {
                    Models.yuanchuang item = new Models.yuanchuang();
                    //item.introduction = data[i]["introduction"].ToString();
                    item.name = data[i]["name"].ToString();
                    item.cover = data[i]["cover"].ToString();
                    item.url = data[i]["url"].ToString();
                    //JArray photo = (JArray)data[i]["photo"];
                    //for (int j = 0; j < photo.Count; j++)
                    //{
                    //    item.photo_src = photo[j]["photo_src"].ToString();
                    //}
                    yc_lists.Add(item);
                }
                viewmodel.YuanChuang = yc_lists;
            }
            #endregion

            #region 得到最美重邮内容
            json = await ZSCY_Win10.Util.Request.ZuiMei_Request();
            if (json != null)
            {
                json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["Data"];
				ObservableCollection<Models.zuimei> zuimei_lists = new ObservableCollection<Models.zuimei>();
				for (int i = 0; i < data.Count; i++)
                {
					Models.zuimei item = new Models.zuimei();
					item.content = data[i]["content"].ToString();
					item.title = data[i]["title"].ToString();
					item.url = data[i]["url"].ToString();
					zuimei_lists.Add(item);
				}
                viewmodel.ZuiMei = zuimei_lists;
            }
            #endregion

            #region 得到优秀学子内容
            json = await ZSCY_Win10.Util.Request.XueZi_Request();
            if (json != null)
            {
                json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["Data"];
                ObservableCollection<Models.xuezi> xuezi_lists = new ObservableCollection<Models.xuezi>();
                for (int i = 0; i < data.Count; i++)
                {
                    Models.xuezi item = new Models.xuezi();
                    item.resume = data[i]["resume"].ToString();
                    item.name = data[i]["name"].ToString();
                    //JArray photo = (JArray)data[i]["url"];
                    item.url = data[i]["url"].ToString();
                    xuezi_lists.Add(item);
                }
                viewmodel.XueZi = xuezi_lists;
            }
            #endregion

            #region 得到优秀教师内容
            json = await ZSCY_Win10.Util.Request.Teather_Request();
            if (json != null)
            {
                json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["Data"];
                ObservableCollection<Models.teacher> teacher_lists = new ObservableCollection<Models.teacher>();
                for (int i = 0; i < data.Count; i++)
                {
                    Models.teacher item = new Models.teacher();
                    item.name = data[i]["name"].ToString();
                    item.url = data[i]["url"].ToString();
                    teacher_lists.Add(item);
                }
                viewmodel.Teacher = teacher_lists;
            }
            #endregion
        }

        #region 实现高斯模糊
        private void Forestglass(UIElement glasshost)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(glasshost);
            Compositor compositor = hostVisual.Compositor;
            var glassEfect = new GaussianBlurEffect
            {
                BlurAmount=15.0f,
                BorderMode=EffectBorderMode.Hard,
                Source=new ArithmeticCompositeEffect
                {
                    MultiplyAmount=0,
                    Source1Amount=0.5f,
                    Source2Amount=0.5f,
                    Source1=new CompositionEffectSourceParameter("backdropBrush"),
                    Source2=new ColorSourceEffect
                    {
                        Color=Color.FromArgb(255,245,245,245)
                    }
                }
            };
            var effectFactory = compositor.CreateEffectFactory(glassEfect);
            var backdropBrush = compositor.CreateBackdropBrush();
            var effectBrush = effectFactory.CreateBrush();
            effectBrush.SetSourceParameter("backdropBrush", backdropBrush);
            var glassVisual = compositor.CreateSpriteVisual();
            glassVisual.Brush = effectBrush;
            ElementCompositionPreview.SetElementChildVisual(glasshost, glassVisual);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            glassVisual.StartAnimation("Size", bindSizeAnimation);
        }
        #endregion
        private void PivotItem1_Add_Content(int p)
        {
            zuzhi_content.Children.Clear();
            if (p == 1)
            {
                for (int i = 0; i < viewmodel.Zuzhi_Intro[0].zuzhi.Count; i++)
                {
                    if (viewmodel.Zuzhi_Intro[0].zuzhi[i].Contains("【"))
                    {
                        zuzhi_content.Children.Add(New_TextBlock(1, viewmodel.Zuzhi_Intro[0].zuzhi[i]));
                    }
                    else
                    {
                        zuzhi_content.Children.Add(New_TextBlock(2, viewmodel.Zuzhi_Intro[0].zuzhi[i]));
                    }
                }
            }
            else if (p == 2)
            {
                for (int i = 0; i < viewmodel.Zuzhi_Intro[zuzhi_listview.SelectedIndex].zuzhi.Count; i++)
                {
                    if (viewmodel.Zuzhi_Intro[zuzhi_listview.SelectedIndex].zuzhi[i].Contains("【"))
                    {
                        zuzhi_content.Children.Add(New_TextBlock(1, viewmodel.Zuzhi_Intro[zuzhi_listview.SelectedIndex].zuzhi[i]));
                    }
                    else
                    {
                        zuzhi_content.Children.Add(New_TextBlock(2, viewmodel.Zuzhi_Intro[zuzhi_listview.SelectedIndex].zuzhi[i]));
                    }
                }
            }
        }

        private TextBlock New_TextBlock(int p, string content)
        {
            TextBlock tb = new TextBlock();
            switch (p)
            {
                case 1: //较重标题
                    {
                        tb.Text = content.Substring(1, (content.LastIndexOf('】') - content.IndexOf('【') - 1));
                        //tb.Foreground = App.APPTheme.Content_Header_Color_Brush;
                        tb.FontSize = 16;
                        tb.Margin = new Thickness(0, 3, 0, 8);
                    }; break;
                case 2: //普通内容
                    {
                        tb.Text = content;
                        FontWeight weight = new FontWeight();
                        weight.Weight = 10;
                        tb.FontWeight = weight;
                        //tb.Foreground = App.APPTheme.Gary_Color_Brush;
                        tb.FontSize = 15;
                        tb.LineHeight = 26;
                    }; break;
            }
            tb.CharacterSpacing = 100;
            tb.TextWrapping = TextWrapping.Wrap;
            return tb;
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (pivot.SelectedIndex < 0)
                {
                    pivot.SelectedIndex = pivot_index = 0;
                }
                (((pivot.Items[pivot_index] as PivotItem).Header as Grid).Children[0] as TextBlock).Foreground = App.APPTheme.Content_Header_Color_Brush;
                (((pivot.Items[pivot_index] as PivotItem).Header as Grid).Children[1] as Line).Visibility = Visibility.Collapsed;
                pivot_index = pivot.SelectedIndex;
                (((pivot.Items[pivot_index] as PivotItem).Header as Grid).Children[0] as TextBlock).Foreground = App.APPTheme.APP_Color_Brush;
                (((pivot.Items[pivot_index] as PivotItem).Header as Grid).Children[1] as Line).Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                return;
            }
        }
        Rectangle rect_old; // 上一次选中的 Rectangle
        Rectangle rect_current; // 当前选中的 Rectangle
        double posi_previous; // 手指点击屏幕时，记录当前的 pivot 中 ScrollViewer.HorizontalOffset
        ScrollViewer pivot_sv;
        bool IsMoving = false;//手指是否在滑动中
        private void zuzhi_listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pivotitem1_ver_offest[zuzhi_listview_index] = zuzhi_sc.VerticalOffset;
            PivotItem1_Add_Content(2);
            if (pivotitem1_ver_offest[zuzhi_listview.SelectedIndex] != 0.0)
            {
                zuzhi_sc.ChangeView(null, pivotitem1_ver_offest[zuzhi_listview.SelectedIndex], null, true);
            }
            else
            {
                zuzhi_sc.ChangeView(null, 0.0, null, true);
            }
            zuzhi_listview_index = zuzhi_listview.SelectedIndex;
            zuzhi_content.Visibility = Visibility.Visible;
            stackpanel_sb.Begin();

            if (firstRectInited == false) return;
            // 如果是当前显示项，则显示“下横线”
            for (int i = 0; i < zuzhi_listview.Items.Count; i++)
            {
                if (zuzhi_listview.ContainerFromIndex(i) != null)
                {
                    var grid = (zuzhi_listview.ContainerFromIndex(i) as ListViewItem).ContentTemplateRoot as Grid;
                    var rect = grid.FindName("rect") as Rectangle;
                    (rect.RenderTransform as CompositeTransform).TranslateX = 0;// 重置横向位移为 0

                    if (zuzhi_listview.SelectedIndex == i) // 当前选中项
                    {
                        zuzhi_listview.ScrollIntoView(zuzhi_listview.SelectedItem);//当滑动 pivot 时，如果 ListView选中项不在视图内，则显示
                        rect_old = rect_current;
                        rect_current = rect;
                        if (IsMoving == false) // 非手指划动 pivot
                        {
                            Rect_Slide();
                            zuzhi_listview.IsHitTestVisible = false; // 当“划动动画”在播放的时候，不再接受单击事件
                        }
                    }

                    if (IsMoving)
                        rect.Opacity = zuzhi_listview.SelectedIndex == i ? 1 : 0;//选中项显示下横向，否则隐藏
                }
            }

            IsMoving = false;

        }

        private async void yc_listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri((e.ClickedItem as Models.yuanchuang).url));
        }

        //private void XueZi_Rectangle_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Binding binding1 = new Binding();
        //    binding1.Source = viewmodel;
        //    binding1.Path = new PropertyPath("XueZi_Height");
        //    (sender as Rectangle).SetBinding(Rectangle.HeightProperty, binding1);
        //    Binding binding2 = new Binding();
        //    binding2.Source = viewmodel;
        //    binding2.Path = new PropertyPath("XueZi_Width");
        //    (sender as Rectangle).SetBinding(Rectangle.WidthProperty, binding2);
        //}

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Models.xuezi)
            {
                detail_img.ImageSource = new BitmapImage(new Uri((e.ClickedItem as Models.xuezi).url, UriKind.Absolute));
                detail_title.Text = (e.ClickedItem as Models.xuezi).name;
                detail_content.Text = (e.ClickedItem as Models.xuezi).resume;
            }
            else if (e.ClickedItem is Models.teacher)
            {
                detail_img.ImageSource = new BitmapImage(new Uri((e.ClickedItem as Models.teacher).url, UriKind.Absolute));
                detail_title.Text = (e.ClickedItem as Models.teacher).name;
            }
            detail_sc.ChangeView(null, 0, null, true);
            black_background.Visibility = Visibility.Visible;
            black_background_sb.Begin();
            Forestglass(black_background);
            detail_popup.IsOpen = true;
        }

        private void detail_popup_Closed(object sender, object e)
        {
            detail_img.ImageSource = null;
            detail_title.Text = "";
            detail_content.Text = "";
            black_background.Visibility = Visibility.Collapsed;
        }

        private void back_but_Click(object sender, RoutedEventArgs e)
        {
            FirstPage.firstpage.Second_Page_Back();
        }

        bool isExit = false;
        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
            FirstPage.firstpage.Second_Page_Back();
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                return;
            }//Frame内无内容
            if (rootFrame.CurrentSourcePageType.Name != "MainPage")
            {
                if (rootFrame.CanGoBack && e.Handled == false)
                {
                    e.Handled = true;
                    rootFrame.GoBack();
                }
            }//Frame不在MainPage页面并且可以返回则返回上一个页面并且事件未处理
            else if (e.Handled == false)
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.ShowAsync();
                statusBar.ProgressIndicator.Text = "再按一次返回键即将退出程序 ~\\(≧▽≦)/~"; // 状态栏显示文本
                statusBar.ProgressIndicator.ShowAsync();

                if (isExit)
                {
                    App.Current.Exit();
                }
                else
                {
                    isExit = true;
                    Task.Run(async () =>
                    {
                        await Task.Delay(1500);
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            await statusBar.ProgressIndicator.HideAsync();
                            await statusBar.ShowAsync(); //此处不隐藏状态栏
                        });
                        isExit = false;
                    });
                    e.Handled = true;
                }//Frame在其他页面并且事件未处理
            }
        }

        private void ScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (IsMoving && rect_current != null && pivot_sv != null)
                (rect_current.RenderTransform as CompositeTransform).TranslateX = (pivot_sv.HorizontalOffset - posi_previous) / pivot_sv.ActualWidth * rect_current.ActualWidth;
        }

        private void ScrollViewer_DirectManipulationStarted(object sender, object e)
        {
            IsMoving = true;
            pivot_sv = sender as ScrollViewer;
            posi_previous = pivot_sv.HorizontalOffset;
        }

        private void SB_Slide_Completed(object sender, object e)
        {
            zuzhi_listview.IsHitTestVisible = true;
            rect_old.Opacity = 0;
            (rect_old.RenderTransform as CompositeTransform).ScaleX = 1;
            rect_current.Opacity = 1;
            SB_Slide.Stop();

        }
        void Rect_Slide()
        {
            if (rect_old != null && rect_current != null)
            {
                // 如果设置 Width 属性，可能会导致列表宽度发生变化，所以这里使用 Scale来缩放下横线
                (rect_old.RenderTransform as CompositeTransform).ScaleX = rect_current.ActualWidth / rect_old.ActualWidth;
                var old_rect = GetBounds(rect_old, zuzhi_listview);
                var new_rect = GetBounds(rect_current, zuzhi_listview);

                // 获取 ListView 单击后，两个 Item之间的距离
                SB_Slide_TransX.KeyFrames[1].Value = new_rect.X - old_rect.X;

                Storyboard.SetTarget(SB_Slide_TransX, rect_old);
                SB_Slide.Begin();
            }
        }
        public Rect GetBounds(FrameworkElement childElement, FrameworkElement parentElement)
        {
            GeneralTransform transform = childElement.TransformToVisual(parentElement);
            return transform.TransformBounds(new Rect(0, 0, childElement.ActualWidth, childElement.ActualHeight));
        }
        bool firstRectInited = false;
        private void rect_Loaded(object sender, RoutedEventArgs e)
        {
            var r = sender as Rectangle;
            r.Loaded -= rect_Loaded;

            if (!firstRectInited && zuzhi_listview.ContainerFromIndex(0) != null)
            {
                var grid = (zuzhi_listview.ContainerFromIndex(0) as ListViewItem).ContentTemplateRoot as Grid;
                rect_current = grid.FindName("rect") as Rectangle;
                rect_current.Opacity = 1;
                firstRectInited = true;
            }
        }
    }
}
