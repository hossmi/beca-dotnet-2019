﻿using System;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Services
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

        public IVehicle build()
        {
            throw new NotImplementedException();
        }

        public void removeWheel()
        {
            throw new NotImplementedException();
        }
    }
}