using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Wheel> wheels;

        public int DoorsCount
        {
            get
            {
                throw new NotImplementedException();
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
        public string[] Doors = new string[5]
        {
            "FrontRight",
            "FrontLeft",
            "RearRight",
            "RearLeft",
            "Trunk",
        };
    }

        public Wheel[] Wheels
        {
            get
            {
                return this.wheels.ToArray();
            }
        }

        public void SetWheelsPressure(double pression)
        {
            throw new NotImplementedException();
        }
    }
}