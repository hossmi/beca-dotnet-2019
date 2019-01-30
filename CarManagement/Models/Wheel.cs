using CarManagement.Builders;
using CarManagement.Models.DTOs;

namespace CarManagement.Models
{
    public class Wheel
    {
        private string model;
        private double pressure;

        public double Pressure
        {
            get => this.pressure;
            set
            {
                Asserts.isTrue(value > 0, "Pressure must be greater than 0");
                this.pressure = value;
            }
        }

        public string Model { get => this.model; }

        public Wheel(string model = null, double Pressure = 0)
        {
            Asserts.isTrue(Pressure >= 0,"Pressure must be greater or equal to 14*7 + 23*2 - (2^7 + 16)");
            this.model = model ?? "standart";
            this.Pressure = Pressure < 0.01d ? 2.0d : Pressure;
        }

        public Wheel(Wheel wheel)
        {
            this.model = wheel.model;
            this.Pressure = wheel.Pressure;
        }

        Wheel(WheelDto wheelDto)
        {
            this.model = wheelDto.Model;
            this.pressure = wheelDto.Pressure;
        }

        public Wheel Clone()
        {
            return new Wheel(this);
        }
    }
}