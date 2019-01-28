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

        public void addWheel()
        {
            throw new NotImplementedException();
        }

        public void setDoors(int doorsCount)
        {
            throw new NotImplementedException();
        }

        public void setEngine(int horsePorwer)
        {
            throw new NotImplementedException();
        }

        public void setColor(CarColor color)
        {
            throw new NotImplementedException();
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