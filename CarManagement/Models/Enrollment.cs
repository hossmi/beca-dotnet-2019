using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Enrollment
    {
        private string serial = "AAA";
        private string number="0000";

        public Enrollment()
        {
            string enrollmentString = this.Serial + this.Number;
        }

        private string Serial
        {
            get
            {
                return this.serial;
            }
            set
            {
                value = this.serial;
            }
        }

        private string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                value = this.number;
            }
        }
       
    }
}

