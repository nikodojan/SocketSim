using System;
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
        private const string StartServerButtonLabel = "Start server";
        private const string StopServerButtonLabel = "Stop server";
        private const string StartUdpListenerButtonLabel = "Start listening";
        private const string StopUdpListenerButtonLabel = "Stop listening";

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
            _server.ServerStarted += SwitchServerControlsOnStart;
            _server.ServerStopped += SwitchServerControlsOnStop;
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
            var parsedEndpoint = ParsingHelper.ParseEndpoint(serverIpTextBox.Text, serverPortTextBox.Text);
            await StartTcpServer(parsedEndpoint);
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
