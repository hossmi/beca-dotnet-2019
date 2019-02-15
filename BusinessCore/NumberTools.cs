using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore
{
    public class NumberTools
    {

        public static long factorial(long number)
        {
            if (number == 0)
            {
                return 1;
            }
            return number * factorial(number - 1);

        }

        public static int fibonacci(int number)
        {
            int a = 0;
            int b = 1;
            for (int i = 0; i < number; i++)
            {
                int temp = a;
                a = b;
                b = temp + b;
            }
            return a;
        }
    }
}

