using System;
using System.Collections.Generic;
using System.Linq;
using CarManagement.Models;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private Engine engine;
        private List<Door> doorList;
        private List<Wheel> wheelList;
        private CarColor color;

        private int lastIssuedEnrollment; // asd-0000-aa

        public Engine Engine { get => new Engine(engine); }
        public int LastIssuedEnrollment { get => lastIssuedEnrollment; }
        public CarColor Color { get => color; }
        public List<Wheel> WheelList
        {
            get
            {
                List<Wheel> wheelList = null;
                foreach(Wheel wheel in this.wheelList)
                {
                    wheelList.Add(new Wheel(wheel));
                }

                return wheelList;
            }
        }
        public List<Door> DoorList
        {
            get
            {
                List<Door> doorList = null;
                foreach (Door door in this.doorList)
                {
                    doorList.Add(new Door(door));
                }

                return doorList;
            }
        }

        public void addWheel()
        {
            if (wheelList.Count() < 4)
                this.wheelList.Add(new Wheel());
            else
                throw new InvalidOperationException("Cannot add more than 4 wheels");
        }

        public void setDoors(int doorsCount)
        {
            if (doorsCount > this.doorList.Count)
            {
                doorsCount = doorsCount - this.doorList.Count;
                for (int i = 0; i < doorsCount; i++)
                {
                    this.doorList.Add(new Door());
                }
            }
            else if (doorsCount < this.doorList.Count)
            {
                this.doorList.Clear();
                for (int i = 0; i < doorsCount; i++)
                {
                    this.doorList.Add(new Door());
                }
            }
        }

        public void setEngine(int horsePorwer)
        {
            this.engine = new Engine(horsePorwer);
        }

        public void setColor(CarColor color)
        {
            this.color = color;
        }

        public Vehicle build()
        {
            int temp = this.lastIssuedEnrollment;
            this.lastIssuedEnrollment++;
            return new Vehicle(this, temp);
        }
    }
}