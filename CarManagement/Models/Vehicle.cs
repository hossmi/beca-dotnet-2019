using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Wheel> wheels;
        private List<Door> door;
        private Engine engine;
        private CarColor color;
        private IEnrollment enrollment;

        public Vehicle(List<Wheel> wheels, List<Door> door, Engine engine, CarColor color, IEnrollment enrollment)
        {
            this.wheels = wheels;
            this.door = door;
            this.engine = engine;
            this.color = color;
            this.enrollment = enrollment;
        }
        public int DoorsCount
        {
            get
            {
                return this.door.Count;
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
                return this.door.ToArray();
            }
        }

        public CarColor Color { get; }

        public void setWheelsPressure(double pression)
        {
            for (int i = 0; i < this.WheelCount; i++)
            {
                this.wheels[i].Pressure = pression;

            }
        }
    }
}