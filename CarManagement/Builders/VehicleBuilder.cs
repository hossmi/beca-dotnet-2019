﻿using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private List<Door> doors = new List<Door>();
        private List<Wheel> wheels = new List<Wheel>();
        private Engine engine = new Engine();
        private CarColor color;

        public void addWheel()
        {
            Wheel wheel = new Wheel();
            wheels.Add(wheel);
        }

        public void setDoors(int doorsCount)
        {
            for(int i=0;i< doorsCount;i++)
            {
                Door door = new Door();
                doors.Add(door);
            }
        }

        public void setEngine(int horsePorwer)
        {
            engine.Horsepower = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            this.color = color;
        }

        public Vehicle build()
        {
            Vehicle vehicle = new Vehicle();
            vehicle.carcolor = this.color;
            vehicle.cardoor = this.doors;
            vehicle.carwheel = this.wheels;
            vehicle.carengine = this.engine;
            return vehicle;
        }
    }
}