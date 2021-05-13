using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema
{
    class Usuario
    { 
        public string contrasenia { get; set; }

        public Usuario(string contrasenia)
        {
            this.contrasenia = contrasenia;
        }

    }
}
