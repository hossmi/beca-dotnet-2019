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
        private IEnrollment enrollment;
        private CarColor colorCode;

        public Vehicle(List<Wheel> wheels, List<Door> doors, Engine engine, CarColor colorCode, IEnrollment enrollment)
        {
            this.wheels = wheels;
            this.doors = doors;
            this.engine = engine;
            this.colorCode = colorCode;
            this.enrollment = enrollment;
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
        
        public List<Wheel> SetWheels
        {
            set
            {
                this.wheels = value;
            }
        }

        public List<Door> SetDoors
        {
            set
            {
                this.doors = value;
            }
        }

        public Engine SetEngine
        {
            set
            {
                this.engine = value;
            }
        }

        public CarColor SetCarColor
        {
            set
            {
                this.color = value;
            }
        }

        public CarColor Color { get; }

        public void setWheelsPressure(double pression)
        {
            foreach (Wheel w in wheels)
            {
                w.FillWheel(pression);
            }
        }

        public IEnrollment SetEnrollment
        {
            set
            {
                this.enrollment = value;
            }
        }
    }
}