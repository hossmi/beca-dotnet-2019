using System;
using System.Collections.Generic;
using CarManagement.Builders;
using CarManagement.Models;

namespace CarManagement.Models
{
    public class Vehicle
    {
        public readonly Engine Engine;
        private List<Door> doorList;
        private List<Wheel> wheelList;
        private CarColor color;

        private readonly string enrollment;

        public Vehicle(VehicleBuilder referenceVehicle, int enrollment)
        {
            this.Engine = referenceVehicle.Engine;
            this.doorList = referenceVehicle.DoorList;
            this.wheelList = referenceVehicle.WheelList;
            this.color = referenceVehicle.Color;

            this.enrollment = $"asd-{enrollment}-ab";
        }

        public int DoorsCount
        {
            get
            {
                return doorList.Count;
            }
        }

        public int WheelCount {get => wheelList.Count;}

        public string Enrollment
        {
            get
            {
                return this.enrollment;
            }
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //        throw new ArgumentException();

            //    this.enrollment = value;
            //}
        }

        public Door[] Doors { get => this.doorList.ToArray(); }
        public Wheel[] Wheels { get => this.wheelList.ToArray(); }

        public void setWheelsPressure(double pressure)
        {
            foreach( Wheel wheel in this.wheelList)
            {
               wheel.Pressure = pressure;
            }
        }
    }
}