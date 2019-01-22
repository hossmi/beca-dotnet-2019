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
            long fact = 1;

            for(int i= 0; i < n; i++)
            {
                fact = fact * i;
            };

            return fact;
        }

        public static int fibonacci(int n)
        {
            int fib = 0;
            int oldFib = 1;
            int temp;

            for (int i = 0; i < n; i++)
            {
                temp = fib;

                fib = oldFib + fib;
                oldFib = temp;
            };

            return fib;
        }
    }
}
