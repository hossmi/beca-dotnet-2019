using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public class Enrollment
    {
        private string serial;
        private int number;

        public Enrollment(String serial, int number)
        {
            this.serial = serial;
            this.number = number;
        }

        public override string ToString()
        {
            return this.serial+this.number.ToString();
        }
    }
}
