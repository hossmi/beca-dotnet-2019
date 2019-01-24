using System;
using System.Collections.Generic;
<<<<<<< HEAD
using CarManagement.Models;
=======
>>>>>>> develop

namespace CarManagement.Models
{
    public class Vehicle
    {
<<<<<<< HEAD
        private string enrollment;
        private List<Wheel> wheels;
        private List<Door> doors;

        public Vehicle()
        {
            enrollment = "AL1234";
        }
=======
        private List<Wheel> wheels;
>>>>>>> develop

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

        public Wheel[] Wheels
        {
            get
            {
                return this.wheels.ToArray();
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