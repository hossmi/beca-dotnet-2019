using CarManagement.Builders;
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
                return this.pressure;
            }
        }

        public void FillWheel(double pression)
        {
            Asserts.isTrue(pression > 0,"Cannot set pressure lower than 0");
            this.pressure = pression;
        }
    }
}
