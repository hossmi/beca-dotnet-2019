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
            int resul = n;            

            for (int i = n - 1; i > 1; i--)
            {
                resul *= i;
            }
            return resul;
        }

        public static int fibonacci(int n)
        {
            int resul = 0;

            if (n > 1)
                resul = fibonacci(n - 1) + fibonacci(n - 2);
            else if (n == 1)
                resul = 1;
            else if (n == 0)
                resul = 0;

            return resul;
        }
    }
}
