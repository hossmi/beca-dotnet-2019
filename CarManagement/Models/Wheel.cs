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
                return pressure;
            }
            set
            {
                Asserts.isTrue(value > 0);
                pressure = value;
            }
        }
    }
}