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
                    remindList.Add(temp);
                }
            }
        }


    }
}
