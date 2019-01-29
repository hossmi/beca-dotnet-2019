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
                return engine;
            }
        }

        public IEnrollment Enrollment
        {
            get
            {
                return enrollment;
            }
        }

        public Wheel[] Wheels
        {
            get
            {
                return wheels.ToArray();
            }
        }
        public Door[] Doors
        {
            get
            {
                return doors.ToArray();
            }
        }

        public CarColor getColor()
        {
            return color;
        }

        public void setWheelsPressure(double pression)
        {
            for(int i = 0; i < WheelCount; i++)
            {
                Wheels[i].Pressure = pression;
            }
        }
        
    }
}