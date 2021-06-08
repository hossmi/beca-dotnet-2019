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
            return new VehicleDto(
                vehicle.Color, 
                new EngineDto(vehicle.Engine.IsStarted, vehicle.Engine.HorsePower), 
                new EnrollmentDto(vehicle.Enrollment.Serial, vehicle.Enrollment.Number),
                (WheelDto[])makeArray(vehicle.Wheels.Length, "wheel", vehicle),
                (DoorDto[])makeArray(vehicle.Doors.Length, "door", vehicle)
                );
        }
        private object[] makeArray(int length, string type, IVehicle vehicle)
        {
            object[] result = new object[length];
            for (int i = 0; i < length; i++)
            {
                if (type == "door")
                {
                    result = new DoorDto[length];
                    result[i] = new DoorDto(vehicle.Doors[i].IsOpen);
                }
                else
                {
                    result = new WheelDto[length];
                    result[i] = new WheelDto(vehicle.Wheels[i].Pressure);
                }
            }
            return result;
        }
        public IVehicle import(VehicleDto vehicleDto)
        {
            IList<IWheel> wheels = new List<IWheel>();
            IList<IDoor> doors = new List<IDoor>();

            foreach (WheelDto wheelDto in vehicleDto.Wheels)
            {
                wheels.Add
                (
                    new Wheel()
                    {
                        Pressure = wheelDto.Pressure
                    }
                );
            }
            foreach (DoorDto doorDto in vehicleDto.Doors)
            {
                doors.Add
                (
                    new Door()
                    {
                        IsOpen = doorDto.IsOpen
                    }
                );
            }
            return new Vehicle()
            {
                Wheels = wheels.ToArray(),
                Doors = doors.ToArray(),
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
            private double pressure;
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
            public bool IsOpen { get; set; }
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
        }
        private class Vehicle : IVehicle
        {
            private List<IDoor> doors;
            private List<IWheel> wheels;
            public IEngine Engine { get; set; }
            public IEnrollment Enrollment { get; set; }
            public CarColor Color { get; set; }
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
        }
    }
}