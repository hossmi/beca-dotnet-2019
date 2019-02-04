using System;

namespace CarManagement.Models
{
    public class Wheel
    {
            private double pressure;
            public double Pressure
            {
                set
                {
                    this.pressure = value;
                }
                get
                {
                    return this.pressure;
                }
            }

            public Wheel()
            {
                this.pressure = 0;
            }

            public Wheel(double pressure)
            {
                this.Pressure = pressure;
            }
        }
    }