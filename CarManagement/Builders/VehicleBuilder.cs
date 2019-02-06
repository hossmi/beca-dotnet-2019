using System;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private CarColor color;
        private int numDoors;
        private int numWheels;
        private int horsePower;
        
        public void addWheel()
        {
            this.numWheels++;
        }

        public void setDoors(int doorsCount)
        {
            this.numDoors = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            this.horsePower = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            this.color = color;
        }

        public Vehicle build()
        {
            throw new NotImplementedException();
        }
    }
}