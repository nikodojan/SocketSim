using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SocketSim.Helpers;
using SocketSim.Sockets;
using SocketSim.StaticLogs;

namespace SocketSim
{
    public partial class MainWindow : Window
    {
        private const string StartUdpListenerButtonLabel = "Start listening";
        private const string StopUdpListenerButtonLabel = "Stop listening";
        private TextBlock UdpLogTextBox;
        private SimpleUdpClient _udpClient;

        #region UDP TAB: Contains all UDP related methods

        /// <summary>
        /// Initializes the UI controls in the UDP tab.
        /// </summary>
        private void InitializeUdpTab()
        {
            _udpClient = new SimpleUdpClient();

            UdpDestinationIpTextBox.Text = "255.255.255.255";
            UdpDestinationPortTextBox.Text = "65000";
            UdpMessageTextBox.Text = "";

            UdpListenerIpTextBox.Text = "127.0.0.1";
            UdpListenerPortTextBox.Text = "65000";

            // log text block and scroll viewer
            UdpLogTextBox = new TextBlock();
            UdpLogTextBox.FontSize = 12;
            UdpLogTextBox.Background = Brushes.White;
            UdpLogTextBox.TextWrapping = TextWrapping.Wrap;

            UdpLogScrollViewer.Content = UdpLogTextBox;
            UdpLogScrollViewer.Height = 150;
            UdpLogScrollViewer.Background = Brushes.White;

            UdpStartListeningButton.Content = StartUdpListenerButtonLabel;

            //event handler
            UdpSendMessageButton.Click += UdpSendMessageButton_Click;
            UdpDeleteMessageButton.Click += (object sender, RoutedEventArgs e) => { UdpMessageTextBox.Text = ""; };
            _udpClient.LogChanged += OnUdpLogChanged;

            UdpStartListeningButton.Click += UdpStartListeningButton_Click;

        }

        private void UdpStartListeningButton_Click(object sender, RoutedEventArgs e)
        {
            if (UdpStartListeningButton.Content.ToString() == StartUdpListenerButtonLabel)
            {
                OnStartUdpListener();
            }
            else if (UdpStartListeningButton.Content.ToString() == StopUdpListenerButtonLabel)
            {
                OnStopUdpListener();
            }
        }

        private async Task OnStartUdpListener()
        {
            var endPoint = ParsingHelper.ParseEndpoint(UdpListenerIpTextBox.Text, UdpListenerPortTextBox.Text);
            if (endPoint != null)
            {
                _udpClient.StartListening(endPoint);
                UdpStartListeningButton.Content = StopUdpListenerButtonLabel;
            }
        }

        private async Task OnStopUdpListener()
        {
            await _udpClient.StopListening();
            UdpStartListeningButton.Content = StartUdpListenerButtonLabel;
        }


        #region UDP event handler
        /// <summary>
        /// Click event handler for Send button in UDP tab.
        /// Sends a message via UDP.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UdpSendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var endPoint = ParsingHelper.ParseEndpoint(UdpDestinationIpTextBox.Text, UdpDestinationPortTextBox.Text);
            if (endPoint != null)
            {
                await _udpClient.SendAsync(endPoint, UdpMessageTextBox.Text);
                UdpMessageTextBox.Text = "";
            }
        }

        /// <summary>
        /// Event handler to update the log text after the event was invoked.
        /// </summary>
        private void OnUdpLogChanged(object sender, EventArgs e)
        {
            UdpLogTextBox.Text += UdpLog.Log[^1];
        }


        #endregion

        #endregion
    }
}
