using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore
{
    public class NumberTools
    {
        public static int factorial(int n)
        {
            int res = n;
            for (int var = n -1;var >1; var--)
            {
                res  *= var;
            }
            return res;
        }

        public static int fibonacci(int n)
        {
            throw new NotImplementedException();
        }
    }
}
