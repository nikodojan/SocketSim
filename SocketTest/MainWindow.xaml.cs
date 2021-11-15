using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using SocketTest.Models;
using SocketTest.StaticLogs;
using SocketTest.TcpServerFiles;

namespace SocketTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string StartServerButtonLabel = "Start server";
        private const string StopServerButtonLabel = "Stop server";

        private TextBlock serverLogTextBox;

        private SimpleTcpServer _server;
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeControls();
        }

        private void InitializeControls()
        {
            // default text
            serverIpTextBox.Text = "127.0.0.1";
            serverPortTextBox.Text = "4646";

            serverStartButton.Content = StartServerButtonLabel;

            serverMessageTextBox.IsEnabled = false;

            // click event handler
            serverStartButton.Click += StartServerButton_Click;

            serverLogTextBox = new TextBlock();
            serverLogTextBox.FontSize = 12;
            serverLogTextBox.Background = Brushes.White;
            serverLogTextBox.TextWrapping = TextWrapping.Wrap;

            serverLogScrollViewer.Content = serverLogTextBox;
            serverLogScrollViewer.Height = 150;
            serverLogScrollViewer.Background = Brushes.White;

            serverClearLogButton.Click += OnClearLogButton_Click;

        }

        #region GUI event handler
        private async void StartServerButton_Click(object sender, RoutedEventArgs e)
        {
            if (serverStartButton.Content.ToString() == StartServerButtonLabel)
            {
                OnStartServerButtonClick();
                serverStartButton.Content = StopServerButtonLabel;
            }
            else if (serverStartButton.Content.ToString() == StopServerButtonLabel)
            {
                OnStopServerButtonClick();
                serverStartButton.Content = StartServerButtonLabel;
            }
        }

        public async void OnStartServerButtonClick()
        {
            IPAddress ip = null;
            bool ipParsed = IPAddress.TryParse(serverIpTextBox.Text, out ip);
            if (!ipParsed)
            {
                serverLogTextBox.Text = "IP Address has invalid format";
            }

            //int port;
            bool portParsed = Int32.TryParse(serverPortTextBox.Text, out int port);
            if (!portParsed)
            {
                serverLogTextBox.Text = "Port has invalid format";
            }

            bool portIsValid = true;
            if (port < 0 || port > 65535)
            {
                serverLogTextBox.Text = "Invalid port number";
                portIsValid = false;
            }

            if (ipParsed && portParsed && portIsValid)
            {
                Endpoint ep = new Endpoint(serverIpTextBox.Text, port);
                await StartTcpServer(ep);
            }
            
        }

        public async void OnStopServerButtonClick()
        {
            await StopTcpServer();
        }

        private void SwitchControlsOnStart(object sender, EventArgs e)
        {
            serverIpTextBox.IsEnabled = false;
            serverPortTextBox.IsEnabled = false;

            serverMessageTextBox.IsEnabled = true;
        }

        private void SwitchControlsOnStop(object sender, EventArgs e)
        {
            serverIpTextBox.IsEnabled = true;
            serverPortTextBox.IsEnabled = true;

            serverMessageTextBox.IsEnabled = false;
        }

        private void OnServerLogChanged(object sender, EventArgs e)
        {
            serverLogTextBox.Text += TcpServerLog.Log[^1];
        }

        private void OnClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            serverLogTextBox.Text = "";
        } 
        #endregion

        /// <summary>
        /// Cerates a new TCP server instance, subscribes to the it's events and starts listener.
        /// </summary>
        /// <param name="ep"></param>
        /// <returns></returns>
        public async Task StartTcpServer(Endpoint ep)
        {
            _server = new SimpleTcpServer(ep);
            _server.LogChanged += OnServerLogChanged;
            _server.ServerStarted += SwitchControlsOnStart;
            _server.ServerStopped += SwitchControlsOnStop;
            await _server.StartListener();

        }

        /// <summary>
        /// Stops the Tcp Server.
        /// </summary>
        public async Task StopTcpServer()
        {
            await _server.StopListener();
        }

    }
}
