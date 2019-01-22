﻿using System;
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
            long i = 1; long o = n;
            long a = o;
            while (o >= i)
            {
                long b = o-1;
                long c = a * b;
                a = c;
                o--;
            }
            return a;
        }
        public static int fibonacci(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }
            else
            {
                int a = 0;
                int i = 1;
                while (n < i)
                {
                    int b = a + 1;
                    a = b;
                    i++;
                }
                return a;

            }
        }
    }
}
