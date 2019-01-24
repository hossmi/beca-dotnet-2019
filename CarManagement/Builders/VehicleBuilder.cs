using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private List<Wheel> wheels = new List<Wheel>();
        private List<Door> doors = new List<Door>();
        private Engine engine = new Engine();
        private CarColor color;
        

        public void addWheel()
        {
            Wheel wheel = new Wheel();
            wheels.Add(wheel);
        }

        public void setDoors(int doorsCount)
        {
            Door door = new Door();

            for (int i = 0; i < doorsCount; i++)
            {
                doors.Add(door);
            }
        }

        public void setEngine(int horsePorwer)
        {
            engine = new Engine(horsePorwer);
        }

        public void setColor(CarColor red)
        {
            color = red;
        }

        public Vehicle build()
        {
            Vehicle vehicle = new Vehicle();

            vehicle.SetWheels = wheels;
            vehicle.SetDoors = doors;
            vehicle.SetEngine = engine;
            vehicle.SetCarColor = color;


            return vehicle;
        }
    }
}