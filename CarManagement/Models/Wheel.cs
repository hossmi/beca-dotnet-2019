using System;

namespace CarManagement.Models
{
    public class Wheel
    {
        private double pression;

        public double Pressure {
            get
            {
                return this.pression;
            }
            set
            {
                this.pression = value;
            }
        }
    }
}
