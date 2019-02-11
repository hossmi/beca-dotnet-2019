using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private readonly List<Wheel> wheels;
        private readonly List<Door> doors;

        public Vehicle(List<Wheel> wheels, List<Door> doors, Engine engine, IEnrollment enrollment, CarColor carColor)
        {
            if (doors.Count >= 0 && doors.Count <= 6)
            {
                this.doors = doors;
            }
            if (wheels.Count > 0 && wheels.Count <= 4)
            {
                this.wheels = wheels;
            }

            this.Engine = engine;
            this.Enrollment = enrollment;
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

        public Engine Engine { get; }

        public IEnrollment Enrollment { get; }

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
            if (pression >= 0)
            {
                foreach (Wheel iterWheel in this.wheels)
                {
                    iterWheel.Pressure = pression;
                }
            }
            else
            {
                throw new ArgumentException("Pression must be greater than 0.");
            }

        }
    }
}
