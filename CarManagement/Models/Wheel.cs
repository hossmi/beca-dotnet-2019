using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;

namespace CarManagement.Models
{
    class Wheel : IWheel
    {
        private const string NOT_ENOUGH_PRESSURE = "Pressure must be greater than 0";
        
        private double pressure;

        public double Pressure
        {
            get => this.pressure;
            set
            {
                Asserts.isTrue(value >= 0, NOT_ENOUGH_PRESSURE);
                this.pressure = value;
            }
        }

        public Wheel(double Pressure = 0)
        {
            Asserts.isTrue(Pressure >= 0, NOT_ENOUGH_PRESSURE);
            this.Pressure = Pressure <= 0 ? 2.0d : Pressure;
        }

        public Wheel(Wheel wheel)
        {
            this.Pressure = wheel.Pressure;
        }

        Wheel(WheelDto wheelDto)
        {
            this.pressure = wheelDto.Pressure;
        }

        public Wheel Clone()
        {
            return new Wheel(this);
        }
    }
}