using System;
using System.Collections.Generic;
using CarManagement.Models;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using CarManagement.Core;
using System.Linq;

namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private const string ERR_WHEEL_ADDITION_CALL = "Cannot add more than 4 wheels";
        private const string ERR_WHEEL_REMOVAL_CALL = "Cannot remove from none wheels";
        private const string ERR_DOOR_NUMBER_OVER_MAX = "You cannot have more than 6 doors";
        private const string ERR_DOOR_NUMBER_UNDER0 = "You cannot have less than 0 doors";

        private readonly IEnrollmentProvider enrollmentProvider;
        
        private Engine engine;
        private readonly List<IDoor> doorList;
        private readonly List<IWheel> wheelList;
        private CarColor color;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.engine = new Engine();
            this.doorList = new List<IDoor>();
            this.wheelList = new List<IWheel>();
            this.color = CarColor.White;
        }

        public IEngine EngineClone { get => this.engine.Clone(); }
        public CarColor Color { get => this.color; }
        public List<IWheel> WheelListClone
        {
            get
            {
                List<IWheel> wheelList = new List<IWheel>();
                foreach(Wheel wheel in this.wheelList)
                {
                    wheelList.Add(wheel.Clone());
                }

                return wheelList;
            }
        }
        public List<IDoor> DoorListClone
        {
            get
            {
                List<IDoor> doorList = new List<IDoor>();
                foreach (Door door in this.doorList)
                {
                    doorList.Add(door.Clone());
                }

                return doorList;
            }
        }

        public void addWheel()
        {
            Asserts.isTrue(this.wheelList.Count() < 4, ERR_WHEEL_ADDITION_CALL);
            this.wheelList.Add(new Wheel());
        }

        //public void removeWheel(Wheel wheel = null)
        public void removeWheel()
        {
            Asserts.isTrue(this.wheelList.Count() > 0, ERR_WHEEL_REMOVAL_CALL);

            //wheel = wheel ?? this.wheelList.Last();
            //this.wheelList.Remove(wheel);
            this.wheelList.Remove(this.wheelList.Last());
        }

        public void setDoors(int doorsCount)
        {
            Asserts.isFalse( doorsCount >= 6, ERR_DOOR_NUMBER_OVER_MAX);
            Asserts.isFalse(doorsCount < 0, ERR_DOOR_NUMBER_UNDER0);
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

        public IVehicle build()
        {
            Asserts.isFalse(this.wheelList.Count() <= 0, $"You cannot build a vehicle with {this.wheelList.Count()} wheels");

            IEnrollment toProvideEnrollment = this.enrollmentProvider.getNew();

            return new Vehicle(this.EngineClone, this.DoorListClone,
                this.WheelListClone, this.Color, toProvideEnrollment );
        }

        public IVehicle import(VehicleDto vehicleDto)
        {
            throw new NotImplementedException();
        }

        public VehicleDto export(IVehicle vehicleDto)
        {
            throw new NotImplementedException();
        }
    }
}