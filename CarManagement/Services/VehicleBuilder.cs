using System;
using System.Collections.Generic;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Builders
{
    public class VehicleBuilder : IVehicleBuilder
    {
        public class Engine : IEngine
        {
            private bool isStart;
            private int horsepower;

            public int HorsePower
            {
                get
                {
                    return this.horsepower;
                }
                set
                {
                    this.horsepower = value;
                }
            }
            public bool IsStarted
            {
                get
                {
                    return this.isStart;
                }
                set
                {
                    this.isStart = value;
                }
            }

            public void start()
            {
                this.isStart = true;
            }

            public void stop()
            {
                this.isStart = false;
            }
        }
        public class Wheel : IWheel
        {
            private double pressure;
            public double Pressure
            {
                get
                {
                    return this.pressure;
                }
                set
                {
                    this.pressure = value;
                }
            }
        }
        public class Door : IDoor
        {
            private bool isOpen;

            public void open()
            {
                this.isOpen = true;
            }
            public void close()
            {
                this.isOpen = false;
            }
            public bool IsOpen
            {
                get
                {
                    return this.isOpen;
                }
            }
        }
        public class Vehicle : IVehicle
        {
            private List<IWheel> wheels = new List<IWheel>();
            private List<IDoor> doors = new List<IDoor>();
            private int doorsCount;
            private int wheelsCount;
            public CarColor Color { set; get; }
            public IEngine Engine { get; set; }
            public IEnrollment Enrollment { get; }

            public Vehicle(List<IWheel> wheels, List<IDoor> doors, IEngine engine, CarColor color, IEnrollment enrollment)
            {
                this.wheels = wheels;
                this.doors = doors;
                this.Engine = engine;
                this.Color = color;
                this.Enrollment = enrollment;
            }

            public int DoorsCount
            {
                get
                {
                    return this.doors.Count;
                }
                set
                {
                    this.doorsCount = value;
                }
            }

            public int WheelCount
            {
                get
                {
                    return this.wheels.Count;
                }
                set
                {
                    this.wheelsCount = value;
                }
            }

            public IDoor[] Doors
            {
                get
                {
                    return this.doors.ToArray();
                }
            }

            public IWheel[] Wheels
            {
                get
                {
                    return this.wheels.ToArray();
                }
            }
            public void setWheelsPressure(double pression)
            {
                Asserts.isTrue(pression > 0);
                foreach (Wheel wheel in this.wheels)
                {
                    wheel.Pressure = pression;
                }
            }
        }
        private IEnrollmentProvider enrollmentProvider;
        private int doorsCount;
        private int wheelCounter = 0;
        private CarColor color;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.wheelCounter < 4);
            this.wheelCounter++;
        }
        public void removeWheel()
        {
            this.wheelCounter--;
            Asserts.isTrue(this.wheelCounter >= 0);
        }

        public void setDoors(int doorsCount)
        {
            Asserts.isTrue(doorsCount >= 0 && doorsCount <= 6);
            this.doorsCount = doorsCount;
        }

        Engine engine = new Engine();
        public void setEngine(int horsePorwer)
        {
            Asserts.isTrue(horsePorwer >= 0);
            this.engine.HorsePower = horsePorwer;
        }

        public VehicleDto export(IVehicle vehicleDto)
        {
            throw new NotImplementedException();
        }

        public IVehicle import(VehicleDto vehicleDto)
        {
            throw new NotImplementedException();
        }
        public void setColor(CarColor color)
        {
            this.color = color;
        }
        IVehicle IVehicleBuilder.build()
        {
            Asserts.isTrue(this.wheelCounter > 0);
            CarColor color = new CarColor();
            List<IWheel> wheels = new List<IWheel>();
            List<IDoor> doors = new List<IDoor>();
            Engine engine = new Engine();
            IEnrollment enrollment = this.enrollmentProvider.getNew();
            for (int i = 0; i < this.wheelCounter; i++)
            {
                Wheel wheel = new Wheel();
                wheels.Add(wheel);
            }
            for (int i = 0; i < this.doorsCount; i++)
            {
                Door door = new Door();
                doors.Add(door);
            }
            Vehicle vehicle = new Vehicle(wheels, doors, engine, color, enrollment);
            return vehicle;
        }
    }
}