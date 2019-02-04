using System;

namespace CarManagement.Models
{
    public class Wheel
    {

        private double presurre;
        public Wheel()
        {
            Pressure = 0;
            this.presurre = 0;
        }

        public double Pressure
        {
            get
            {
                return this.presurre;
            }
            set
            {
                this.presurre = value;
            }
        }
    }
}