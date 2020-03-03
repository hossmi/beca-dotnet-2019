using System;
using System.Collections.Generic;
using System.Linq;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private readonly IEnrollmentProvider enrollmentProvider;
        private int doorsCount;
        private int wheelCount;
        private CarColor color;
        private int power;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.wheelCount < 4);
            this.wheelCount++;
        }
        public void removeWheel()
        {
            this.wheelCount--;
            Asserts.isTrue(this.wheelCount >= 0);
        }
        public void setDoors(int doorsCount)
        {
            Asserts.isTrue(doorsCount >= 0 && doorsCount <= 6);
            this.doorsCount = doorsCount;
        }
        public void setEngine(int horsePorwer)
        {
            Asserts.isTrue(horsePorwer > 0);
            this.power = horsePorwer;
        }
        public void setColor(CarColor color)
        {
            Asserts.isTrue(Enum.IsDefined(typeof(CarColor), color));
            this.color = color;
        }

        public IVehicle build()
        {
            Asserts.isTrue(this.wheelCount > 0);
            List<IWheel> wheels = new List<IWheel>();
            List<IDoor> doors  = new List<IDoor>();
            for (int i = 0; i < this.wheelCount; i++)
            {
                wheels.Add(new Wheel());
            }
            for (int i = 0; i < this.doorsCount; i++)
            {
                doors.Add
                (
                    new Door() 
                    { 
                        IsOpen = false
                    }
                );
            }
            return new Vehicle()
            {
                Wheels = wheels.ToArray(), 
                Doors = doors.ToArray(), 
                Engine = new Engine() 
                { 
                    HorsePower = this.power, 
                    IsStarted = false
                }, 
                Color = color, 
                Enrollment = this.enrollmentProvider.getNew()
            };
        }

        public VehicleDto export(IVehicle vehicle)
        {
            return convert(vehicle);
        }
        public IVehicle import(VehicleDto vehicleDto)
        {
            return convert(vehicleDto);
        }
        public IEnrollment import(string serial, int number)
        {
            return this.enrollmentProvider.import(serial, number);
        }

        private IEngine convert(EngineDto engineDto)
        {
            return new Engine() 
            { 
                HorsePower = engineDto.HorsePower, 
                IsStarted = engineDto.IsStarted
            };
        }
        private EngineDto convert(IEngine engine)
        {
            return new EngineDto() { HorsePower = engine.HorsePower, IsStarted = engine.IsStarted };
        }
        private IVehicle convert(VehicleDto vehicleDto)
        {
            List<IWheel> wheels = new List<IWheel>();
            List<IDoor> doors = new List<IDoor>();
            foreach (WheelDto wheelDto in vehicleDto.Wheels)
            {
                wheels.Add(convert(wheelDto));
            }
            foreach (DoorDto doorDto in vehicleDto.Doors)
            {
                doors.Add(convert(doorDto));
            }
            return new Vehicle(
                wheels, 
                doors, 
                convert(vehicleDto.Engine), 
                vehicleDto.Color, 
                convert(vehicleDto.Enrollment));
        }
        private VehicleDto convert(IVehicle vehicle)
        {
            WheelDto[] wheelsDto = new WheelDto[vehicle.Wheels.Length];
            DoorDto[] doorsDto = new DoorDto[vehicle.Doors.Length];
            for (int i = 0; i < vehicle.Wheels.Length; i++)
            {
                wheelsDto[i] = convert(vehicle.Wheels[i]);
            }
            for (int i = 0; i < vehicle.Doors.Length; i++)
            {
                doorsDto[i] = convert(vehicle.Doors[i]);
            }
            return new VehicleDto()
            {
                Color = vehicle.Color,
                Engine = convert(vehicle.Engine),
                Enrollment = convert(vehicle.Enrollment),
                Wheels = wheelsDto,
                Doors = doorsDto
            };
        }

        private IDoor convert(DoorDto doorDto)
        {
            return new Door() 
            { 
                IsOpen = doorDto.IsOpen
            };
        }
        private DoorDto convert(IDoor door)
        {
            return new DoorDto() 
            { 
                IsOpen = door.IsOpen 
            };
        }
        private IWheel convert(WheelDto wheelDto)
        {
            return new Wheel() 
            { 
                Pressure = wheelDto.Pressure 
            };
        }
        private WheelDto convert(IWheel wheel)
        {
            return new WheelDto() { Pressure = wheel.Pressure };
        }
        private IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }
        private EnrollmentDto convert(IEnrollment enrollment)
        {
            return new EnrollmentDto() { Serial = enrollment.Serial, Number = enrollment.Number };
        }

        private class Engine : IEngine
        {
            public int HorsePower { get; set; }
            public bool IsStarted { get; set; }
            
            public void start()
            {
                Asserts.isTrue(this.IsStarted == false);
                this.IsStarted = true;
            }
            public void stop()
            {
                Asserts.isTrue(this.IsStarted == true);
                this.IsStarted = false;
            }
        }
        private class Wheel : IWheel
        {
            private double pressure { get; set; }
            public Wheel()
            {
                this.pressure = 1;
            }

            public double Pressure
            {
                get
                {
                    return this.pressure;
                }
                set
                {
                    Asserts.isTrue(value >= 1 && value <= 5);
                    this.pressure = value;
                }
            }
        }
        private class Door : IDoor
        {
            public void open()
            {
                Asserts.isTrue(this.IsOpen == false);
                this.IsOpen = true;
            }
            public void close()
            {
                Asserts.isTrue(this.IsOpen == true);
                this.IsOpen = false;
            }
            public bool IsOpen { get; set; }
        }
        private class Vehicle : IVehicle
        {
            private List<IDoor> doors;
            private List<IWheel> wheels;
            public IEngine Engine { get; set; }
            public IEnrollment Enrollment { get; set; }
            public IWheel[] Wheels
            {
                get
                {
                    return this.wheels.ToArray();
                }
                set 
                {
                    this.wheels = value.ToList();
                }
            }

            public Vehicle(List<IWheel> wheels, List<IDoor> doors, IEngine engine, CarColor color, IEnrollment enrollment)
            {
                this.Engine = engine;
                this.doors = doors;
                this.Color = color;
                this.wheels = wheels;
                this.Enrollment = enrollment;
            }
            public Vehicle()
            {

            }

            public IDoor[] Doors
            {
                get
                {
                    return this.doors.ToArray();
                }
                set
                {
                    this.doors = value.ToList();
                }
            }
            public CarColor Color { get; set; }
        }
    }
}