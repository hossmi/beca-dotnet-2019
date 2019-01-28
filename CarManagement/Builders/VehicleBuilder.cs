﻿using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder : IVehicleBuilder
    {
        const int MAX_WHEELS = 4;
        private int wheelsCount;
        private int doorsCount;
        private int enginePower;
        private CarColor colorCode;
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.wheelsCount = 0;
            this.doorsCount = 0;
            this.enginePower = 0;
            this.colorCode = 0;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.wheelsCount < MAX_WHEELS, "Maximum number of wheels reached. Cannot add more wheels.");
            this.wheelsCount++;
        }

        public void setDoors(int doorsCount)
        {
            Asserts.isTrue(doorsCount >= 0,"Cannot create a vehicle with negative doors");
            this.doorsCount = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            Asserts.isTrue(horsePorwer > 0, "Cannot create an engine with 0 or less Horse Power.");
            this.enginePower = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            //if (Enum.IsDefined(typeof(CarColor), color) == false)
            //    throw new ArgumentException($"Parameter {nameof(color)} has not a valid value.");
            Asserts.isEnumDefined<CarColor>(color,"The selected color does not match.");
            this.colorCode = color;
        }

        public Vehicle build()
        {
            Asserts.isTrue(this.wheelsCount > 0);

            List<Wheel> wheels = CreateObject<Wheel>(wheelsCount);
            List<Door> doors = CreateObject<Door>(doorsCount);
            Engine engine = CreateEngine(enginePower);
            IEnrollment enrollment = enrollmentProvider.getNewEnrollment();

            Vehicle vehicle = new Vehicle(wheels, doors, engine, colorCode, enrollment);

            return vehicle;
        }

        private List<TItem> CreateObject<TItem>(int count) where TItem : class, new()
        {
            List<TItem> list = new List<TItem>();
            for (int i = 0; i < count; i++)
            {
                TItem obj = new TItem();
                list.Add(obj);
            }
            return list;
        }

        private Engine CreateEngine(int power)
        {
            Engine engine = new Engine(power);
            return engine;
        }

        public void removeWheel()
        {
            Asserts.isTrue(this.wheelsCount > 0, "The vehicle does not have any more wheels to remove.");
            this.wheelsCount--;
        }
    }
}