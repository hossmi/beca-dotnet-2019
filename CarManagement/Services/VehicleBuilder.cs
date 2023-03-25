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
                    HorsePower = this.power
                }, 
                Color = color, 
                Enrollment = this.enrollmentProvider.getNew()
            };
        }

        public VehicleDto export(IVehicle vehicle)
        {
            return new VehicleDto(
                vehicle.Color, 
                new EngineDto(vehicle.Engine.IsStarted, vehicle.Engine.HorsePower), 
                new EnrollmentDto(vehicle.Enrollment.Serial, vehicle.Enrollment.Number),
                vehicle.Wheels.Select(x => new WheelDto() { Pressure = x.Pressure }).ToArray(),
                vehicle.Doors.Select(x => new DoorDto() { IsOpen = x.IsOpen }).ToArray()
                );
        }
        public IVehicle import(VehicleDto vehicleDto)
        {
            return new Vehicle()
            {
                Wheels = vehicleDto.Wheels.Select(x => (IWheel)new Wheel() { Pressure = x.Pressure }).ToArray(),
                Doors = vehicleDto.Doors.Select(x => (IDoor)new Door() { IsOpen = x.IsOpen }).ToArray(),
                Engine = new Engine()
                {
                    HorsePower = vehicleDto.Engine.HorsePower,
                    IsStarted = vehicleDto.Engine.IsStarted
                },
                Color = vehicleDto.Color,
                Enrollment = this.enrollmentProvider.import(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number)
            };
        }
        public IEnrollment import(string serial, int number)
        {
            return this.enrollmentProvider.import(serial, number);
        }
    }
}