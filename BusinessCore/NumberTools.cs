using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore
{
    public class NumberTools
    {
        public static int factorial(int n)
        {


            throw new NotImplementedException();
        }

        public static long fibonacci(int n)
        {

            long resultado = 0;
            int aux1 = 0;

            for(int i = 0; i < n; i++)
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
                    resultado = resultado + n-1;
                }
            }
            return (resultado);




            throw new NotImplementedException();
        }
    }
}
