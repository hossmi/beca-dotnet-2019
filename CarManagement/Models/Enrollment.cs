using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Enrollment
    {
        private string serial;
        private int number;

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
        
        private int Number
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
