﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ZSCY_Win10.Data
{
    public sealed partial class loginControl : UserControl
    {
        private Popup m_Popup;
        private string m_TextBlockContent;

        private loginControl()
        {
            this.InitializeComponent();
            m_Popup = new Popup();
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            m_Popup.Child = this;
            this.Loaded += MessagePopupWindow_Loaded;
            this.Unloaded += MessagePopupWindow_Unloaded;
        }

        public loginControl(string showMsg) : this()
        {
            this.m_TextBlockContent = showMsg;
        }

        private void MessagePopupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbContent.Text = m_TextBlockContent;
            Window.Current.SizeChanged += MessagePopupWindow_SizeChanged; ;
        }

        private void MessagePopupWindow_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.Width = e.Size.Width;
            this.Height = e.Size.Height;
        }

        private void MessagePopupWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= MessagePopupWindow_SizeChanged; ;
        }

        public void ShowWIndow()
        {
            m_Popup.IsOpen = true;
        }

        private void DismissWindow()
        {
            m_Popup.IsOpen = false;
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            DismissWindow();
            LeftClick?.Invoke(this, e);
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            DismissWindow();
            RightClick?.Invoke(this, e);
        }

        public event EventHandler<RoutedEventArgs> LeftClick;

        public event EventHandler<RoutedEventArgs> RightClick;
    }
}