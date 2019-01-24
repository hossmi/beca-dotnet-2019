using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public class Wheel
    {
        private double pressure;

        public double Pressure
        {
            get
            {
                return pressure;
            }

            set
            {
                if (value <= 0)
                    throw new ArgumentException("Wheel pressure value must be over 0.");
                pressure = value;
            }
        }
    }
}
