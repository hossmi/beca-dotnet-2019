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
        public CarColor carColor { set; get; }
        public Engine Engine { get; set; }
        public IEnrollment Enrollment { get; }

        public Vehicle(List<Wheel> wheels, List<Door> doors, Engine engine, CarColor color, IEnrollment enrollment)
        {
            this.wheels = wheels;
            this.doors = doors;
            this.Engine = engine;
            this.carColor = color;
            this.Enrollment = enrollment;
        }

        public Vehicle()
        {
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