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
using SocketTest.Resources;
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

        /// <summary>
        /// Initializes the UI controls.
        /// </summary>
        private void InitializeControls()
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

            fileMenuExitButton.Click += (object sender, RoutedEventArgs e) => Application.Current.Shutdown();

            serverMessageTextBox.Text = "";
            serverSendMessageButton.Click += ServerSendMessageButton_Click;
            serverDeleteMessageButton.Click += ServerDeleteMessageButton_Click;

            
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

        #region GUI event handler
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
            IPAddress ip = null;
            bool ipParsed = IPAddress.TryParse(serverIpTextBox.Text, out ip);
            if (!ipParsed)
            {
                serverLogTextBox.Text = "IP Address has invalid format\r\n";
            }

            //int port;
            bool portParsed = Int32.TryParse(serverPortTextBox.Text, out int port);
            if (!portParsed)
            {
                serverLogTextBox.Text = "Port has invalid format\r\n";
            }

            bool portIsValid = true;
            if (port < 0 || port > 65535)
            {
                serverLogTextBox.Text = "Invalid port number\r\n";
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

        /// <summary>
        /// Disables IP and port inputs when the server is started.
        /// Enables the server message input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchControlsOnStart(object sender, EventArgs e)
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
        private void SwitchControlsOnStop(object sender, EventArgs e)
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
        }

        private void OnClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            serverLogTextBox.Text = "";
        } 

        
        #endregion

        /// <summary>
        /// Creates a new TCP server instance, subscribes to the it's events and starts listener.
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
