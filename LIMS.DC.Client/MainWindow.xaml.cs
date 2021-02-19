﻿using LIMS.DC.Client.Control;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LIMS.DC.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent(); 
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs args)
        {
            HamburgerMenuGlyphItem item = (HamburgerMenuGlyphItem)args.InvokedItem;
            UserControl control = null;
            switch (item.Label)
            {
                case "基础配置":
                    control = new u_DataConfig();
                    break;
            }
            this.HamburgerMenuControl.Content = control;
        }
    }
}
