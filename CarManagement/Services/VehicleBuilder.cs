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
        private int doorsCount = 0;
        private int wheelCount = 0;
        public void addWheel()
        {
            void addWheel()
            {
                Asserts.isTrue(this.wheelCount < 4);
                this.wheelCount++;
            }
        }
        public class Engine
        {
            private bool isstart;
            private int horsePower;
            public int HorsePower { get; }
            public bool IsStarted
            {
                get
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
        public class Wheel
        {
            private double Pression;
            public double Pressure
            {
                get
                {
                    return this.Pression;
                }
                set
                {
                    this.Pression = value;
                }
            }
        }
        public class Door
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
        public class Vehicle
        {
            private List<Wheel> Lwheels = new List<Wheel>();
            private List<Door> Ldoors = new List<Door>();
            public CarColor Color = new CarColor();
            public Engine engine = new Engine();
            public IEnrollment Enrollment;

            public Vehicle(List<Wheel> wheels, List<Door> doors, Engine engine, CarColor color, IEnrollment enrollment)
            {
                this.Lwheels = wheels;
                this.Ldoors = doors;
                this.engine = engine;
                this.Color = color;
                this.Enrollment = enrollment;
            }
            public int DoorsCount
            {
                get
                {
                    return this.Ldoors.Count;
                }
                set
                {
                    doorsCount = value;
                }
            }
            /*public IWheel[] Wheels
            {
                get
                {
                    return this.Lwheels.ToArray();
                }
            }*/
            public int WheelCount
            {
                get
                {
                    return this.Lwheels.Count;
                }
                set
                {
                    this.wheelsCount = value;
                }
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
            
            Asserts.isTrue(this.horsePower >= 0);
            this.engine.HorsePower = this.horsePower;
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
/*public class VehicleBuilder : IVehicleBuilder
{
    public void addWheel()
    {
        throw new NotImplementedException();
    }

    public IVehicle build()
    {
        throw new NotImplementedException();
    }

    public VehicleDto export(IVehicle vehicleDto)
    {
        throw new NotImplementedException();
    }

    public IVehicle import(VehicleDto vehicleDto)
    {
        throw new NotImplementedException();
    }

    public void removeWheel()
    {
        throw new NotImplementedException();
    }

    public void setColor(CarColor color)
    {
        throw new NotImplementedException();
    }

    public void setDoors(int doorsCount)
    {
        throw new NotImplementedException();
    }

    public void setEngine(int horsePorwer)
    {
        throw new NotImplementedException();
    }
}*/
}