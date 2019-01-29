using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
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

        private List<Wheel> wheels = new List<Wheel>();
        private List<Door> doors = new List<Door>();
        private Engine engine = new Engine();
        private CarColor color;
        private IEnrollment enrollment;


        public List<Wheel> carwheel
        {
            set
            {
                wheels = value;
            }
        }
        public List<Door> cardoor
        {
            set
            {
                doors = value;
            }
        }
        public Engine carengine
        {
            set
            {
                engine = value;
            }
        }
        public CarColor carcolor
        {
            set
            {
                color = value;
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
            if (pression > 0)
            {
                foreach (Wheel w in wheels)
                {
                    w.Pressure = pression;
                }
            }

        }
    }
}