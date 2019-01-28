using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Door> doors = new List<Door>();
        private List<Wheel> wheels = new List<Wheel>();
        private IEnrollment enrollment;
        private CarColor carColor;

        public Vehicle(List<Door> doors, List<Wheel> wheels, Engine engine, IEnrollment enrollment, CarColor carcolor)
        {
            this.doors = doors;
            this.wheels = wheels;
            this.Engine = engine;
            this.enrollment = enrollment;
            this.carColor = carcolor;
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

        public IEnrollment Enrollment
        {
            get
            {
                return this.enrollment;
            }
        }
        public int WheelCount
        {
            get
            {
                return wheels.Count;
            }
        }

        public Engine Engine { get; }

        public CarColor Carcolor
        {
            get
            {
                return carColor;
            }
            set
            {
                if (Enum.IsDefined(typeof(CarColor), value) == false)
                    throw new ArgumentException("Unexpected car color value.");
                carColor = value;
            }
        }

        public CarColor Color { get; }

        public void setWheelsPressure(double pression)
        {
            foreach (Wheel wheel in wheels)
                wheel.Pressure = pression;
        }
    }
}