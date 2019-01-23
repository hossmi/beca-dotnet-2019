using System;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private Vehicle vehicle;

        public void addWheel()
        {
            throw new NotImplementedException();
        }

        public void setDoors(int doorsCount)
        {
            /*for(int i = 0; i < doorsCount; i++)
            {
                door[i] = new Door();
            }*/
        }

        public void setEngine(int horsePorwer)
        {
            //this.engine.HorsePorwer = horsePorwer;
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