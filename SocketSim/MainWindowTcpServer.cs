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
    /// Partial class that handles the TCP server tab in the UI.
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string StartServerButtonLabel = "Start server";
        private const string StopServerButtonLabel = "Stop server";

        private TextBlock serverLogTextBox;
        private SimpleTcpServer _server;

        #region TCP SERVER TAB : Contains all TCP server related methods

        /// <summary>
        /// Initializes the UI controls in the TCP server tab.
        /// </summary>
        private void InitializeTcpServerTab()
        {
            // default text
            serverIpTextBox.Text = "127.0.0.1";
            serverPortTextBox.Text = "4646";

            serverStartButton.Content = StartServerButtonLabel;
            serverMessageTextBox.IsEnabled = false;

            // click event handler
            serverStartButton.Click += StartServerButton_Click;
            serverClearLogButton.Click += OnClearLogButton_Click;

            // log text block and scroll viewer
            serverLogTextBox = new TextBlock();
            serverLogTextBox.FontSize = 12;
            serverLogTextBox.Background = Brushes.White;
            serverLogTextBox.TextWrapping = TextWrapping.Wrap;

            serverLogScrollViewer.Content = serverLogTextBox;
            serverLogScrollViewer.Height = 150;
            serverLogScrollViewer.Background = Brushes.White;

            

            serverMessageTextBox.Text = "";
            serverSendMessageButton.Click += ServerSendMessageButton_Click;
            serverDeleteMessageButton.Click += ServerDeleteMessageButton_Click;
        }



        /// <summary>
        /// Stops the Tcp Server.
        /// </summary>
        public async Task StopTcpServer()
        {
            await _server.StopListener();
        }

        #region TCP Server event handler
        /// <summary>
        /// Click event handler for the "Start server" / "Stop server" button.
        /// The called method depends on whether the server is running or not.
        /// </summary>
        private async void StartServerButton_Click(object sender, RoutedEventArgs e)
        {
            if (serverStartButton.Content.ToString() == StartServerButtonLabel)
            {
                OnStartServerButtonClick();
            }
            else if (serverStartButton.Content.ToString() == StopServerButtonLabel)
            {
                OnStopServerButtonClick();
            }
        }

        /// <summary>
        /// This method is called when the server is not running and the start button is pressed.
        /// It checks if the entered IP address and port have a valid format, creates the endpoint and calls the StartTcpServer method.
        /// </summary>
        public async void OnStartServerButtonClick()
        {
            try
            {
                var parsedEndpoint = ParsingHelper.TryParseEndpoint(serverIpTextBox.Text, serverPortTextBox.Text);
                await StartTcpServer(parsedEndpoint);
            }
            catch (EndPointParserException e)
            {
                MessageBox.Show(e.Message, "Address error");
            }
        }

        /// <summary>
        /// Creates a new TCP server instance, subscribes to it's events and starts listener.
        /// </summary>
        /// <param name="ep"></param>
        /// <returns></returns>
        public async Task StartTcpServer(IPEndPoint ep)
        {
            var echo = ServerEchoCheckbox.IsChecked ?? false;
            _server = new SimpleTcpServer(ep, echo);
            _server.LogChanged += OnServerLogChanged;
            _server.ServerStarted += SwitchServerControlsOnStart;
            _server.ServerStopped += SwitchServerControlsOnStop;
            await _server.StartListener();
        }

        public async void OnStopServerButtonClick()
        {
            await StopTcpServer();
        }

        /// <summary>
        /// Disables IP and port inputs when the server is started.
        /// Enables the server message input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchServerControlsOnStart(object sender, EventArgs e)
        {
            serverIpTextBox.IsEnabled = false;
            serverPortTextBox.IsEnabled = false;
            serverMessageTextBox.IsEnabled = true;
            serverStartButton.Content = StopServerButtonLabel;
        }

        /// <summary>
        /// Enables the IP and port inputs when the server is stopped.
        /// Disables the server message input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchServerControlsOnStop(object sender, EventArgs e)
        {
            serverIpTextBox.IsEnabled = true;
            serverPortTextBox.IsEnabled = true;
            serverMessageTextBox.IsEnabled = false;
            serverStartButton.Content = StartServerButtonLabel;
        }

        /// <summary>
        /// Gets the last record from the server log and adds it to the server log output in the UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnServerLogChanged(object sender, EventArgs e)
        {
            serverLogTextBox.Text += TcpServerLog.Log[^1];
            serverLogScrollViewer.ScrollToBottom();
        }

        private void OnClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            serverLogTextBox.Text = "";
        }

        private void ServerDeleteMessageButton_Click(object sender, RoutedEventArgs e)
        {
            serverMessageTextBox.Text = "";
        }

        private async void ServerSendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            await _server.SendMessage(serverMessageTextBox.Text);
            serverMessageTextBox.Text = "";
        }

        #endregion


        #endregion
    }
}
