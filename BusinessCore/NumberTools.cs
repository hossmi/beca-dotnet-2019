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
            for (long b = n - 1, c = n, a = 0; a < b; a++, c--)
            {
                n = n * (c - 1);
            }
            return n;
        }
        public static int fibonacci(int n)
        {
            int c = 0;
            for (int a = 0, b = 1, i = 0; i < n; i++)
            {
                c = a + b;
                a = b;
                b = c;
            }
            return c;
        }
    }
}
