using System;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        public Vehicle Vehicle { get; }

        public void addWheel()
        {
            this.Vehicle.Wheels.Add(new Wheel());
        }

        public void setDoors(int doorsCount)
        {
            for(int i = 0; i < doorsCount; i++)
            {
                this.Vehicle.Doors.Add(new Door());
            }
        }

        public void setEngine(int horsePorwer)
        {
            this.Vehicle.Engine.HorsePorwer = horsePorwer;
        }

        public void setColor(CarColor red)
        {
            throw new NotImplementedException();
        }

        public Vehicle build()
        {
            return Vehicle;
        }
    }
}