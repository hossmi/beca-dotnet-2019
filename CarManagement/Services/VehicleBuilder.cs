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
        private IEnrollmentProvider enrollmentProvider;
        private int doorsCount;
        private int wheelCount;
        private CarColor color;
        private int power;
        private IEngine engine;
        private VehicleDto vehicleDto;
        private IVehicle vehicle;
        private bool checkColor;
        private List<IWheel> wheels;
        private List<IDoor> doors;
        private IEnrollment enrollment;
        private IWheel wheel;
        private IDoor door;
        private EngineDto engineDto;
        private DoorDto doorDto;
        private WheelDto wheelDto;
        private EnrollmentDto enrollmentDto;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.engine = new Engine();
            this.wheelCount = 0;
            this.checkColor = false;
            this.color = new CarColor();
            this.wheels = new List<IWheel>();
            this.doors = new List<IDoor>();
            this.doorsCount = 0;
            this.power = 0;
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
        public VehicleDto export(IVehicle vehicle)
        {
            this.vehicleDto = new VehicleDto();
            this.vehicleDto = convert(vehicle);
            return this.vehicleDto;
        }
        public IVehicle import(VehicleDto vehicleDto)
        {
            this.vehicle = convert(vehicleDto);
            return this.vehicle;
        }
        public void setColor(CarColor color)
        {
            this.color = color;
            foreach (CarColor carColor in Enum.GetValues(typeof(CarColor)))
            {
                if (color == carColor)
                {
                    this.checkColor = true;
                }
            }
            Asserts.isTrue(this.checkColor == true);
        }
        public IVehicle build()
        {
            Asserts.isTrue(this.wheelCount > 0);
            Engine engineSet = new Engine();
            engineSet.setEngine(this.power);
            this.enrollment = this.enrollmentProvider.getNew();
            for (int i = 0; i < this.wheelCount; i++)
            {
                this.wheel = new Wheel();
                this.wheels.Add(this.wheel);
            }
            for (int i = 0; i < this.doorsCount; i++)
            {
                this.door = new Door();
                this.doors.Add(this.door);
            }
            this.vehicle = new Vehicle(this.wheels, this.doors, engineSet, this.color, this.enrollment);
            return this.vehicle;
        }
        public IEnrollment import(string serial, int number)
        {
            return this.enrollmentProvider.import(serial, number);
        }

        private IEngine convert(EngineDto engineDto)
        {
            Engine engine = new Engine();
            engine.setEngine(engineDto.HorsePower);
            engine.IsStarted = engineDto.IsStarted;
            return engine;
        }
        private EngineDto convert(IEngine engine)
        {
            this.engineDto = new EngineDto();
            this.engineDto.HorsePower = engine.HorsePower;
            this.engineDto.IsStarted = engine.IsStarted;
            return this.engineDto;
        }
        private IVehicle convert(VehicleDto vehicleDto)
        {
            this.enrollment = this.enrollmentProvider.getNew();

            for (int i = 0; i < vehicleDto.Wheels.Length; i++)
            {
                this.wheels.Add(convert(vehicleDto.Wheels[i]));
            }
            for (int i = 0; i < vehicleDto.Doors.Length; i++)
            {
                this.doors.Add(convert(vehicleDto.Doors[i]));
            }
            this.engine = convert(vehicleDto.Engine);
            this.color = vehicleDto.Color;
            this.enrollment = convert(vehicleDto.Enrollment);
            this.vehicle = new Vehicle(this.wheels, this.doors, this.engine, this.color, this.enrollment);
            return this.vehicle;
        }
        private VehicleDto convert(IVehicle vehicle)
        {
            this.vehicleDto = new VehicleDto();
            this.vehicleDto.Color = new CarColor();
            this.vehicleDto.Doors = new DoorDto[vehicle.Doors.Length];
            this.vehicleDto.Wheels = new WheelDto[vehicle.Wheels.Length];
            this.vehicleDto.Enrollment = new EnrollmentDto();
            this.vehicleDto.Engine = new EngineDto();

            for (int i = 0; i < vehicle.Wheels.Length; i++)
            {
                this.vehicleDto.Wheels[i] = convert(vehicle.Wheels[i]);
            }

            for (int i = 0; i < vehicle.Doors.Length; i++)
            {
                this.vehicleDto.Doors[i] = convert(vehicle.Doors[i]);
            }

            this.vehicleDto.Color = vehicle.Color;
            this.vehicleDto.Enrollment = convert(vehicle.Enrollment);
            this.vehicleDto.Engine = convert(vehicle.Engine);
            return this.vehicleDto;
        }
        private IDoor convert(DoorDto doorDto)
        {
            this.door = new Door();
            if (doorDto.IsOpen == true)
            {
                this.door.open();
            }
            return this.door;
        }
        private DoorDto convert(IDoor door)
        {
            this.doorDto = new DoorDto();
            this.doorDto.IsOpen = door.IsOpen;
            return this.doorDto;
        }
        private IWheel convert(WheelDto wheelDto)
        {
            this.wheel = new Wheel();
            this.wheel.Pressure = wheelDto.Pressure;
            return this.wheel;
        }
        private WheelDto convert(IWheel wheel)
        {
            this.wheelDto = new WheelDto();
            this.wheelDto.Pressure = wheel.Pressure;
            return this.wheelDto;
        }
        private IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }
        private EnrollmentDto convert(IEnrollment enrollment)
        {
            this.enrollmentDto = new EnrollmentDto();
            this.enrollmentDto.Number = enrollment.Number;
            this.enrollmentDto.Serial = enrollment.Serial;
            return this.enrollmentDto;
        }

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
            public void setEngine(int horsePorwer)
            {
                Asserts.isTrue(horsePorwer > 0);
                this.horsepower = horsePorwer;
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
            private IEngine engine;
            private List<IDoor> doors;
            private List<IWheel> wheels;
            private CarColor color;
            private IEnrollment enrollment;

            public Vehicle(List<IWheel> wheels, List<IDoor> doors, IEngine engine, CarColor color, IEnrollment enrollment)
            {
                this.engine = engine;
                this.doors = doors;
                this.color = color;
                this.wheels = wheels;
                this.enrollment = enrollment;
            }

            public IDoor[] Doors
            {
                get
                {
                    return this.doors.ToArray();
                }
            }
            public IEngine Engine
            {
                get
                {
                    return this.engine;
                }
            }
            public IEnrollment Enrollment
            {
                get
                {
                    return this.enrollment;
                }
            }
            public IWheel[] Wheels
            {
                get
                {
                    return this.wheels.ToArray();
                }
            }
            public CarColor Color
            {
                get
                {
                    return this.color;
                }
            }
        }

        public EngineDto engineDtoBuilder(bool isStarted, int horsePower)
        {
            this.engineDto = new EngineDto();
            this.engineDto.IsStarted = isStarted;
            this.engineDto.HorsePower = horsePower;
            return this.engineDto;
        }
        public EnrollmentDto enrollmentDtoBuilder(string serial, int number)
        {
            this.enrollmentDto = new EnrollmentDto();
            this.enrollmentDto.Serial = serial;
            this.enrollmentDto.Number = number;
            return this.enrollmentDto;
        }
        public WheelDto wheelDtoBuilder(double pressure)
        {
            this.wheelDto = new WheelDto();
            this.wheelDto.Pressure = pressure;
            return this.wheelDto;
        }
        public DoorDto doorDtoBuilder(bool isOpen)
        {
            this.doorDto = new DoorDto();
            this.doorDto.IsOpen = isOpen;
            return this.doorDto;
        }
        public VehicleDto vehicleDtoBuilder(EnrollmentDto enrollmentDto, EngineDto engineDto, CarColor color, WheelDto[] wheelsDto, DoorDto[] doorsDto)
        {
            this.vehicleDto = new VehicleDto();
            this.vehicleDto.Color = color;
            this.vehicleDto.Enrollment = enrollmentDto;
            this.vehicleDto.Engine = engineDto;
            this.vehicleDto.Doors = doorsDto;
            this.vehicleDto.Wheels = wheelsDto;
            return this.vehicleDto;
        }
    }
}