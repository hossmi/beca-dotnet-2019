using System;
using CarManagement.Models.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        public int DoorsCount
        {
            get
            {
                return DoorsCount;
            }

            set
            {
                DoorsCount = value;
            }
        }

        public int WheelCount
        {
            get
            {
                return WheelCount;
            }
            set
            {
                WheelCount = value;
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
            throw new NotImplementedException();
        }
    }
}