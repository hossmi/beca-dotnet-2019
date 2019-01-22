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
            long lResult = 1;

            if (n > 0)
            {
                long i;
                for (i = n; i > 0; i--)
                {
                    lResult = lResult * i;
                }
            }
            else if (n == 0)
            {
                lResult = 1;
            }
            else if (n < 0)
            {
                throw new ArgumentException("El numero introducido debe ser positivo");
            }

            return (lResult);
            
        }

        public static int fibonacci(int n)
        {
            int iResult = 0;

            if (n > 1)
            {
                iResult = fibonacci(n - 1) + fibonacci(n - 2);
            }
            else if(n==1)
            {
                iResult = 1;
            }
            else if (n==0)
            {
                iResult = 0;
            }
            else if (n < 0)
            {
                throw new ArgumentException("El numero introducido debe ser positivo");
            }

            return (iResult);

        }
    }
}
