using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private string enrollment;
        private List<Wheel> wheels;
        private List<Door> doors;

        public Vehicle()
        {
            enrollment = "AL1234";
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

        public Engine Engine { get; }

        public string Enrollment
        {
            get
            {
                return enrollment;
            }
        }

        public Wheel[] Wheels
        {
            get
            {
                return wheels.ToArray();
            }
        }
        public Door[] Doors
        {
            get
            {
                return doors.ToArray();
            }
        }

        public void SetWheelsPressure(double pression)
        {
            for(int i = 0; i < wheels.Count; i++)
            {
                Wheels[i].Pressure = pression;
            }
        }
        
    }
}