using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private IEnrollment enrollment;
        private List<Wheel> wheels;
        private List<Door> doors;
        private Engine engine;
        private CarColor color;

        public Vehicle(List<Wheel> wheels, List<Door> doors, Engine engine, CarColor color, IEnrollment enrollment)
        {
            this.wheels = wheels;
            this.doors = doors;
            this.engine = engine;
            this.color = color;
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

        public CarColor getColor()
        {
            return this.color;
        }

        public void setWheelsPressure(double pression)
        {
            Asserts.isTrue(pression > 0);
            for(int i = 0; i < this.WheelCount; i++)
            {
                this.Wheels[i].Pressure = pression;
            }
        }
        
    }
}