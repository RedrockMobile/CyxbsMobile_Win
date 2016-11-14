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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.AddRemindPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RemindListPage : Page
    {
        ObservableCollection<MyRemind> remindList = new ObservableCollection<MyRemind>();

        public RemindListPage()
        {
            this.InitializeComponent();
            ReadDatabase();
            RemindListView.ItemsSource = remindList;
        }
        private void ReadDatabase()
        {
            using (var conn = new SQLiteConnection(new SQLitePlatformWinRT(), App.RemindListDBPath))
            {
                
                remindList.Clear();
                var list = conn.Table<RemindListDB>();
                foreach (var item in list)
                {
                    MyRemind temp = JsonConvert.DeserializeObject<MyRemind>(item.json);
                    //getDetailClass(ref temp);
                    temp.ClassDay = ClassMixDay(ref temp);
               
                    remindList.Add(temp);
                }
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
            for(int i=0;i< remind.DateItems.Count;i++)
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

    }
}
