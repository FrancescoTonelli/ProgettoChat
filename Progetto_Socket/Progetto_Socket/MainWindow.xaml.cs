using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using System.Windows.Threading;

namespace Progetto_Socket
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Socket calza;
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                calza = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPAddress local = IPAddress.Any;
                IPEndPoint local_endpoint = new IPEndPoint(local.MapToIPv4(), 65000);

                lblRx.Content = "Ricezione messaggi attiva";

                calza.Bind(endpoint);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPAddress remote = IPAddress.Parse(txtIP.Text);

                IPEndPoint remote_endpoint = new IPEndPoint(remote, int.Parse(txtPorta.Text));

                byte[] mex = Encoding.UTF8.GetBytes(txtMex.Text);

                calza.SendTo(mex, remote_endpoint);

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
