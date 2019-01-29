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
            Asserts.isTrue(counter < 4);
            counter++;
        }
        public void removeWheel()
        {
            counter--;
        }
        public void setDoors(int doorsCount)
        {
            D00rs = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            Horseporwer = horsePorwer;
        }

        public void setColor(CarColor color)
        {
        }

        public Vehicle build()
        {
            Asserts.isTrue(counter > 0);
            wheels = new List<Wheel>();
            doors = new List<Door>();
            engine = new Engine();
            engine.Horsepower = Horseporwer;
            IEnrollment enrollment = enrollmentProvider.getNew();
            for (int i = 0; i < counter; i++)
            {
                Wheel wheel = new Wheel();
                wheels.Add(wheel);
            }
            for (int i = 0; i < D00rs; i++)
            {
                Door door = new Door();
                doors.Add(door);
            }
            Vehicle vehicle = new Vehicle(wheels, doors, engine, color, enrollment);
            return vehicle;
        }
    }
}