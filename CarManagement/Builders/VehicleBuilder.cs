using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private int counter;
        private int D00rs;
        private int horsePorwer;
        private List<Door> doors;
        private List<Wheel> wheels;
        private Engine engine;
        private CarColor color;
        private IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            counter++;
        }

        public void setDoors(int doorsCount)
        {
            D00rs = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
        }

        public void setColor(CarColor color)
        {
        }

        public Vehicle build()
        {
            Vehicle vehicle = new Vehicle();
            wheels = new List<Wheel>();
            doors = new List<Door>();
            engine = new Engine();
            engine.Horsepower = horsePorwer;
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
            vehicle.carcolor = this.color;
            vehicle.cardoor = this.doors;
            vehicle.carwheel = this.wheels;
            vehicle.carengine = this.engine;
            return vehicle;
        }
    }
}