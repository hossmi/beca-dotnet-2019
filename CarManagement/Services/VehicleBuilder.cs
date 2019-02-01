﻿using System;

using System.Collections.Generic;

using CarManagement.Services;

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

        private int numWheels;
        private int numDoors;
        private int horsePower;
        private CarColor color;
        private IEnrollment enrollment;
        public void addWheel()
        {
            Asserts.isTrue(this.numWheels < 4);
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

        public IVehicle build()

        {
            List<Wheel> wheel = new List<Wheel>();
            List<Door> door = new List<Door>();
            Engine engine = new Engine(this.horsePower);
            Vehicle vehicle = new Vehicle(wheel, door, engine, this.color, this.enrollment);
            {

            }
            return vehicle;
        }

        public void removeWheel()
        {
            throw new NotImplementedException();
        }
    }
}