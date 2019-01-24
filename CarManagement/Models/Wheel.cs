using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    class Wheel
    {
        int diametro;
        int ancho;

        public Wheel()
        {
            this.diametro = 0;
            this.ancho = 0;
        }
        public Wheel(int diametro,int ancho)
        {
            this.diametro = diametro;
            this.ancho = ancho;
        }
        
    }
}
}
