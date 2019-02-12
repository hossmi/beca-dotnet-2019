using System;

namespace CarManagement.Models
{
    public class Wheel
    {

        private double pressure;

        public Wheel(double pressure)
        {
            this.pressure = pressure;
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