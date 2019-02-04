using System;

namespace CarManagement.Models
{
    public class Wheel
    {
        private double pressure;

        public Wheel()
        {
            this.pressure = 0;
        }
        public Wheel(double pressure)
        {
            this.Pressure = pressure;
        }
        public double Pressure
        {
            get
            {
                return this.pressure;
            }
            set
            {
                this.pressure = value;
            }
        }
    }
}