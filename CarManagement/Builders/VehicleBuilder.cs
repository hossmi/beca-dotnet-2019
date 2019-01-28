using System;
using System.Collections.Generic;
using System.Linq;
using CarManagement.Models;
using CarManagement.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder : IVehicleBuilder
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
            Asserts.isTrue(wheelList.Count() < 4, "Cannot add more than 4 wheels");
            this.wheelList.Add(new Wheel());
        }

        //public void removeWheel(Wheel wheel = null)
        public void removeWheel()
        {
            Asserts.isTrue(wheelList.Count() > 0, "Cannot remove from none wheels");

            //wheel = this.wheelList.Last();
            //this.wheelList.Remove(wheel);
            this.wheelList.Remove(this.wheelList.Last());
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
            Asserts.isEnumDefined < CarColor > (color);
            this.color = color;
        }

        public Vehicle build()
        {
            Asserts.isFalse(wheelList.Count() <= 0, $"You cannot build a vehicle with {wheelList.Count()} wheels");

            IEnrollment toProvideEnrollment = this.enrollmentProvider.getNewEnrollment();

            return new Vehicle(this.EngineClone, this.DoorListClone,
                this.WheelListClone, this.Color, toProvideEnrollment );
        }
    }
}