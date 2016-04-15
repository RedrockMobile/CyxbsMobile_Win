using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SetPersonInfoPage : Page
    {
        public SetPersonInfoPage()
        {
            this.InitializeComponent();
        }

        //昵称
        string nametext ;
        //简介
        string abstracttext ;
        //手机
        string phonetext ;
        //QQ
        string qqtext ;
        bool completeness1;
        bool completeness2;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var navPage = e.Parameter;
            
            if (this.Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (this.Frame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                this.Frame.GoBack();
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void SetPersonInfoOKAppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        //昵称
        private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            nametext = nameTextBox.Text;
            Completeness();
        }

        //简介
        private void abstractTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            abstracttext = abstractTextBox.Text;
            abstractTextBlock.Text = abstracttext.Length + "/30";
            Completeness();
        }

        //手机
        private void phoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            phonetext = phoneTextBox.Text;
            string phonemessage = phoneTextBox.Text.Trim();
            isNumeric(phonemessage);
        }
        public void isNumeric(string phonemessage)
        {
            if (phonemessage != "" && Regex.IsMatch(phonemessage, @"^1\d{10}$")) //判断是否为以 1 开头的 11 位数字
            {
                //成功
                phoneTextBlock.Text = "T";
                completeness1 = true;
            }
            else
            {
                //失败
                phoneTextBlock.Text = "F";
                completeness1 = false;
            }
            Completeness();
        }

        //QQ
        private void qqTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            qqtext = qqTextBox.Text;

            Regex rex =
            new Regex(@"^\d{5,10}$");

            if (rex.IsMatch(qqtext))
            {
                qqTextBlock.Text = "T";
                completeness2 = true;
            }
            else
            {
                qqTextBlock.Text = "F";
                completeness2 = false;
            }
            Completeness();
        }

        //判定是否可以提交
        private void Completeness()
        {
            if (nametext != null && abstracttext != null && phonetext != null && qqtext != null && completeness1 == true && completeness2 == true)
            {
                SetPersonInfoButton.IsEnabled = true;
            }
            else
                SetPersonInfoButton.IsEnabled = false;
            //信息展示
            textBlocktwo.Text = "昵称：" + nametext + "简介：" + abstracttext + "手机：" + phonetext + "QQ：" + qqtext;
        }

        //完成按钮
        private void SetPersonInfoButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
