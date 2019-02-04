using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private Engine engine;
        private CarColor color;
        private List<Door> doors;
        private List<Wheel> wheels;


        public Vehicle(List<Wheel> wheels,List<Door> doors, Engine engine, CarColor color)
        {
            this.doors = doors;
            this.engine = engine;
            this.color = color;
            this.wheels = wheels;
        }

       

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
                throw new NotImplementedException();
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