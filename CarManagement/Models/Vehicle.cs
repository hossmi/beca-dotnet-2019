using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Door> doors = new List<Door>();
        private List<Wheel> wheels = new List<Wheel>();

        public Vehicle(List<Door> doors, List<Wheel> wheels, Engine engine, String enrollment, CarColor carcolor)
        {
            this.doors = doors;
            this.wheels = wheels;
            this.Engine = engine;
            this.Enrollment = enrollment;
            this.Carcolor = carcolor;
        }

        public Door[] Doors
        {
            get
            {
                return doors.ToArray();
            }
        }
        public int DoorsCount
        {
            get
            {
                return doors.Count;
            }
        }

        public Wheel[] Wheels
        {
            get
            {
                return wheels.ToArray();
            }
        }

        public int WheelCount
        {
            get
            {
                return wheels.Count;
            }
        }

        public Engine Engine
        {
            get
            {
                return Engine;
            }
            set
            {
                Engine = value;
            }
        }

        public String Enrollment
        {
            get
            {
                return Enrollment;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("The enrollment can not be empty or have blank spaces.");
                Enrollment = value;
            }
        }

        public CarColor Carcolor
        {
            get
            {
                return Carcolor;
            }
            set
            {
                if (Enum.IsDefined(typeof(CarColor), value) == false)
                    throw new ArgumentException("Unexpected car color value.");
                Carcolor = value;
            }
        }

        public void SetWheelsPressure(double pression)
        {
            foreach (Wheel wheel in wheels)
                wheel.Pressure = pression;
        }
    }
}