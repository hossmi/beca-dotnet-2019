using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Wheel> wheels;
        private List<Door> doors;
        private Engine engine;
        private CarColor color;
        private string enrollment;


        public Vehicle(List<Wheel> wheels, List<Door> doors, Engine engine, CarColor color, string enrollment)
        {
            this.doors = doors;
            this.engine = engine;
            this.color = color;
            this.wheels = wheels;
            this.enrollment = enrollment;
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

        public IEnrollment Enrollment
        {
            get
            {
                return this.enrollment;
            }
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