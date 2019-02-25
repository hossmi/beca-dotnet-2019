using System;

namespace CarManagement.Models
{
    public class Wheel
    {
        private double pressure;

        public Wheel()
        {
            this.pressure = 1;
        }
        public double Pressure
        {

            get
            {
                return pressure;
            }
            set
            {
                pressure = value;
            }
        }
    }
}