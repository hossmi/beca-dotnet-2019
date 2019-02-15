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
        #region "CONSTS"
        private const int MAX_WHEELS = 4;
        private const string ERR_WHEEL_ADDITION_CALL = "Cannot add more than 4 wheels";
        private const string ERR_WHEEL_REMOVAL_CALL = "Cannot remove from none wheels";
        private const int MAX_DOORS = 6;
        private const string ERR_DOOR_NUMBER_OVER_MAX = "You cannot have more than 6 doors";
        private const string ERR_DOOR_NUMBER_UNDER0 = "You cannot have less than 0 doors";
        #endregion

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
            Asserts.isTrue(this.wheelList.Count() < MAX_WHEELS, ERR_WHEEL_ADDITION_CALL);
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
            Asserts.isFalse( doorsCount >= MAX_DOORS, ERR_DOOR_NUMBER_OVER_MAX);
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

        #region "DTO Operations"
        public IVehicle import(VehicleDto vehicleDto)
        {
            List<IDoor> doors = new List<IDoor>();
            foreach (DoorDto door in vehicleDto.Doors)
            {
                doors.Add(this.convert(door));
            }

            List<IWheel> wheels = new List<IWheel>();
            foreach (WheelDto wheel in vehicleDto.Wheels)
            {
                wheels.Add(this.convert(wheel));
            }

            return new Vehicle(this.convert(vehicleDto.Engine), doors, wheels, vehicleDto.Color, this.convert(vehicleDto.Enrollment));
        }
        public VehicleDto export(IVehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto
            {
                Color = vehicle.Color,
                Engine = this.convert(vehicle.Engine),
                Enrollment = this.convert(vehicle.Enrollment)
            };
            List<DoorDto> dtoDoorsList = new List<DoorDto>();
            foreach (Door door in vehicle.Doors)
            {
                dtoDoorsList.Add(this.convert(door));
            }
            vehicleDto.Doors = dtoDoorsList.ToArray();

            List<WheelDto> dtoWheelsList = new List<WheelDto>();
            foreach (Wheel wheel in vehicle.Wheels)
            {
                dtoWheelsList.Add(this.convert(wheel));
            }
            vehicleDto.Wheels = dtoWheelsList.ToArray();

            return vehicleDto;
        }
        private IEngine convert(EngineDto engineDto)
        {
            return new Engine(engineDto.HorsePower, engineDto.IsStarted);
        }
        private EngineDto convert(IEngine engine)
        {
            return new EngineDto
            {
                HorsePower = engine.HorsePower,
                IsStarted = engine.IsStarted
            };
        }
        private IDoor convert(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }
        private DoorDto convert(IDoor door)
        {
            return new DoorDto
            {
                IsOpen = door.IsOpen,
            };
        }
        private IWheel convert(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Pressure);
        }
        private WheelDto convert(IWheel wheel)
        {
            return new WheelDto
            {
                Pressure = wheel.Pressure,
            };
        }
        private IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }
        private EnrollmentDto convert(IEnrollment enrollment)
        {
            return new EnrollmentDto
            {
                Serial = enrollment.Serial,
                Number = enrollment.Number
            };
        }
        #endregion

        public IEnrollment import(string serial, int number)
        {
            return this.enrollmentProvider.import(serial, number);
        }
    }
}