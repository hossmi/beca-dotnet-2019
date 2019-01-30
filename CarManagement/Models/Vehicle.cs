using System;
using System.Collections.Generic;
using CarManagement.Builders;
using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Models
{
    public class Vehicle
    {
        private List<Door> doorList;
        private List<Wheel> wheelList;
        private CarColor color;

        private readonly IEnrollment enrollment;

        public Vehicle(Engine engine, List<Door> doorList,
            List<Wheel> wheelList, CarColor color, IEnrollment enrollment)
        {
            this.Engine = engine;
            this.doorList = doorList;
            this.wheelList = wheelList;
            this.color = color;
            this.enrollment = enrollment;
        }
        public Engine Engine { get; }
        public int DoorsCount
        {
            get => this.doorList.Count;
        }

        public int WheelCount
        {
            get => this.wheelList.Count;
        }

        public IEnrollment Enrollment
        {
            get => this.enrollment;
        }

        public CarColor Color { get => this.color; }

        public Door[] Doors
        {
            get => this.doorList.ToArray();
        }
        public Wheel[] Wheels
        {
            get => this.wheelList.ToArray();
        }

        public void setWheelsPressure(double pressure)
        {
            foreach( Wheel wheel in this.wheelList)
            {
               wheel.Pressure = pressure;
            }
        }
    }
}