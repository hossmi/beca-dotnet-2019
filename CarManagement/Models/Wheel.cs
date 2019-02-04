using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;

namespace CarManagement.Models
{
    class Wheel : IWheel
    {
        private const string NOT_ENOUGH_PRESSURE = "Pressure must be greater or equal to 1";
        private const string TOO_MUCH_PRESSURE = "Pressure must be less or equal to 5";
        
        private double pressure;

        public double Pressure
        {
            get => this.pressure;
            set
            {
                Asserts.isTrue(value >= 1, NOT_ENOUGH_PRESSURE);
                Asserts.isTrue(value <= 5, TOO_MUCH_PRESSURE);
                this.pressure = value;
            }
        }

        public Wheel(double Pressure = 1)
        {
            Asserts.isTrue(Pressure >= 1, NOT_ENOUGH_PRESSURE);
            Asserts.isTrue(Pressure <= 5, TOO_MUCH_PRESSURE);
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