using System;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private Vehicle vehicle;
        private Wheel wheel;
        private Door door;
        
        public VehicleBuilder()
        {
            vehicle = new Vehicle();
        }

        public void addWheel()
        {
            wheel = new Wheel();
            vehicle.Wheels.Add(wheel);
        }

        public void setDoors(int doorsCount)
        {
            for(int i = 0; i < doorsCount; i++)
            {
                door = new Door();
                vehicle.Doors.Add(door);
            }
        }

        public void setEngine(int horsePorwer)
        {
            this.vehicle.Engine.HorsePorwer = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            throw new NotImplementedException();
        }

        public Vehicle build()
        {
            return vehicle;
        }
    }
}