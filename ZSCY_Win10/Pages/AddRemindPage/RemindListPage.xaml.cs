using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Models.RemindPage;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using Newtonsoft.Json;
using Windows.Storage;
using System.ComponentModel;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.AddRemindPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RemindListPage : Page
    {
        List<string> DeleteList = new List<string>();
        private bool isSave;
        public RemindListPage()
        {
            this.InitializeComponent();
            DatabaseMethod.ReadDatabase(Visibility.Collapsed);
            RemindListView.ItemsSource = App.remindList;
            isSave = true;
            this.SizeChanged += (s, e) =>
            {
                ListGrid1.Width = 400;
                var state = "VisualState000";
                if (e.NewSize.Width > 000)
                {
                    ListGrid1.Width = e.NewSize.Width;
                    Frame2.Width = e.NewSize.Width;
                    state = "VisualState000";
                    ListGrid2.Visibility = Visibility.Visible;
                }
                if (e.NewSize.Width > 800)
                {
                    ListGrid1.Width = 400;
                    Frame2.Width = e.NewSize.Width - 400;
                    state = "VisualState800";
                }
                VisualStateManager.GoToState(this, state, true);
            };
            SystemNavigationManager.GetForCurrentView().BackRequested += RemindListPage_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

        }

        private void RemindListPage_BackRequested(object sender, BackRequestedEventArgs e)
        {


            Frame.GoBack();
            SystemNavigationManager.GetForCurrentView().BackRequested -= RemindListPage_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Debug.WriteLine(e.SourcePageType);
            if (!isSave)//e.parameter 是否放弃
            {
                if (e.Parameter == null)
                {
                    e.Cancel = true;
                    MessageDialog dialog = new MessageDialog("你尚未保存更改，是否放弃");
                    dialog.Commands.Add(new UICommand("放弃") { Id = true });
                    dialog.Commands.Add(new UICommand("继续编辑") { Id = false });
                    var result = await dialog.ShowAsync();
                    if ((bool)result.Id)
                    {
                        e.Cancel = false;
                        Frame.Navigate(e.SourcePageType, true);
                    }
                    else
                    {
                        e.Cancel = true;

                    }
                }
            }
            else
            {

            }
        }



        //private void getDetailClass(ref MyRemind myRemind)
        //{
        //    string[] dayArray = myRemind.DateItems[0].Day.Split(',');
        //    string[] classArray = myRemind.DateItems[0].Class.Split(',');
        //    string mix = "";
        //    for (int i = 0; i < classArray.Count(); i++)
        //    {
        //        mix+= "周" + conver(int.Parse(dayArray[i]) + 1) + changeClass(int.Parse(classArray[i])) + "节、";
        //    }
        //    mix = mix.Remove(mix.Length - 1);
        //    myRemind.DateItems[0].Class = mix;

        //}



        private void EditRemindList_Click(object sender, RoutedEventArgs e)
        {
            isSave = false;
            EditRemind_Icon.Glyph = "";
            EditRemindList.Click += SaveRemindList_Click;
            foreach (var item in App.remindList)
            {
                item.Dot = Visibility.Collapsed;
                item.Rewrite = Visibility.Visible;
                item.DeleteIcon = Visibility.Visible;
            }
        }
        private void SaveRemindList_Click(object sender, RoutedEventArgs e)
        {
            isSave = true;
            EditRemind_Icon.Glyph = "";
            EditRemindList.Click += EditRemindList_Click;
            foreach (var item in App.remindList)
            {
                item.Dot = Visibility.Visible;
                item.Rewrite = Visibility.Collapsed;
                item.DeleteIcon = Visibility.Collapsed;
            }
            for (int i = 0; i < DeleteList.Count; i++)
                DatabaseMethod.DeleteRemindItem(DeleteList[i]);
        }
        private void DeleteRemindGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int i = RemindListView.SelectedIndex;
            DeleteList.Add(App.remindList[i].Tag);
            App.remindList.RemoveAt(i);
        }

        private void RewriteRemindGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int i = RemindListView.SelectedIndex;
            isSave = true;
            MyRemind remind = new MyRemind();
            remind = App.remindList[i];
            App.isLoad = false;
            ListGrid2.Visibility = Visibility.Visible;
            Frame2.Navigate(typeof(EditRemindPage), remind);
        }
    }

}
