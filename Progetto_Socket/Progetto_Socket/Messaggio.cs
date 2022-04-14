using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Progetto_Socket
{
    internal class Messaggio
    {
        public Messaggio()
        {

        }

        public Messaggio(string mittente, string testo)
        {
            _mittente = mittente;
            _testo = testo;
        }

        public override string ToString()
        {
            return (_mittente + "□" + _testo);
        }

        public string ToList()
        {
            return _mittente + ": " + _testo;
        }

        private string _mittente;
        private string _testo;


    }
}
