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
using System.Windows.Shapes;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Progetto_Socket
{
    /// <summary>
    /// Logica di interazione per Versione2.xaml
    /// </summary>
    public partial class Versione2 : Window
    {
        List<Contatto> Rubrica;
        int indiceDestinatario = -1;
        Socket socket;
        object semaforoModificaRubrica = new object();
        public Versione2()
        {
            try
            {
                InitializeComponent();
                InizializzazioneComunicazioni();
                LeggiRubrica();
                AggiornaListaContatti();

                Thread ricevitore = new Thread(new ThreadStart(Ricezione));
                ricevitore.Start();

            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //inizializzazione della socket
        private void InizializzazioneComunicazioni()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress local = IPAddress.Any;
                IPEndPoint local_endpoint = new IPEndPoint(local.MapToIPv4(), 65000);
                socket.Bind(local_endpoint);
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        //Legge e memorizza tutti i contatti e relativi messaggi
        private void LeggiRubrica()
        {
            try
            {
                using (StreamReader sr = new StreamReader("backup.txt"))
                {
                    Rubrica = new List<Contatto>();
                    string riga;
                    while ((riga = sr.ReadLine()) != null)
                    {
                        string nome = "";
                        string ip = "";
                        int porta = 0;
                        List<Messaggio> lista = new List<Messaggio>();
                        string[] lettura = riga.Split('¢');
                        for (int i = 0; i < lettura.Length; i++)
                        {
                            if (i == 0)
                            {
                                nome = lettura[i];
                            }
                            else if (i == 1)
                            {
                                ip = lettura[i];
                            }
                            else if (i == 2)
                            {
                                porta = int.Parse(lettura[i]);
                            }
                            else
                            {
                                string[] messaggio = lettura[i].Split('□');
                                Messaggio messaggioAppoggio = new Messaggio(messaggio[0], messaggio[1]);
                                lista.Add(messaggioAppoggio);
                            }
                        }
                        Contatto nuovo = new Contatto(nome, ip, porta, lista);
                        Rubrica.Add(nuovo);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //aggiorna la listbox con i contatti
        private void AggiornaListaContatti()
        {
            lock (semaforoModificaRubrica)
            {
                lstContatti.Items.Clear();
                foreach (Contatto c in Rubrica)
                {
                    lstContatti.Items.Add(c.ToList());
                }
            }
        }

        //salva tutti i contatti e relativi messaggi
        private void SalvaRubrica()
        {
            try
            {
                lock (semaforoModificaRubrica)
                {
                    using (StreamWriter sw = new StreamWriter("backup.txt"))
                    {
                        for (int i = 0; i < Rubrica.Count; i++)
                        {
                            sw.WriteLine(Rubrica[i].ToString());
                        }
                    }
                }
            }catch (Exception ex)
            {
                throw ex;
            }
        }
        //aggiunge un contatto da input
        private void AggiungiContatto()
        {
            try
            {
                string nome = txtNome.Text;
                string ip = txtIP.Text;
                int port = int.Parse(txtPorta.Text);
                Contatto c = new Contatto(nome, ip, port);
                lock (semaforoModificaRubrica)
                {
                    Rubrica.Add(c);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AggiungiContatto();
                AggiornaListaContatti();
                SalvaRubrica();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDlt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CancellaContatto(lstContatti.SelectedIndex);
                AggiornaListaContatti();
                SalvaRubrica();

            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //cancella contatto selezionato
        private void CancellaContatto(int i)
        {
            try
            {
                lock (semaforoModificaRubrica)
                {
                    lstContatti.SelectedIndex = -1;
                    Rubrica.RemoveAt(i);
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        private void lstContatti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstContatti.SelectedIndex != -1)
                {
                    CambiaDestinatario();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //quando viene selezionato un contatto ne stampa la chat
        private void CambiaDestinatario()
        {
            try
            {
                lock (semaforoModificaRubrica)
                {
                    indiceDestinatario = lstContatti.SelectedIndex;
                    lstMex.Items.Clear();
                    lblDest.Content = "Stai parlando con " + Rubrica[indiceDestinatario].ToList();
                    foreach (Messaggio m in Rubrica[indiceDestinatario].GetChat())
                    {
                        lstMex.Items.Add(m.ToList());
                    }
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        private void btnDltChat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CancellaChat();
                SalvaRubrica();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //pulizia chat relativa al destinatario in questione
        private void CancellaChat()
        {
            try
            {
                if(indiceDestinatario != -1)
                {
                    lock (semaforoModificaRubrica)
                    {
                        Rubrica[indiceDestinatario].CancellaChat();
                        lstMex.Items.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InviaMessaggio(txtMex.Text);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //invio dei messaggi
        private void InviaMessaggio(string messaggio)
        {
            try
            {
                if(indiceDestinatario != -1)
                {
                    IPAddress remote = IPAddress.Parse(Rubrica[indiceDestinatario].GetIP());
                    IPEndPoint remote_endpoint = new IPEndPoint(remote, Rubrica[indiceDestinatario].GetPort());
                    byte[] mex = Encoding.UTF8.GetBytes(messaggio);
                    lock (semaforoModificaRubrica)
                    {
                        socket.SendTo(mex, remote_endpoint);
                        lstMex.Items.Add("Me: " + Encoding.Default.GetString(mex));
                        Messaggio m = new Messaggio("Me", messaggio);
                        Rubrica[indiceDestinatario].AggiungiMessaggio(m);
                        SalvaRubrica();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        //metodo per il thread di ricezione
        private void Ricezione()
        {
            try
            {
                while (true)
                {
                    int nBytes;
                    if ((nBytes = socket.Available) > 0)
                    {
                        byte[] buffer = new byte[nBytes];
                        EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        nBytes = socket.ReceiveFrom(buffer, ref remoteEndPoint);
                        string from = ((IPEndPoint)remoteEndPoint).Address.ToString();
                        string messaggio = Encoding.UTF8.GetString(buffer, 0, nBytes);
                        for (int i = 0; i < Rubrica.Count; i++)
                        {
                            if (Rubrica[i].GetIP() == from)
                            {
                                lock (semaforoModificaRubrica)
                                {
                                    Messaggio m = new Messaggio(from, messaggio);
                                    Rubrica[i].AggiungiMessaggio(m);
                                    if (i == indiceDestinatario)
                                    {
                                        this.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            lstMex.Items.Add(m.ToList());
                                        }));
                                    }
                                }
                                break;
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
