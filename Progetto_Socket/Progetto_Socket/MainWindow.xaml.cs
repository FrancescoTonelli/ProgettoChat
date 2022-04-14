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
        //creo l'oggetto socket
        Socket socket;
        //creo un timer per la recezione dei messaggi
        DispatcherTimer dTimer;
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                //inizializzo l'oggetto socket specificando il tipo di indirizzi utilizzato (IP), il tipo di socket (datagram)
                //e il tipo di protocollo (UDP)
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                //creo l'oggetto che ospita l'indirizzo IP locale
                IPAddress local = IPAddress.Any;

                //creo un endopint associando l'indirizzo locale alla porta da usare per la ricezione
                IPEndPoint local_endpoint = new IPEndPoint(local.MapToIPv4(), 65000);

                //associo l'endpoint alla socket
                socket.Bind(local_endpoint);


                //inizializzo l'oggetto timer
                dTimer = new DispatcherTimer();
                //associo un evento al tick del timer
                dTimer.Tick += new EventHandler(aggiornamento);
                //imposto un intervallo tra i tick del timer (250 ms)
                dTimer.Interval += new TimeSpan(0, 0, 0, 0, 250);
                //avvio il timer
                dTimer.Start();


                //stampo sull'interfaccia che il programma è pronto a ricevere messaggi
                lblRx.Content = "Ricezione messaggi attiva";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //inizializzo l'indirizzo IP da raggiungere in base a quanto inserito dall'utente
                IPAddress remote = IPAddress.Parse(txtIP.Text);

                //inizializzo l'endpoint da raggiungere associando l'indirizzo appena creato e la porta da raggiungere specificata
                //dal mittente
                IPEndPoint remote_endpoint = new IPEndPoint(remote, int.Parse(txtPorta.Text));

                //ricavo il messaggio da inviare codificando la stringa scritta dall'utente e convertendola
                //ad un array di byte
                byte[] mex = Encoding.UTF8.GetBytes(txtMex.Text);

                //invio l'array messaggio all'endpoint ricevitore, tramite l'oggetto socket
                socket.SendTo(mex, remote_endpoint);

                //aggiungo nella listbox anche i messaggi che scrivo io stesso
                lstBox.Items.Add("Me: " + Encoding.Default.GetString(mex));

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void aggiornamento(object sender, EventArgs e)
        {
            try
            {
                //definisco una variabile int per usarla nell'if come variabile d'appoggio
                int nBytes;
                //se ci sono socket in attesa
                if ((nBytes = socket.Available) > 0)
                {
                    //inizializzo un array di byte che userò come buffer
                    byte[] buffer = new byte[nBytes];
                    //inizializzo un oggetto EndPoint da usare dopo
                    EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    //aggiorno in nBytes il numero di bytes ricevuti dalla socket, il messaggio ricevuto in buffer e l'endpoint
                    //del mittente
                    nBytes = socket.ReceiveFrom(buffer, ref remoteEndPoint);
                    //converto l'indirizzo IP di arrivo in un oggetto string
                    string from = ((IPEndPoint)remoteEndPoint).Address.ToString();
                    //decodifico in una stringa il messa
                    string messaggio = Encoding.UTF8.GetString(buffer, 0, nBytes);

                    //aggiungo nella listbox il messaggio appena ricevuto
                    lstBox.Items.Add(from + ": " + messaggio);
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //pulisco la listbox con la chat
            lstBox.Items.Clear();
        }
    }
}
