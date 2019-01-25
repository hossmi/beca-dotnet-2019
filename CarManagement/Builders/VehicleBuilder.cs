using System;
using System.Collections.Generic;
using System.Linq;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder
    {
        private readonly IEnrollmentProvider enrollmentProvider;
        
        private Engine engine;
        private List<Door> doorList;
        private List<Wheel> wheelList;
        private CarColor color;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.engine = new Engine();
            this.doorList = new List<Door>();
            this.wheelList = new List<Wheel>();
            this.color = CarColor.White;
        }

        public Engine EngineClone { get => new Engine(this.engine); }
        public CarColor Color { get => this.color; }
        public List<Wheel> WheelListClone
        {
            get
            {
                List<Wheel> wheelList = new List<Wheel>();
                foreach(Wheel wheel in this.wheelList)
                {
                    wheelList.Add(new Wheel(wheel));
                }

                return wheelList;
            }
        }
        public List<Door> DoorListClone
        {
            get
            {
                List<Door> doorList = new List<Door>();
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
                throw new InvalidOperationException
                    ("Cannot add more than 4 wheels");
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
            if (wheelList.Count() <= 0)
                throw new System.InvalidOperationException
                    ($"You cannot build a vehicle with {wheelList.Count()} wheels");

            return new Vehicle(this.EngineClone, this.DoorListClone,
                this.WheelListClone, this.Color, enrollmentProvider.getNewEnrollment() );
        }
    }
}