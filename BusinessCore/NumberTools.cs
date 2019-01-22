using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore
{
    public class NumberTools
    {
        public static long factorial(long n)
        {
            long resultado = 1;
            for (long i = 1; i <= n; i++)
            {
                resultado = resultado * i;
            }
            return (resultado);
        }

        public static long fibonacci(int n)
        {

            int aux1 =0;
            int aux2 = 1;
            for(int i = 0; i<n; i++)
            {
                int temporal = aux1;
                aux1 = aux2;
                aux2 = temporal + aux2;
            }
            return (aux1);
        }
    }
}
