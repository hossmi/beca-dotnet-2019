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
            long resultado = 0;
            for (long i = 1; i <= n; i++)
            {
                resultado = resultado * i +i;
            }
            return (resultado);
        }

        public static long fibonacci(int n)
        {

            long resultado = 0;
            for(int i = 0; i<n; i++)
            {
                if (n == 0)
                {
                    return (resultado);
                }
                else if (n == 1)
                {
                    resultado = 1;
                    return (resultado);
                }
                else
                {
                    return fibonacci(n - 1) + fibonacci(n - 2);
                }
            }
            return (resultado);
        }
    }
}
