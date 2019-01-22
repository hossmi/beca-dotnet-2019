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
            int i = 1; int o = n;
            while(o >= i)
            {
                int temp = o * o - 1;
                o--;
            }
            throw new NotImplementedException();
        }

        public static int fibonacci(int n)
        {
            throw new NotImplementedException();
        }
    }
}
