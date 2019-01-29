using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    class WheelDto
    {
        public double Pressure { get; set; }

        public WheelDto(Wheel w)
        {
            this.Pressure = w.Pressure;
        }

        public Wheel ConvertToWheel()
        {
            Wheel w = new Wheel();
            w.FillWheel(this.Pressure);

            return w;
        }
    }
}

