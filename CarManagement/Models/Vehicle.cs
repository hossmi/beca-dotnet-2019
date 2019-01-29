using System;
using System.Collections.Generic;
using System.Linq;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private CarColor color;
        private IReadOnlyList<Wheel> wheels;
        private IEnrollment enrollment;
        private IReadOnlyList<Door> doors;
        private Engine engine;

        public Vehicle(IEnrollment enrollment)
        {
            this.enrollment = enrollment;
        }
        public Vehicle(CarColor color,   List<Wheel> wheels, IEnrollment enrollment, List<Door> doors, Engine engine)
        {
            this.color = color;
            this.wheels = wheels;
            this.enrollment = enrollment;
            this.doors = doors;
            this.engine = engine;
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

        public CarColor Color { get; }

        public void setWheelsPressure(double pression)
        {
            foreach (Wheel wheel in this.wheels )
            {
                wheel.Pressure = pression;
            }
        }
    }
}