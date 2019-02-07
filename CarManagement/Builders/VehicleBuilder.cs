using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private CarColor color;
        private int numDoors;
        private int numWheels;
        private int horsePower;
        private int enrollment;


        public VehicleBuilder()
        {
            this.numWheels = 0;
            this.numDoors = 0;
            this.horsePower = 0;
            this.color = CarColor.Red;
            this.enrollment = 000000;
        }

        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            if (this.numWheels < 4)
            {
                this.numWheels++;
            }
            else
            {
                throw new Exception();
            }

        }

        public void setDoors(int doorsCount)
        {
            this.numDoors = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            this.horsePower = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            this.color = color;
        }

        public Vehicle build()
        {
            Engine engine = new Engine(this.horsePower);

            CarColor color = new CarColor();

            List<Door> doors = new List<Door>();
            for (int i = 0; i < this.numDoors; i++)
            {
                Door door = new Door();
                doors.Add(door);
            }

            List<Wheel> wheels = new List<Wheel>();

            if (this.numWheels == 0)
            {
                throw new Exception("vehicle has to have 1 wheel at least");
            }

            for (int i = 0; i < this.numWheels; i++)
            {
                Wheel wheel = new Wheel();
                wheels.Add(wheel);
            }

            string enrollment = getNextEnrollment();

            Vehicle vehicle = new Vehicle(wheels, doors, engine, color, enrollment);
            return vehicle;
        }

        private string getNextEnrollment()
        {
            this.enrollment++;
            string enrollmentString = this.enrollment.ToString();
            return enrollmentString;

        }
    }
}