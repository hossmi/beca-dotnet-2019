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
            int result = 1;
            int i;

            for (i = n; i > 0; i--)
            {
                result = result * i;
            }

            return (result);

            throw new NotImplementedException();
        }

        public static int fibonacci(int n)
        {
            int result = 0;

            if (n == 0)
            {
                result = 0;
            }
            else if(n==1)
            {
                result = 1;
            }
            else
            {
                result = fibonacci(n - 1) + fibonacci(n - 2);
            }
            
            return (result);

            throw new NotImplementedException();
        }
    }
}
