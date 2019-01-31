﻿using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private List<Door> doors;
        private List<Wheel> wheels;
        private Engine engine;
        private CarColor color;
        private int horsePorwer;
        private IEnrollmentProvider enrollmentProvider;
        private IEnrollment enrollment;
        private int doorsCount;
        private int wheelCounter = 0;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.wheelCounter < 4);
            this.wheelCounter++;
        }
        public void removeWheel()
        {
            Asserts.isTrue(this.wheelCounter >= 0);
            this.wheelCounter--;
        }

        public void setDoors(int doorsCount)
        {
            Asserts.isTrue(doorsCount >= 0 && doorsCount <= 6);
            this.doorsCount = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            Asserts.isTrue(horsePorwer >= 0);
            this.horsePorwer = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            this.color = color;
        }

        public Vehicle build()
        {
            Asserts.isTrue(this.wheelCounter > 0);
            this.wheels = new List<Wheel>();
            this.doors = new List<Door>();
            this.engine = new Engine();
            this.enrollment = this.enrollmentProvider.getNew();
            for (int i = 0; i < this.wheelCounter; i++)
            {
                Wheel wheel = new Wheel();
                this.wheels.Add(wheel);
            }
            for (int i = 0; i < this.doorsCount; i++)
            {
                Door door = new Door();
                this.doors.Add(door);
            }
            Vehicle vehicle = new Vehicle(this.wheels, this.doors, this.engine, this.color, this.enrollment);
            return vehicle;
        }
    }
}