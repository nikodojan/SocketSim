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
using SocketTest.Sockets;
using SocketTest.StaticLogs;

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
        private TextBlock UdpLogTextBox;

        private SimpleTcpServer _server;
        private SimpleUdpClient _udpClient;
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeTcpServerTab();
            InitializeUdpTab();
        }
        
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

            fileMenuExitButton.Click += (object sender, RoutedEventArgs e) => Application.Current.Shutdown();

            serverMessageTextBox.Text = "";
            serverSendMessageButton.Click += ServerSendMessageButton_Click;
            serverDeleteMessageButton.Click += ServerDeleteMessageButton_Click;
        }

        /// <summary>
        /// Creates a new TCP server instance, subscribes to it's events and starts listener.
        /// </summary>
        /// <param name="ep"></param>
        /// <returns></returns>
        public async Task StartTcpServer(IPEndPoint ep)
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
            var output = ParseEndpoint(serverIpTextBox.Text, serverPortTextBox.Text);
            await StartTcpServer(output);
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

        #region TCP CLIENT TAB : Contains all TCP client related methods
        /// <summary>
        /// Initializes the UI controls in the TCP client tab.
        /// </summary>
        private void InitializeTcpClientTab()
        {

        }
        #endregion

        #region UDP TAB: Contains all UDP related methods

        /// <summary>
        /// Initializes the UI controls in the UDP tab.
        /// </summary>
        private void InitializeUdpTab()
        {
            _udpClient = new SimpleUdpClient();

            UdpDestinationIpTextBox.Text = "255.255.255.255";
            UdpDestinationPortTextBox.Text = "65535";
            UdpMessageTextBox.Text = "";

            // log text block and scroll viewer
            UdpLogTextBox = new TextBlock();
            UdpLogTextBox.FontSize = 12;
            UdpLogTextBox.Background = Brushes.White;
            UdpLogTextBox.TextWrapping = TextWrapping.Wrap;

            UdpLogScrollViewer.Content = serverLogTextBox;
            UdpLogScrollViewer.Height = 150;
            UdpLogScrollViewer.Background = Brushes.White;

            //event handler
            UdpSendMessageButton.Click += UdpSendMessageButton_Click;
            UdpDeleteMessageButton.Click += (object sender, RoutedEventArgs e) => { UdpMessageTextBox.Text = ""; };
            _udpClient.LogChanged += OnUdpLogChanged;
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
            var endPoint = ParseEndpoint(UdpDestinationIpTextBox.Text, UdpDestinationPortTextBox.Text);
            if (endPoint != null)
                await _udpClient.SendAsync(endPoint, UdpMessageTextBox.Text);
        }

        private void OnUdpLogChanged(object sender, EventArgs e)
        {
            UdpLogTextBox.Text += UdpLog.Log[^1];
        }


        #endregion

        #endregion




        /// <summary>
        /// Parses the entered IP address and port to IPEndPoint.
        /// </summary>
        /// <param name="ipInput">IP addresses to be parsed</param>
        /// <param name="portInput">Port number to be parsed</param>
        /// <returns>An instance of an IPEndPoint with the entered parameters or <see langword="null"/> if the entered values are invalid. </returns>
        private IPEndPoint ParseEndpoint(string ipInput, string portInput)
        {
            bool ipParsed = IPAddress.TryParse(serverIpTextBox.Text, out IPAddress ip);
            if (!ipParsed)
                MessageBox.Show("IP Address has invalid format.", "Input error");


            bool portParsed = Int32.TryParse(serverPortTextBox.Text, out int port);
            if (!portParsed)
                MessageBox.Show("Port has invalid format.", "Input error");

            bool portIsValid = true;
            if (port < 0 || port > 65535)
            {
                MessageBox.Show("Invalid port number.\r\n" +
                                "The port number must be between 0 and 65535", "Input error");
                portIsValid = false;
            }

            if (ipParsed && portParsed && portIsValid)
            {
                //Endpoint ep = new Endpoint(serverIpTextBox.Text, port);
                IPEndPoint endPoint = new IPEndPoint(ip, port);
                return endPoint;
            }
            return null;
        }
    }
}
