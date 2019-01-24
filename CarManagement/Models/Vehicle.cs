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
        private Engine engine;
        private CarColor color;

        public Vehicle(int nWheels, int nDoors, int horsePorwer, CarColor color, string enrollment)
        {
            this.wheels = new List<Wheel>();
            this.doors = new List<Door>();
            this.engine = new Engine(horsePorwer);
            this.color = color;
            createWheels(nWheels);
            createDoors(nDoors);
            this.enrollment = enrollment;
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

        public Engine Engine
        {
            get
            {
                return engine;
            }
        }

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

        public void createWheels(int nWheels)
        {
            for (int i = 0; i < nWheels; i++)
            {
                Wheel aux = new Wheel();
                wheels.Add(aux);
            }
        }

        public void createDoors(int nDoors)
        {
            for (int i = 0; i < nDoors; i++)
            {
                Door aux = new Door();
                doors.Add(aux);
            }
        }

        public Door[] Doors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void setWheelsPressure(double pression)
        {
            for(int i = 0; i < WheelCount; i++)
            {
                Wheels[i].Pressure = pression;
            }
        }
        
    }
}