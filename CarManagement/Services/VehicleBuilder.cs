using System;
using System.Collections.Generic;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        public class Engine : IEngine
        {
            private bool isstart;
            private int horsePower;
            public int HorsePower {
                get
                {
                    return this.horsePower;
                }
            }

            public bool IsStarted { get
                {
                    return this.isstart;
                }
            }

            public void start()
            {
                this.isstart = true;
            }

            public void stop()
            {
                this.isstart = false;
            }
        }
        public class Door : IDoor
        {
            private bool isOpen;
            public bool IsOpen
            {
                get
                {
                    return this.isOpen;
                }
            }

            public void close()
            {
                this.isOpen = false;
            }

            public void open()
            {
                this.isOpen = true;
            }
        }
        public class Wheel : IWheel
        {
            public double Pressure { get; set; }
        }
        public class Vehicle : IVehicle
        {
            public CarColor Color { get; }

            public IDoor[] Doors { get; }

            public IEngine Engine { get; }

            public IEnrollment Enrollment { get; }

            public IWheel[] Wheels { get; }
        }

        private Door door;
        private List<Wheel> wheels;
        private List<Door> doors;
        private Engine engine;
        private CarColor color;
        private int horsePorwer;
        private IEnrollmentProvider enrollmentProvider;
        private IEnrollment enrollment;
        private int doorsCount;
        private int wheelCounter = 0;

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

        public void setEngine(int horsePorwer)
        {
            Asserts.isTrue(horsePorwer >= 0);
            this.horsePorwer = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            this.color = color;
        }

        public IVehicle build()
        {
            Asserts.isTrue(this.wheelCounter > 0);
            this.wheels = new List<Wheel>();
            this.doors = new List<Door>();
            this.engine = new Engine();
            this.enrollment = this.enrollmentProvider.getNew();
            for (int i = 0; i < this.wheelCounter; i++)
            {
                Wheel wheel = new Wheel();
                this.wheels.Add(wheel);
            }
            for (int i = 0; i < this.doorsCount; i++)
            {
                Door door = new Door();
                this.doors.Add(door);
            }
            Vehicle vehicle = new Vehicle(this.wheels, this.doors, this.engine, this.color, this.enrollment);
            return vehicle;
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