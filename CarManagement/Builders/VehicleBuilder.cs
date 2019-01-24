using System;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private int nWheel = 0;
        private int doors = 0;
        private int engine = 0;
        private CarColor color;


        public void addWheel()
        {
            this.nWheel++;
        }

        public void setDoors(int doorsCount)
        {
            this.doors = ;
        }

        public void setEngine(int horsePorwer)
        {
            this.engine = horsePorwer;
        }

        public void setColor(CarColor valor)
        {
            this.color = valor;
        }

        public Vehicle build()
        {
            return new Vehicle(nWheel, doors, engine,);
            throw new NotImplementedException();
        }
    }
}