﻿using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SocketSim.Helpers;
using SocketSim.Sockets;
using SocketSim.StaticLogs;

namespace SocketSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeMenu();
            InitializeTcpServerTab();
            InitializeTcpClientTab();
            InitializeUdpTab();
        }

        private void InitializeMenu()
        {
            fileMenuExitButton.Click += (object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        }

    }
}
