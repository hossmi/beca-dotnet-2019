﻿using CarManagement.Builders;
using CarManagement.Models.DTOs;

namespace CarManagement.Models
{
    public class Wheel
    {
        private string model;
        private double pressure;

        public double Pressure
        {
            get => pressure;
            set
            {
                Asserts.isTrue(value > 0, "Pressure must be greater than 0");
                pressure = value;
            }
        }

        public string Model { get => model; }

        public Wheel(string model = null, double Pressure = 0)
        {
            this.model = model ?? "standart";
            this.Pressure = Pressure < 0.1d ? 2.0d : Pressure;
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