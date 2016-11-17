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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.AddRemindPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RemindListPage : Page
    {
        ObservableCollection<MyRemind> remindList = new ObservableCollection<MyRemind>();
        List<string> DeleteList = new List<string>();
        private bool isSave;
        public RemindListPage()
        {
            this.InitializeComponent();
            ReadDatabase();
            RemindListView.ItemsSource = remindList;
            isSave = true;
            this.SizeChanged += (s, e) =>
              {
                  ListGrid1.Width = 400;
              };
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



        private void ReadDatabase()
        {
            try
            {

                using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
                {

                    remindList.Clear();
                    var list = conn.Table<RemindListDB>();
                    foreach (var item in list)
                    {
                        MyRemind temp = JsonConvert.DeserializeObject<MyRemind>(item.json);
                        //getDetailClass(ref temp);
                        temp.Tag = item.Id_system;
                        temp.ClassDay = ClassMixDay(ref temp);
                        temp.Dot = Visibility.Visible;
                        temp.Rewrite = Visibility.Collapsed;
                        temp.DeleteIcon = Visibility.Collapsed;
                        remindList.Add(temp);
                    }
                }
            }
            catch
            {
                var conn=new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath);
                conn.CreateTable<RemindListDB>();
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

        private string ClassMixDay(ref MyRemind remind)
        {
            string temp = "";
            for (int i = 0; i < remind.DateItems.Count; i++)
            {
                temp += ConvertDay(int.Parse(remind.DateItems[i].Day)) + ConvertClass(int.Parse(remind.DateItems[i].Class)) + "节、";
            }

            temp = temp.Remove(temp.Length - 1);
            return temp;
        }
        private string ConvertClass(int i)
        {
            switch (i)
            {
                case 0:
                    return "12";
                case 1:
                    return "34";
                case 2:
                    return "56";
                case 3:
                    return "78";
                case 4:
                    return "910";
                case 5:
                    return "1112";
                default:
                    return "";
            }

        }
        private string ConvertDay(int i)
        {
            switch (i)
            {
                case 0:
                    return "周一";
                case 1:
                    return "周二";
                case 2:
                    return "周三";
                case 3:
                    return "周四";
                case 4:
                    return "周五";
                case 5:
                    return "周六";
                case 6:
                    return "周日";
                default:
                    return "";
            }

        }

        private void EditRemindList_Click(object sender, RoutedEventArgs e)
        {
            isSave = false;
            EditRemind_Icon.Glyph = "";
            EditRemindList.Click += SaveRemindList_Click;
            foreach (var item in remindList)
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
            foreach (var item in remindList)
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
            DeleteList.Add(remindList[i].Tag);
            remindList.RemoveAt(i);
        }

        private void RewriteRemindGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int i = RemindListView.SelectedIndex;
            isSave = true;
            MyRemind remind = new MyRemind();
            remind = remindList[i];
            App.isLoad = false;
            Frame2.Navigate(typeof(EditRemindPage), remind);
        }
    }

}
