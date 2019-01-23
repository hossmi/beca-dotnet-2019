using System;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private int nWheel = 0;
        private int nDoors = 0;
        private int engine = 0;
        private CarColor color;


        public void addWheel()
        {
            this.nWheel++;
        }

        public void setDoors(int doorsCount)
        {
            this.nDoors++;
        }

        public void setEngine(int horsePorwer)
        {
            this.engine= horsePorwer;
        }

        public void setColor(CarColor red)
        {
            this.color = red;
        }

        public Vehicle build()
        {
            throw new NotImplementedException();
        }
    }
}