using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        List<Door> doors;
        List<Wheel> wheels;

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
                throw new NotImplementedException();
            }
        }

        public Engine Engine
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Enrollment
        {
            get
            {
                throw new NotImplementedException();
            }
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //        throw new ArgumentException();

            //    this.enrollment = value;
            //}
        }

        public void SetWheelsPressure(double pression)
        {
            foreach (Wheel w in wheels)
            {
                w.FillWheel(pression);
            }
        }
    }
}