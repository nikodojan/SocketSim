using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SocketSim.Exceptions;
using SocketSim.Helpers;
using SocketSim.Sockets;
using SocketSim.StaticLogs;

namespace SocketSim
{
    /// <summary>
    /// Partial class that handles the TCP client tab in the UI.
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ClientConnectButtonLabel = "Connect";
        private const string ClientDisconnectButtonLabel = "Disconnect";

        private TextBlock ClientLogTextBox;

        private SimpleTcpClient _client;

        #region TCP CLIENT TAB : Contains all TCP client related methods
        /// <summary>
        /// Initializes the UI controls in the TCP client tab.
        /// </summary>
        private void InitializeTcpClientTab()
        {
            ClientIpTextBox.Text = "127.0.0.1";
            ClientPortTextBox.Text = "4646";

            // log text block and scroll viewer
            ClientLogTextBox = new TextBlock();
            ClientLogTextBox.FontSize = 12;
            ClientLogTextBox.Background = Brushes.White;
            ClientLogTextBox.TextWrapping = TextWrapping.Wrap;

            ClientLogScrollViewer.Content = ClientLogTextBox;
            ClientLogScrollViewer.Height = 150;
            ClientLogScrollViewer.Background = Brushes.White;

            ClientClearLogButton.Click += ClientClearLogButton_Click;

            ClientConnectButton.Click += ClientConnectButton_Click;
            ClientConnectButton.Content = ClientConnectButtonLabel;

            ClientMessageTextBox.IsEnabled = false;
            ClientSendMessageButton.IsEnabled = false;
            ClientDeleteMessageButton.IsEnabled = false;

            ClientSendMessageButton.Click += ClientSendMessageButton_Click;
            ClientDeleteMessageButton.Click += ClientDeleteMessageButton_Click;

            
        }

        private void ClientConnectButton_Click(object sender, RoutedEventArgs e)
        {
            switch (ClientConnectButton.Content)
            {
                case "Connect":
                    Connect();
                    break;
                case "Disconnect":
                    Disconnect();
                    break;
            };
        }

        public async Task Connect()
        {
            try
            {
                var endPoint = ParsingHelper.TryParseEndpoint(ClientIpTextBox.Text, ClientPortTextBox.Text);
                _client = new SimpleTcpClient();
                _client.LogChanged += OnClientLogChanged;
                _client.Connect(endPoint);
                SwitchClientControls_OnConnect(this, EventArgs.Empty);
            }
            catch (EndPointParserException e)
            {
                MessageBox.Show(e.Message, "Address error");
                SwitchClientControls_OnDisconnect(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                SwitchClientControls_OnDisconnect(this, EventArgs.Empty);
            }
        }

        private async Task Disconnect()
        {
            try
            {
                _client?.Disconnect();
                SwitchClientControls_OnDisconnect(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                SwitchClientControls_OnDisconnect(this, EventArgs.Empty);
            }
        }

        #region Event handlers
        private void ClientDeleteMessageButton_Click(object sender, RoutedEventArgs e)
        {
            ClientMessageTextBox.Text = "";
        }
        private async void ClientSendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _client.Send(ClientMessageTextBox.Text);
            }
            catch (Exception exception)
            {

            }
        }

        /// <summary>
        /// When "Connect" button is clicked: Switches label to "Disconnect" and enables the message controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchClientControls_OnConnect(object sender, EventArgs e)
        {
            ClientConnectButton.Content = ClientDisconnectButtonLabel;
            ClientIpTextBox.IsEnabled = false;
            ClientPortTextBox.IsEnabled = false;

            ClientMessageTextBox.IsEnabled = true;
            ClientSendMessageButton.IsEnabled = true;
            ClientDeleteMessageButton.IsEnabled = true;
        }
        /// <summary>
        /// When "Disonnect" button is clicked: Switches label to "Connect" and disables the message controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchClientControls_OnDisconnect(object sender, EventArgs e)
        {
            ClientConnectButton.Content = ClientConnectButtonLabel;
            ClientIpTextBox.IsEnabled = true;
            ClientPortTextBox.IsEnabled = true;

            ClientMessageTextBox.IsEnabled = false;
            ClientSendMessageButton.IsEnabled = false;
            ClientDeleteMessageButton.IsEnabled = false;
        }

        private void OnClientLogChanged(object sender, EventArgs e)
        {
            ClientLogTextBox.Text += TcpClientLog.Log[^1];
        }
        private async void ClientClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            ClientLogTextBox.Text = "";
            await TcpClientLog.ResetAsync();
        }
        #endregion

        #endregion
    }
}
