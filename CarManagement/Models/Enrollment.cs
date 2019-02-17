using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public class Enrollment
    {
        public Enrollment(String serial, int number)
        {
            this.Serial = serial;
            this.Number = number;
        }

        public string Serial { get; }
        public int Number { get; }

        public  override string ToString()
        {
            return this.Serial+"-"+this.Number.ToString();
        }
    }
}
