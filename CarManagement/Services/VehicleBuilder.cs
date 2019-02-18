using System;

using System.Collections.Generic;

using CarManagement.Services;

using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;


namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private readonly IEnrollmentProvider enrollmentProvider;

        private int numWheels;
        private int numDoors;
        private int horsePower;
        private CarColor color;
        private IEnrollment enrollment;

        public VehicleBuilder()
        {
            this.numWheels = 0;
            this.numDoors = 0;
            this.horsePower = 0;
            this.color = CarColor.Red;

        }

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            for (int i = 0; i <= 4; i++)
            {
                this.numWheels++;
            }
        }
        public void removeWheel()
        {
            this.numWheels--;
        }
        public void setColor(CarColor color)
        {
            this.color = color;
        }
        public void setDoors(int doorsCount)
        {
            this.numDoors = doorsCount;
        }
        public void setEngine(int horsePower)
        {
            this.horsePower = horsePower;
        }


        private class Engine : IEngine
        {
            public int horsePower;
            public bool isStarted;
            public int HorsePower
            {
                get
                {
                    return this.horsePower;
                }
            }

            public bool IsStarted
            {
                get
                {
                    return this.isStarted;
                }
            }

            public Engine(int horsePower)
            {
                this.horsePower = horsePower;
                this.isStarted = false;

            }

            public Engine(int horsePower, bool isStarted)
            {
                this.horsePower = horsePower;
                this.isStarted = isStarted;

            }

            public void start()
            {
                this.isStarted = true;
            }
            public void stop()
            {
                this.isStarted = false;
            }
        }
        private class Wheel : IWheel
        {
            public double pressure;

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
                    this.pressure = value;
                }
            }

        }
        private class Door : IDoor
        {
            public bool isOpen;

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
        private class Enrollment : IEnrollment
        {
            public string serial;
            public int number;

            public Enrollment()
            {

            }

            public string Serial
            {
                get
                {
                    return this.serial;
                }
            }

            public int Number
            {
                get
                {
                    return this.number;
                }
            }
        }
        private class Vehicle : IVehicle
        {
            private List<IWheel> wheel;
            private List<IDoor> door;
            private IEngine engine;
            private CarColor color;
            private IEnrollment enrollment;

            public Vehicle(List<IWheel> wheel, List<IDoor> door, IEngine engine, CarColor color, IEnrollment enrollment)
            {
                this.wheel = wheel;
                this.door = door;
                this.engine = engine;
                this.color = color;
                this.enrollment = enrollment;
            }

            public CarColor Color
            {
                get
                {
                    return this.Color;
                }
            }
            public IDoor[] Doors
            {
                get
                {
                    return this.Doors;
                }
            }
            public IEngine Engine
            {
                get
                {
                    return this.Engine;
                }
            }
            public IEnrollment Enrollment
            {
                get
                {
                    return this.Enrollment;
                }
            }
            public IWheel[] Wheels
            {
                get
                {
                    return this.Wheels;
                }
            }
        }


        public IVehicle build()
        {

            IEngine engine = new Engine(this.horsePower);

            CarColor color = new CarColor();

            List<IDoor> doors = new List<IDoor>();
            for (int i = 0; i < this.numDoors; i++)
            {
                Door d = new Door();
                doors.Add(d);
            }

            List<IWheel> wheels = new List<IWheel>();
            for (int i = 0; i < this.numWheels; i++)
            {
                Wheel w = new Wheel();
                wheels.Add(w);
            }

            IEnrollment enrollment = new Enrollment();

            Vehicle v = new Vehicle(wheels, doors, engine, color, enrollment);
            return v;
        }


        public IVehicle import(VehicleDto vehicleDto)
        {
            throw new NotImplementedException();
        }

        public VehicleDto export(IVehicle vehicleDto)
        {
            throw new NotImplementedException();
        }

        public IEnrollment import(string serial, int number)
        {
            return this.enrollmentProvider.import(serial, number);
        }
    }
}


