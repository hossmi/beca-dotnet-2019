using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private List<Door> doors = new List<Door>();
        private List<Wheel> wheels = new List<Wheel>();

        public void addWheel()
        {
            Wheel wheel = new Wheel();
            wheels.Add(wheel);
        }

        public void setDoors(int doorsCount)
        {
            throw new NotImplementedException();
        }

        public void setEngine(int horsePorwer)
        {
            throw new NotImplementedException();
        }

        public void setColor(CarColor red)
        {
            throw new NotImplementedException();
        }

        public Vehicle build()
        {
            throw new NotImplementedException();
        }
    }
}