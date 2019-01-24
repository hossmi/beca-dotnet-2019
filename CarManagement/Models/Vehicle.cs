using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Door> doors;
        private List<Wheel> wheels;
        private Engine engine;
        private CarColor color;
        private string enrollment;

        public Door[] Doors
        {
            get
            {
                return doors.ToArray();
            }

        }
        public Wheel[] Wheels
        {
            get
            {
                return wheels.ToArray();
            }

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
        public string Enrollment
        {
            get
            {
                return enrollment;
            }
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //        throw new ArgumentException();

            //    this.enrollment = value;
            //}
        }
        
        public List<Wheel> SetWheels
        {
            set
            {
                wheels = value;
            }
        }

        public List<Door> SetDoors
        {
            set
            {
                doors = value;
            }
        }

        public Engine SetEngine
        {
            set
            {
                engine = value;
            }
        }
        public CarColor SetCarColor
        {
            set
            {
                color = value;
            }
        }

        public void setWheelsPressure(double pression)
        {
            foreach (Wheel w in wheels)
            {
                w.FillWheel(pression);
            }
        }

        public string SetEnrollment
        {
            set
            {
                enrollment = value;
            }
        }
    }
}