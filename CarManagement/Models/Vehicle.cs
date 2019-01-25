using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private CarColor color;
        private Engine engines;
        private string enrollment;
        private List<Door> doors;
        private Engine engine;
        private List<Wheel> wheels;

  

        public Vehicle(CarColor color, List<Door> doors, Engine engine, List<Wheel> wheels, string enrollment)
        {
            this.color = color;
            this.doors = doors;
            this.engine = engine;
            this.wheels = wheels;
            this.enrollment = enrollment;
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

        public Engine Engine
        {
            get
            {
                return this.Engine;                               
            }
        }

        public string Enrollment
        {
        
            get
            {
                return this.enrollment;
             
            }
<<<<<<< HEAD
         
=======
>>>>>>> 46e3256130078045a4863617e58a2c25cb5fde1d
        }

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

        public void setWheelsPressure(double pression)
        {
            Wheel pressure = new Wheel();   
            
        }
     
    }
}