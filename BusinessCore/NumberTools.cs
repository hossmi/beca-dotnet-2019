using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core;

namespace BusinessCore
{
    public class NumberTools
    {
        public static long factorial(long n)
        {
            if (n == 0)
                return 0;

            long factorial = 1;

            for (long i = 2; i <= n; i++)
                factorial *= i;

            return factorial;
        }

        public static int fibonacci(int n)
        {
            if (n == 0)
                return 0;

            if (n == 1)
                return 1;

            return fibonacci(n - 1) + fibonacci(n - 2);
        }
    }
}
