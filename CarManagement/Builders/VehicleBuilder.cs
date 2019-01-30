using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private int Horseporwer;
        private List<Door> doors;
        private List<Wheel> wheels;
        private Engine engine;
        private CarColor color;
        private int horsepower;
        private IEnrollmentProvider enrollmentProvider;
        private int counter;
        private int D00rs;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.counter < 4);
            this.counter++;
        }
        public void removeWheel()
        {
            this.counter--;
        }
        public void setDoors(int doorsCount)
        {
            this.D00rs = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            this.Horseporwer = horsePorwer;
        }

        public void setColor(CarColor color)
        {
        }

        public Vehicle build()
        {
            Asserts.isTrue(this.counter > 0);
            this.wheels = new List<Wheel>();
            this.doors = new List<Door>();
            this.engine = new Engine();
            this.engine.Horsepower = this.Horseporwer;
            IEnrollment enrollment = this.enrollmentProvider.getNew();
            for (int i = 0; i < this.counter; i++)
            {
                Wheel wheel = new Wheel();
                this.wheels.Add(wheel);
            }
            Asserts.isTrue(this.D00rs > 0 && this.D00rs <= 6);
            for (int i = 0; i < this.D00rs; i++)
            {
                Door door = new Door();
                this.doors.Add(door);
            }
            Vehicle vehicle = new Vehicle(this.wheels, this.doors, this.engine, this.color, enrollment);
            return vehicle;
        }
    }
}