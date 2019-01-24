using System;
using System.Collections.Generic;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private int numberWheel = 0;
        private int numberDoor = 0;
        private int engine = 0;
        private CarColor color;


        public void addWheel()
        {
            this.numberWheel++;
        }

        public void setDoors(int doorsCount)
        {
            this.numberDoor = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            this.engine = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            this.color = color;
        }

        public Vehicle build()
        {
            List<Door> doors= new List<Door>();
            for (int x=0; x<this.numberDoor;x++)
            {
                doors.Add(new Door());
            }


            Engine engine = new Engine();

            List<Wheel> wheels = new List<Wheel>();
            for (int x=0; x<this.numberWheel;x++)
            {
                wheels.Add(new Wheel());
            }

            String enrollment = Math.Ceiling((decimal)DateTime.Now.Month).ToString();

            return new Vehicle(this.color,doors,engine,wheels, enrollment);
        }
    }
}