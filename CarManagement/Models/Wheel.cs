using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{

    public class Wheel
    {

        public double Pressure
        {
            set
            {
                if (value>=0)
                {
                    this.Pressure = value;
                }
                else
                {
                    throw new ArgumentException("value");
                }
            }
            get
            {
                return this.Pressure;
            }
        }

        public Wheel()
        {
            Pressure = 0;
        }

   
    }
}
