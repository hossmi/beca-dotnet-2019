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
            long nfactorial = 1;

            for (int i = 1; i<=n; i++)
            {
                nfactorial = nfactorial * i;
            }

            return nfactorial;
        }

        public static int fibonacci(int n)
        {
            int nfibonacci =  n;

            if (n != 0 && n!=1)
            {
                nfibonacci = fibonacci(n - 1) + fibonacci(n - 2);
            }
            else
            {
                if (n == 0)
                { nfibonacci = 0; }
                else
                { nfibonacci = 1; }
            }
            return nfibonacci;
        }
    }
}
