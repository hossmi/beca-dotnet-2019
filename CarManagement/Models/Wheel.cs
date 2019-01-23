using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public class Wheel
    {
        private double pressure;

        private double Pressure
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

        public Wheel(double valor)
        {
            this.pressure = valor;
        }
    }
}
