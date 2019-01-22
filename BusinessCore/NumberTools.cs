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
            //throw new NotImplementedException();
            if (n == 0)
            {
                return 1;
            }
            else if(n == 1)
            {
                return 1;
            }
            return n * factorial(n - 1);
        }

        public static int fibonacci(int n)
        {
            //throw new NotImplementedException();
            if(n == 0)
            {
                return 0;
            }
            else if(n == 1)
            {
                return 1;
            }
            return fibonacci(n -1) + fibonacci(n - 2);
        }
    }
}
