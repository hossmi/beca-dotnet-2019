using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        

        public int DoorsCount
        {
            get
            {
                return Doors.Count;
            }
        }

        public int WheelCount
        {
            get
            {
                return Wheels.Count;
            }
        }

        public Engine Engine { get; }

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

        public List<Wheel> Wheels { get; }
        public List<Door> Doors { get;  }

        public void SetWheelsPressure(double pression)
        {
            for(int i = 0; i < Wheels.Count; i++)
            {
                Wheels[i].Pressure = pression;
            }
        }
        
    }
}