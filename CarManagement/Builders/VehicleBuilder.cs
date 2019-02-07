using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private CarColor color;
        private int numDoors;
        private int numWheels;
        private int horsePower;

        public VehicleBuilder()
        {
            this.numWheels = 0;
            this.numDoors = 0;
            this.horsePower = 0;
            this.color = CarColor.Red;
        }

        public void addWheel()
        {
            this.numWheels++;
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
                Door d = new Door();
                doors.Add(d);
            }

            List<Wheel> wheels = new List<Wheel>();
            for (int i = 0; i < this.numWheels; i++)
            {
                Wheel w = new Wheel();
                wheels.Add(w);
            }

            Enrollment enrollment = new Enrollment();

            Vehicle vehicle = new Vehicle(wheels, doors, engine, color,enrollment);
            return vehicle;
        }
    }
}