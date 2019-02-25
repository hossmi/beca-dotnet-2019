using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Wheel> wheels;
        private CarColor color;
        private Engine engine;
        private List<Door> doors;
        private IEnrollment enrollment;

        public Vehicle(CarColor color, Engine engine, List<Wheel> wheels, List<Door> doors, IEnrollment enrollment)
        {
            this.color = color;
            this.engine = engine;
            this.wheels = wheels;
            this.doors = doors;
            this.enrollment = enrollment;
        }

        public int DoorsCount
        {
            get
            {
                return doors.Count;
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
            foreach ( Wheel wheel in wheels)
            {
                wheel.Pressure = pression;
            }
        }
    }
}