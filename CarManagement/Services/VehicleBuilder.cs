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
        private readonly IEnrollmentProvider enrollmentProvider;
        private int doorsCount;
        private int wheelCount;
        private CarColor color;
        private int power;
        private bool checkColor;
        private List<IWheel> wheels;
        private List<IDoor> doors;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.wheelCount = 0;
            this.checkColor = false;
            this.doorsCount = 0;
            this.power = 0;
        }
        public VehicleBuilder(IEnrollmentProvider enrollmentProvider, CarColor color, int horsePower, int wheels, int doors)
        {
            this.enrollmentProvider = enrollmentProvider;
            checkColors(color);
            this.color = color;
            Asserts.isTrue(horsePower > -1);
            this.power = horsePower;
            Asserts.isTrue(doors > -1 && doors < 6);
            this.doorsCount = doors;
            Asserts.isTrue(wheels > -1 && wheels < 7);
            this.wheelCount = wheels;
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
            checkColors(color);
            this.color = new CarColor();
            this.color = color;
        }

        public IVehicle build()
        {
            Asserts.isTrue(this.wheelCount > 0);
            this.wheels = new List<IWheel>();
            this.doors = new List<IDoor>();
            for (int i = 0; i < this.wheelCount; i++)
                this.wheels.Add(new Wheel());
            for (int i = 0; i < this.doorsCount; i++)
                this.doors.Add(new Door());
            return new Vehicle(
                this.wheels, 
                this.doors, 
                new Engine(
                    this.power), 
                this.color, 
                this.enrollmentProvider.getNew());
        }
        private void checkColors(CarColor color)
        {
            foreach (CarColor carColor in Enum.GetValues(typeof(CarColor)))
            {
                if (color == carColor)
                    this.checkColor = true;
            }
            Asserts.isTrue(this.checkColor == true);
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
        public EnrollmentDto export(IEnrollment enrollment)
        {
            return convert(enrollment);
        }

        private IEngine convert(EngineDto engineDto)
        {
            return new Engine(engineDto.HorsePower, engineDto.IsStarted);
        }
        private EngineDto convert(IEngine engine)
        {
            return new EngineDto(engine.HorsePower, engine.IsStarted);
        }
        private IVehicle convert(VehicleDto vehicleDto)
        {
            this.wheels = new List<IWheel>();
            this.doors = new List<IDoor>();
            foreach (WheelDto wheelDto in vehicleDto.Wheels)
                this.wheels.Add(convert(wheelDto));
            foreach (DoorDto doorDto in vehicleDto.Doors)
                this.doors.Add(convert(doorDto));
            return new Vehicle(
                this.wheels, 
                this.doors, 
                convert(vehicleDto.Engine), 
                vehicleDto.Color, 
                convert(vehicleDto.Enrollment));
        }
        private VehicleDto convert(IVehicle vehicle)
        {
            WheelDto[] wheelsDto = new WheelDto[vehicle.Wheels.Length];
            DoorDto[] doorsDto = new DoorDto[vehicle.Doors.Length];
            for (int i = 0; i < vehicle.Wheels.Length; i++)
                wheelsDto[i] = convert(vehicle.Wheels[i]);
            for (int i = 0; i < vehicle.Doors.Length; i++)
                doorsDto[i] = convert(vehicle.Doors[i]);
            return new VehicleDto(
                vehicle.Color, 
                convert(vehicle.Engine), 
                convert(vehicle.Enrollment), 
                wheelsDto, 
                doorsDto);
        }

        /*private T[] genericConversor<T>(int counter, dynamic[] items)
        {
            T[] arrays = new T[items.Length];
            for (int i = 0; i < counter; i++)
                arrays[i] = convert(items[i]);
            return arrays;
        }*/

        private IDoor convert(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }
        private DoorDto convert(IDoor door)
        {
            return new DoorDto(door.IsOpen);
        }
        private IWheel convert(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Pressure);
        }
        private WheelDto convert(IWheel wheel)
        {
            return new WheelDto(wheel.Pressure);
        }
        private IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }
        private EnrollmentDto convert(IEnrollment enrollment)
        {
            return new EnrollmentDto(enrollment.Serial, enrollment.Number);
        }

        private class Engine : IEngine
        {
            public Engine(int horsePower, bool isStarted)
            {
                this.HorsePower = horsePower;
                this.IsStarted = isStarted;
            }
            public Engine(int horsePower)
            {
                this.HorsePower = horsePower;
            }
            public Engine()
            {
                this.HorsePower = 1;
            }

            public int HorsePower { get; }
            public bool IsStarted { get; private set; }
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
            public Wheel(double pressure)
            {
                this.pressure = pressure;
            }
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
            public Door(bool isOpen)
            {
                this.IsOpen = isOpen;
            }
            public Door()
            {
                this.IsOpen = false;
            }

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
            public bool IsOpen { get; private set; }
        }
        private class Vehicle : IVehicle
        {
            private readonly List<IDoor> doors;
            private readonly List<IWheel> wheels;
            public IEngine Engine { get; }
            public IEnrollment Enrollment { get; }
            public IWheel[] Wheels
            {
                get
                {
                    return this.wheels.ToArray();
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
            }
            public CarColor Color { get; }
        }
    }
}