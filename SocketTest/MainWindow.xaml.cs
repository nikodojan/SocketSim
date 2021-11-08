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

namespace SocketTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        


        public MainWindow()
        {
            InitializeComponent();
            serverIntroTextBlock.Text = "Use this window to start a TCP server, which is listening to the specified IP address and port.";
            serverIpTextBox.Text = "127.0.0.1";
            serverPortTextBox.Text = "4646";
        }

    }
}
