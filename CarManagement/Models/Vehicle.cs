using System;
using System.Collections.Generic;
using CarManagement.Models.Models;

namespace CarManagement.Models
{
    
    public partial class Vehicle
    {

        List<Door> doors;

        public Door[] Doors
        {
            get
            {
                throw new NotImplementedException();
            }

        }


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

        public void SetWheelsPressure(double pression)
        {
            throw new NotImplementedException();
        }

    }
}