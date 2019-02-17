using System;

namespace CarManagement.Models
{
    public class Wheel
    {
        private double pressureMax = 6;
        private double pressureMin = 5;
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
                if (value >= this.pressureMin && value <= this.pressureMax)
                {
                    this.presurre = value;
                }
            }
        }
    }
}