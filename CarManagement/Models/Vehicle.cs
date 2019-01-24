using System;
using CarManagement.Models;
using CarManagement.Builders;

namespace CarManagement.Models
{
    public class Vehicle
    {
        public int DoorsCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void WheelCount
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
            throw new NotImplementedException();
        }
    }
}