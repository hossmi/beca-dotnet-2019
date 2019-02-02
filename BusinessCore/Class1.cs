using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore
{
    public class Numeros
    {
        public class Uno
        {
            int numero = 1;

            public int getNumero()
            {
                return this.numero;
            }


        }

        Uno uno;//declarar
        Uno uno2 = new Uno();//instanciar
        int numero = 2; //inicializar
        public int GetUno()
        {
            return uno2.getNumero();
        }
        private class Dos
        {
            int numero = 2;
        }
    }
}
