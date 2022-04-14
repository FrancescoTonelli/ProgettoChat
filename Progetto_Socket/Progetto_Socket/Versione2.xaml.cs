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

namespace Progetto_Socket
{
    /// <summary>
    /// Logica di interazione per Versione2.xaml
    /// </summary>
    public partial class Versione2 : Window
    {
        List<Contatto> Rubrica;
        int indiceInterlocutore;
        public Versione2()
        {
            try
            {
                InitializeComponent();
                LeggiRubrica();
                AggiornaListaContatti();
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                        List<Messaggio> lista;
                        string[] lettura = riga.Split('¢');
                        for (int i = 0; i < lettura.Length; i++)
                        {
                            lista = new List<Messaggio>();
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
                        Contatto nuovo = new Contatto();
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
            lstContatti.Items.Clear();
            foreach(Contatto c in Rubrica)
            {
                lstContatti.Items.Add(c.ToList());
            }
        }

        //salva tutti i contatti e relativi messaggi
        private void SalvaRubrica()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("backup.txt"))
                {
                    for (int i = 0; i < Rubrica.Count; i++)
                    {
                        sw.WriteLine(Rubrica[i].ToString());
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
                Rubrica.Add(c);
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

        private void CancellaContatto(int i)
        {
            try
            {
                lstContatti.SelectedIndex = -1;
                Rubrica.RemoveAt(i);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        private void lstContatti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CambiaDestinatario();
        }

        private void CambiaDestinatario()
        {

        }
    }
}
