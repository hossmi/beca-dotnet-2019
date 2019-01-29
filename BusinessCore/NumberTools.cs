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
            return resultado;
        }

        public static int fibonacci(int n)
        {
            int num1 = 0;
            int num2 = 1;
            for (int i = 0; i < n; i++)
            {
                int temp = num1;
                num1 = num2;
                num2 = temp + num2;
            }
            return num1;
        }
}
}
