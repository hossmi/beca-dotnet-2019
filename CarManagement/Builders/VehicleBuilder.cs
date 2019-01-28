using System;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        private int numWheels;
        private int numDoors;
        private int horsePower;
        private CarColor color;
        public void addWheel()
        {
            this.numWheels++;
        }

        public void setDoors(int doorsCount)
        {
            this.numDoors = doorsCount;
        }

        public void setEngine(int horsePower)
        {
            this.horsePower = horsePower;
        }

        public void setColor(CarColor color)
        {
            this.color = color;
        }

        public Vehicle build()
        {
            throw new NotImplementedException();
        }

        public void removeWheel()
        {
            throw new NotImplementedException();
        }
    }
}