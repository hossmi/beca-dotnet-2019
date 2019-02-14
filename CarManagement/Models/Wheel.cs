using CarManagement.Builders;
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
                Asserts.isTrue(value > 0);
                this.pressure = value;
            }
        }
    }
}