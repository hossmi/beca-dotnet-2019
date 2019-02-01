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
            set
            {
                Asserts.isTrue(value >= 0);
                Asserts.isTrue(value <= 5);
                this.pressure = value;
            }
            get
            {
                return this.pressure;
            }
        }

        public Wheel()
        {
            this.pressure = 1;
        }

        internal void FillWheel(double pressure)
        {
            Asserts.isTrue(pressure >= 1);
            this.pressure = pressure;
        }
    }
}
