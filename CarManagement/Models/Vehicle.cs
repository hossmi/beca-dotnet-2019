using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private Engine engine;
        private List<Door> doors;
        private List<Wheel> wheels;
        private Enrollment enrollment;


        public Vehicle(List<Wheel> wheels,List<Door> doors, Engine engine, CarColor color)
        {
            this.doors = doors;
            this.engine = engine;
            this.Color = color;
            this.wheels = wheels;
            this.enrollment = new Enrollment("BBB",666);
        }

        public CarColor Color { get; }

        public int DoorsCount
        {
            get
            {
                return this.doors.Count;
            }
        }

        public int WheelCount
        {
            get
            {
                return this.wheels.Count;
            }
        }

        public Engine Engine
        {
            get
            {
                return this.engine;
            }
        }

        public string Enrollment
        {
            get
            {
                return this.enrollment.ToString();
            }
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //        throw new ArgumentException();

            //    this.enrollment = value;
            //}
        }

        public Wheel[] Wheels
        {
            get
            {
                return this.wheels.ToArray();
            }
        }

        public Door[] Doors
        {
            get
            {
                return this.doors.ToArray();
            }
        }

        public void setWheelsPressure(double pression)
        {
            foreach (Wheel Wheel in this.wheels)
            {
                Wheel.Pressure = pression;
            }
        }
    }
}