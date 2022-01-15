using System;
using System.Collections.Generic;
using System.Linq;
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
            ClientIpTextBox.Text = "127.0.0.0";
            ClientPortTextBox.Text = "4646";

            // log text block and scroll viewer
            ClientLogTextBox = new TextBlock();
            ClientLogTextBox.FontSize = 12;
            ClientLogTextBox.Background = Brushes.White;
            ClientLogTextBox.TextWrapping = TextWrapping.Wrap;

            ClientLogScrollViewer.Content = serverLogTextBox;
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

        private void ClientDeleteMessageButton_Click(object sender, RoutedEventArgs e)
        {
            ClientMessageTextBox.Text = "";
        }

        private void ClientSendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        private async void ClientClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            ClientLogTextBox.Text = "";
            await TcpClientLog.ResetAsync();
        }

        private void ClientConnectButton_Click(object sender, RoutedEventArgs e)
        {
            switch (ClientConnectButton.Content)
            {
                case "Connect":
                    Connect();
                    
                    break;
                case "Disconnect":
                    SwitchClientControls_OnDisconnect(this, EventArgs.Empty);
                    break;
            };
        }

        private void SwitchClientControls_OnConnect(object sender, EventArgs e)
        {
            ClientConnectButton.Content = ClientDisconnectButtonLabel;
            ClientIpTextBox.IsEnabled = false;
            ClientPortTextBox.IsEnabled = false;

            ClientMessageTextBox.IsEnabled = true;
            ClientSendMessageButton.IsEnabled = true;
            ClientDeleteMessageButton.IsEnabled = true;
        }

        private void SwitchClientControls_OnDisconnect(object sender, EventArgs e)
        {
            ClientConnectButton.Content = ClientConnectButtonLabel;
            ClientIpTextBox.IsEnabled = true;
            ClientPortTextBox.IsEnabled = true;

            ClientMessageTextBox.IsEnabled = false;
            ClientSendMessageButton.IsEnabled = false;
            ClientDeleteMessageButton.IsEnabled = false;
        }

        public async Task Connect()
        {
            try
            {
                var endPoint = ParsingHelper.TryParseEndpoint(ClientIpTextBox.Text, ClientPortTextBox.Text);
                SwitchClientControls_OnConnect(this, EventArgs.Empty);
            }
            catch (EndPointParserException e)
            {
                MessageBox.Show(e.Message, "Address error");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            _client = new SimpleTcpClient();

        }
    }
}
