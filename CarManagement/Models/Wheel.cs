using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public class Wheel
    {
        public float Pressure
        {
            get
            {
                return Pressure;
            }

            set
            {
                if (value <= 0)
                    throw new ArgumentException("Wheel pressure value must be over 0.");
                else
                    Pressure = value;
            }
        }
    }
}
