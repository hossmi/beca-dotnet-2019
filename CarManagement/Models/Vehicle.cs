using System;
using System.Collections.Generic;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private CarColor color;
        private Door [] doors;
        private Engine engines;
        //private Wheel wheels;
        private Wheel [] wheels;
        private string enrollment;
        GeneraRandom NumRandom = new GeneraRandom();
         
       

        public Vehicle(CarColor color,Door[]doors, Engine engines, Wheel[]wheels,string enrrollment)
        {
            this.color = color;
            this.doors = doors;
            this.engines = engines;
            this.wheels = wheels;
            this.enrollment = enrollment;
            
            
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
                return (this.WheelCount);
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
         
        }

        public Wheel[] Wheels
        {
            get
            {
                return this.wheels;
            }
        }

        public void SetWheelsPressure(double pression)
        {
            Wheel pressure = new Wheel();   
            
        }
     
    }
}