using System;
using System.Collections.Generic;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;

namespace CarManagement.Models
{
    public class Vehicle : IVehicle
    {
        private readonly IEngine engine;
        private readonly List<IDoor> doorList;
        private readonly List<IWheel> wheelList;
        private CarColor color;

        private readonly IEnrollment enrollment;

        public Vehicle(IEngine engine, List<IDoor> doorList,
            List<IWheel> wheelList, CarColor color, IEnrollment enrollment)
        {
            this.engine = engine;
            this.doorList = doorList;
            this.wheelList = wheelList;
            this.color = color;
            this.enrollment = enrollment;
        }
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

        IEngine IVehicle.Engine { get; }

        IWheel[] IVehicle.Wheels
        {
            get => this.wheelList.ToArray();
        }

        IDoor[] IVehicle.Doors
        {
            get => this.doorList.ToArray();
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