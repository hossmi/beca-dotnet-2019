using System;
using System.Collections.Generic;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private readonly IEnrollmentProvider enrollmentProvider;
        private int doorsCount;
        private int wheelsCount;
        private int horsePower;
        private CarColor color;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            if (this.wheelsCount >= 4)
                throw new InvalidOperationException("Can not add more than 4 wheels.");
            this.wheelsCount++;
        }

        public void removeWheel()
        {
            if (this.wheelsCount <= 0)
                throw new InvalidOperationException("There is no wheel to remove.");
            else
                this.wheelsCount--;
        }

        public void setDoors(int doorsCount)
        {
            if (doorsCount < 0 || doorsCount > 6)
                throw new ArgumentException("Doors number must be between 0 and 6");
            else
                this.doorsCount = doorsCount;
        }

        public void setEngine(int horsePower)
        {
            if (horsePower <= 0)
                throw new ArgumentException("Horse power must be over 0.");
            else
                this.horsePower = horsePower;
        }

        public void setColor(CarColor color)
        {
            if (Enum.IsDefined(typeof(CarColor), color) == false)
                throw new ArgumentException("Color value is not valid.");
            else
                this.color = color;
        }

        public IVehicle build()
        {
            if (this.wheelsCount < 1)
                throw new ArgumentException("Can not build a vehicle without wheels.");

            Vehicle vehicle;

            Wheel[] wheels = new Wheel[this.wheelsCount];

            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i] = new Wheel();
            }

            Door[] doors = new Door[this.doorsCount];

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i] = new Door();
            }

            Engine engine = new Engine(this.horsePower);
            IEnrollment enrollment = this.enrollmentProvider.getNew();

            vehicle = new Vehicle(this.color, doors, engine, enrollment, wheels);
            return vehicle;
        }

        public IVehicle import(VehicleDto vehicleDto)
        {
            Vehicle vehicle;

            Wheel[] wheels = new Wheel[vehicleDto.Wheels.Length];


            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i] = new Wheel();
                wheels[i].Pressure = vehicleDto.Wheels[i].Pressure;
            }

            Door[] doors = new Door[vehicleDto.Doors.Length];

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i] = new Door();

                if (vehicleDto.Doors[i].IsOpen == true)
                    doors[i].open();
                else
                    doors[i].close();
            }

            CarColor color = vehicleDto.Color;

            Engine engine = new Engine(vehicleDto.Engine.HorsePower);

            if (vehicleDto.Engine.IsStarted == true)
                engine.start();

            IEnrollment enrollment = this.enrollmentProvider.import(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number);

            vehicle = new Vehicle(color, doors, engine, enrollment, wheels);
            return vehicle;
        }

        public VehicleDto export(IVehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto();

            vehicleDto.Color = vehicle.Color;

            WheelDto[] wheelsDto = new WheelDto[vehicle.Wheels.Length];

            for (int i = 0; i < wheelsDto.Length; i++)
            {
                wheelsDto[i] = new WheelDto();
                wheelsDto[i].Pressure = vehicle.Wheels[i].Pressure;
            }

            DoorDto[] doorsDto = new DoorDto[vehicle.Doors.Length];

            for (int i = 0; i < doorsDto.Length; i++)
            {
                doorsDto[i] = new DoorDto();

                if (vehicle.Doors[i].IsOpen == true)
                    doorsDto[i].IsOpen = true;
                else
                    doorsDto[i].IsOpen = false;
            }

            vehicleDto.Color = vehicle.Color;

            vehicleDto.Engine = new EngineDto();
            vehicleDto.Engine.HorsePower = vehicle.Engine.HorsePower;
            vehicleDto.Engine.IsStarted = vehicle.Engine.IsStarted;

            vehicleDto.Enrollment = new EnrollmentDto();
            vehicleDto.Enrollment.Serial = vehicle.Enrollment.Serial;
            vehicleDto.Enrollment.Number = vehicle.Enrollment.Number;

            vehicleDto.Doors = doorsDto;
            vehicleDto.Wheels = wheelsDto;

            return vehicleDto;
        }

        private class Door : IDoor
        {
            private bool isOpen = false;

            public bool IsOpen
            {
                get
                {
                    return this.isOpen;
                }
                private set
                {
                    this.isOpen = value;
                }
            }

            public void close()
            {
                if (this.isOpen == false)
                    throw new ArgumentException("Door is already closed.");
                else
                    this.isOpen = false;
            }

            public void open()
            {
                if (this.isOpen == true)
                    throw new ArgumentException("Door is already open.");
                else
                    this.isOpen = true;
            }
        }
        private class Wheel : IWheel
        {
            private double pressure = 1;

            public double Pressure
            {
                get
                {
                    return this.pressure;
                }
                set
                {
                    if (value < 1 || value > 5)
                        throw new ArgumentException("Wheel pressure must be between 1 and 5 atmos.");
                    else
                        this.pressure = value;
                }
            }
        }
        private class Engine : IEngine
        {
            private int horsePower;
            private bool isStarted;

            public Engine(int horsePower)
            {
                this.horsePower = horsePower;
                this.isStarted = false;
            }

            public int HorsePower
            {
                get
                {
                    return this.horsePower;
                }
                private set
                {
                    if (value <= 0)
                        throw new ArgumentException("Horse power must be over 0.");
                    else
                        this.horsePower = value;
                }
            }

            public bool IsStarted
            {
                get
                {
                    return this.isStarted;
                }
            }

            public void start()
            {
                if (this.isStarted == true)
                    throw new ArgumentException("Engine is already started.");
                else
                    this.isStarted = true;
            }

            public void stop()
            {
                if (this.isStarted == false)
                    throw new ArgumentException("Engine is already started.");
                else
                    this.isStarted = false;
            }
        }
        private class Vehicle : IVehicle
        {
            public Vehicle(CarColor color, IDoor[] doors, IEngine engine, IEnrollment enrollment, IWheel[] wheels)
            {
                this.Color = color;
                this.Doors = doors;
                this.Engine = engine;
                this.Enrollment = enrollment;
                this.Wheels = wheels;
            }

            public void setWheelsPressure(double pressure)
            {
                if (pressure <= 0)
                    throw new ArgumentException("Pressure must be over 0.");
                else
                {
                    foreach (Wheel wheel in this.Wheels)
                    {
                        wheel.Pressure = pressure;
                    }
                }
            }

            public CarColor Color { get; }

            public IDoor[] Doors { get; }

            public IEngine Engine { get; }

            public IEnrollment Enrollment { get; }

            public IWheel[] Wheels { get; }
        }
    }
}