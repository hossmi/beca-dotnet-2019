using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        public const int maxDoorsCount = 6;
        public const int minDoorsCount = 0;
        public const int minEngineHorsePower = 0;

        private int wheel;
        private int door;
        private int engineHorsePower;
        private CarColor color;
        private Enrollment enrollment;

        public VehicleBuilder(Enrollment enrollment)
        {
            this.wheel = 0;
            this.door = 0;
            this.engineHorsePower = 0;
            this.color = CarColor.Red;
            this.enrollment = enrollment;
        }


        CarColor carColor = new CarColor();

        public void addWheel()
        {
            this.wheel++;
        }

        public void setDoors(int doorsCount)
        {
            if (doorsCount >= minDoorsCount && doorsCount >= maxDoorsCount)
            {
                this.door = doorsCount;
            }
            
        }

        public void setEngine(int horsePorwer)
        {
            if (horsePorwer >= minEngineHorsePower)
            {
                this.engineHorsePower = horsePorwer;
            }
        }

        public void setColor(CarColor color)
        {
            this.carColor = color;
        }

        public Vehicle build()
        {
            Engine engine = new Engine(this.engineHorsePower);

            CarColor color = new CarColor();

            List<Door> doors = new List<Door>();
            for (int i = 0; i < this.door; i++)
            {
                Door d = new Door();
                doors.Add(d);
            }

            List<Wheel> wheels = new List<Wheel>();
            for (int i = 0; i < this.wheel; i++)
            {
                Wheel w = new Wheel();
                wheels.Add(w);
            }

            return new Vehicle(wheels, doors, engine, color);
        }
    }
}