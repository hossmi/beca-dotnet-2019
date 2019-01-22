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
            long result = 1;
            
            if (n == 0)
            {
                result = 1;
            }
            else
            {
                long i;
                for (i = n; i > 0; i--)
                {
                    result = result * i;
                }
            }

            return (result);

            
        }

        public static int fibonacci(int n)
        {
            int result = 0;

            if (n > 1)
            {
                result = fibonacci(n - 1) + fibonacci(n - 2);
            }
            else if(n==1)
            {
                result = 1;
            }
            else if (n==0)
            {
                result = 0;
            }
            
            return (result);

            
        }
    }
}
