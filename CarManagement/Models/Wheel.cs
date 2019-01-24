using System;

namespace CarManagement.Models
{
    public class Wheel

       
    {
        public Wheel()
        {
            Pressure = 0;
        }
        public double Pressure
        {
            get
            {
                return this.Pressure;
            }
            set
            {
                if (Pressure < 0)
                {
                    throw new NotFiniteNumberException();
                }
                else
                {
                    this.Pressure = Pressure;
                }
            }
        }
    }
}