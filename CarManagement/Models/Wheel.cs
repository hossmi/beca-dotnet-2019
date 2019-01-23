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
        }

        public void FillWheel(double pression)
        {
            pressure = pression;
        }
    }
}
