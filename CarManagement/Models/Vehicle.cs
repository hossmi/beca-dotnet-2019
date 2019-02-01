using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Wheel> wheels = new List<Wheel>();
        private List<Door> doors = new List<Door>();
        private int doorsCount;
        private int wheelsCount;
        private Engine engine = new Engine();
        private CarColor color;
        private IEnrollment enrollment;

        public Vehicle(List<Wheel> wheels, List<Door> doors, Engine engine, CarColor color, IEnrollment enrollment)
        {
            this.wheels = wheels;
            this.doors = doors;
            this.engine = engine;
            this.color = color;
            this.enrollment = enrollment;
        }

        public Vehicle()
        {
        }
        public CarColor carolor
        {
            set
            {
                this.color = value;
            }
            get
            {
                return this.color;
            }
        }
        public int DoorsCount
        {
            get
            {
                return this.doors.Count;
            }
            set
            {
                this.doorsCount = value;
            }
        }

        public int WheelCount
        {
            get
            {
                return this.wheels.Count;
            }
            set
            {
                this.wheelsCount = value;
            }
        }

        public Engine Engine
        {
            get
            {
                return this.engine;
            }
            set
            {
                this.engine = value;
            }
        }

        public IEnrollment Enrollment
        {
            get
            {
                return this.enrollment;
            }
        }

        public Door[] Doors
        {
            get
            {
                return this.doors.ToArray();
            }
        }

        public Wheel[] Wheels
        {
            get
            {
                return this.wheels.ToArray();
            }
        }
        public void setWheelsPressure(double pression)
        {
            Asserts.isTrue(pression > 0);
            foreach (Wheel wheel in this.wheels)
            {
                wheel.Pressure = pression;
            }
        }
    }
}