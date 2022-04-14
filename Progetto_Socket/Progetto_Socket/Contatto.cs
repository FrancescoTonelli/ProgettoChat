using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Progetto_Socket
{
    internal class Contatto
    {
        private string _nome;
        private string _ip;
        private int _port;
        private List<Messaggio> _chat;
        public Contatto()
        {
            _nome = "";
            _ip = "";
            _port = 0;
            _chat = new List<Messaggio>();
        }

        public Contatto(string nome, string ip, int port)
        {
            try
            {
                _nome = nome;
                _ip = ip;
                _port = port;
                _chat = new List<Messaggio>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Contatto(string nome, string ip, int port, List<Messaggio> chat)
        {
            try
            {
                _nome = nome;
                _ip = ip;
                _port = port;
                _chat = chat;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public override string ToString()
        {
            string appoggioChat = "";
            for(int i = 0; i < _chat.Count; i++)
            {
                if (i == 0)
                {
                    appoggioChat += "¢";
                }
                appoggioChat += _chat[i].ToString();
                if(i != _chat.Count - 1)
                {
                    appoggioChat += "¢";
                }
            }
            return (_nome + "¢" + _ip + "¢" + _port + appoggioChat);
        }

        public string ToList()
        {
            return _nome;
        }

        public string GetIP()
        {
            return _ip;
        }

        public int GetPort()
        {
            return _port;
        }

        public List<Messaggio> GetChat()
        {
            return _chat;
        }

        public void AggiungiMessaggio(Messaggio m)
        {
            _chat.Add(m);
        }

        public void CancellaChat()
        {
            _chat.Clear();
        }
    }
}
