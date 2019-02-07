using System;

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
            set
            {
                this.pressure = value;
            }
        }
    }
}