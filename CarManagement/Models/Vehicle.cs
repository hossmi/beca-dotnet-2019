using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {

        public int DoorsCount
        {
            get
            {

            }
        }

        public int WheelCount
        {
            get
            {
                return wheelsCount;
            }
        }

        public Engine Engine
        {

            get
            {

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