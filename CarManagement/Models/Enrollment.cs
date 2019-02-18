using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public class Enrollment
    {
        private readonly string serial;
        private readonly int number;

        public Enrollment(String serial, int number)
        {
            this.serial = serial;
            this.number = number;
        }

        public string Serial
        {
            get
            {
                return this.serial;
            }
        }
        public int Number
        {
            get
            {
                return this.number;
            }
        }

        public override string ToString()
        {
            return this.Serial + "-" + this.Number.ToString();
        }
    }
}
