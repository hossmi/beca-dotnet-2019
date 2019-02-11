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
        private class Engine : IEngine
        {
            private bool isStart;
            private int horsepower;
            public Engine()
            {
                this.horsepower = 1;
            }

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
                Asserts.isTrue(this.isStart == false);
                this.isStart = true;
            }

            public void stop()
            {
                Asserts.isTrue(this.isStart == true);
                this.isStart = false;
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
            private bool isOpen;
            public Door()
            {
                this.isOpen = false;
            }

            public void open()
            {
                Asserts.isTrue(this.isOpen == false);
                this.isOpen = true;
            }
            public void close()
            {
                Asserts.isTrue(this.isOpen == true);
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

        private class Vehicle : IVehicle
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

            public Vehicle()
            {
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
                Asserts.isTrue(pression >= 0);
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
        private int hp;

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
            Asserts.isTrue(horsePorwer > 0);
            this.hp = horsePorwer;
        }

        public VehicleDto export(IVehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto();
            vehicleDto = convert(vehicle);
            return vehicleDto;
        }

        public IVehicle import(VehicleDto vehicleDto)
        {
            IVehicle vehicle = new Vehicle();
            vehicle = convert(vehicleDto);
            return vehicle;
        }
        public void setColor(CarColor color)
        {

            this.color = color;
            bool t = false;
            foreach (CarColor c in Enum.GetValues(typeof(CarColor)))
            {
                if (color == c)
                {
                    t = true;
                }
            }
            Asserts.isTrue(t == true);
        }
        public IVehicle build()
        {
            Asserts.isTrue(this.wheelCounter > 0);
            CarColor color = new CarColor();
            List<IWheel> wheels = new List<IWheel>();
            List<IDoor> doors = new List<IDoor>();
            Engine engine = new Engine();
            engine.HorsePower = this.hp;
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
        public IEngine convert(EngineDto engineDto)
        {
            Engine engine = new Engine();
            engine.HorsePower = engineDto.HorsePower;
            engine.IsStarted = engineDto.IsStarted;
            return engine;
        }

        public EngineDto convert(IEngine engine)
        {
            EngineDto engineDto = new EngineDto();
            engineDto.HorsePower = engine.HorsePower;
            engineDto.IsStarted = engine.IsStarted;
            return engineDto;
        }

        public IVehicle convert(VehicleDto vehicleDto)
        {
            CarColor color = new CarColor();
            List<IWheel> wheels = new List<IWheel>();
            List<IDoor> doors = new List<IDoor>();
            IEngine engine = new Engine();
            IEnrollment enrollment = this.enrollmentProvider.getNew();

            for (int i = 0; i < vehicleDto.Wheels.Length; i++)
            {
                wheels.Add(convert(vehicleDto.Wheels[i]));
            }
            for (int i = 0; i < vehicleDto.Doors.Length; i++)
            {
                doors.Add(convert(vehicleDto.Doors[i]));
            }
            engine = convert(vehicleDto.Engine);
            color = vehicleDto.Color;
            enrollment = convert(vehicleDto.Enrollment);
            Vehicle vehicle = new Vehicle(wheels, doors, engine, color, enrollment);
            return vehicle;
        }

        public VehicleDto convert(IVehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto();
            vehicleDto.Color = new CarColor();
            vehicleDto.Doors = new DoorDto[vehicle.Doors.Length];
            vehicleDto.Wheels = new WheelDto[vehicle.Wheels.Length];
            vehicleDto.Enrollment = new EnrollmentDto();
            vehicleDto.Engine = new EngineDto();

            for (int i = 0; i < vehicle.Wheels.Length; i++)
            {
                vehicleDto.Wheels[i] = convert(vehicle.Wheels[i]);
            }

            for (int i = 0; i < vehicle.Doors.Length; i++)
            {
                vehicleDto.Doors[i] = convert(vehicle.Doors[i]);
            }

            vehicleDto.Color = vehicle.Color;
            vehicleDto.Enrollment = convert(vehicle.Enrollment);
            vehicleDto.Engine = convert(vehicle.Engine);
            return vehicleDto;
        }

        public IDoor convert(DoorDto doorDto)
        {
            IDoor door = new Door();
            if (doorDto.IsOpen == true)
            {
                door.open();
            }
            return door;
        }

        public DoorDto convert(IDoor door)
        {
            DoorDto doorDto = new DoorDto();
            doorDto.IsOpen = door.IsOpen;
            return doorDto;
        }

        public IWheel convert(WheelDto wheelDto)
        {
            IWheel wheel = new Wheel();
            wheel.Pressure = wheelDto.Pressure;
            return wheel;
        }

        public WheelDto convert(IWheel wheel)
        {
            WheelDto wheelDto = new WheelDto();
            wheelDto.Pressure = wheel.Pressure;
            return wheelDto;
        }

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            enrollmentDto.Number = enrollment.Number;
            enrollmentDto.Serial = enrollment.Serial;
            return enrollmentDto;
        }
    }
}