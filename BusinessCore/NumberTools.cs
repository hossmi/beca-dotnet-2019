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
            int[] arr = new int[n + 2];
            arr[0] = 0;
            arr[1] = 1;
            for (int i = 2; i <= n; i++)
            {
                arr[i] = arr[i - 1] + arr[i - 2];
            }
            return arr[n];
        }
    }
}
