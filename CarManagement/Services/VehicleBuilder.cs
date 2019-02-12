using System;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using System.Collections.Generic;
using CarManagement.Core;

namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private readonly IEnrollmentProvider enrollmentProvider;

        private int wheels;
        private int doors;
        private int horsepowerValue;
        private CarColor vehicleColor;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.wheels = 0;
            this.doors = 0;
            this.horsepowerValue = 0;
            this.vehicleColor = CarColor.White;
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.wheels < 4);
            this.wheels++;
        }

        public void setDoors(int doorsCount)
        {
            Asserts.isTrue(doorsCount >= 0 && doorsCount <= 6);
            this.doors = doorsCount;
        }

        public void setEngine(int horsePower)
        {
            Asserts.isTrue(horsePower >= 1);
            this.horsepowerValue = horsePower;
        }

        public void setColor(CarColor color)
        {
            Asserts.isEnumDefined(color);
            this.vehicleColor = color;
        }

        public IVehicle build()
        {

            List<IWheel> wheelsList = new List<IWheel>();

            if (this.wheels > 0 && this.wheels <= 4)
            {
                for (int i = 1; i <= this.wheels; i++)
                {
                    Wheel newWheel = new Wheel();
                    wheelsList.Add(newWheel);
                }
            }
            else
            {
                throw new ArgumentException("The number maximun of wheels is 4");
            }

            List<IDoor> doorsList = new List<IDoor>();

            if (this.doors <= 6)
            {
                for (int i = 1; i <= this.doors; i++)
                {
                    Door newDoor = new Door(false);
                    doorsList.Add(newDoor);
                }
            }
            else
            {
                throw new ArgumentException("The number maximun of doors is 6");
            }

            Engine engine = new Engine(this.horsepowerValue);

            IEnrollment enrollment = this.enrollmentProvider.getNew();

            CarColor carColor = this.vehicleColor;

            Vehicle vehicle = new Vehicle(wheelsList, doorsList, engine, enrollment, carColor);
            return vehicle;
        }

        public void removeWheel()
        {
            if (this.wheels > 0)
            {
                this.wheels--;
            }
            else
            {
                throw new ArgumentException("The number maximun of wheels is 4");
            }
        }

        public IVehicle import(VehicleDto vehicleDto)
        {
            throw new NotImplementedException();
        }

        public VehicleDto export(IVehicle vehicleDto)
        {
            throw new NotImplementedException();
        }

        private static IEngine convert(EngineDto engineDto)
        {
            Engine engine = new Engine(engineDto.HorsePower, engineDto.IsStarted);
            return engine;
        }

        private static EngineDto convert(IEngine engine)
        {
            EngineDto engineDto = new EngineDto
            {
                HorsePower = engine.HorsePower,
                IsStarted = engine.IsStarted
            };

            return engineDto;
        }

        private static IVehicle convert(VehicleDto vehicleDto, IEnrollmentProvider enrollmentProvider)
        {
            List<IWheel> wheels = new List<IWheel>(vehicleDto.Wheels.Length);
            List<IDoor> doors = new List<IDoor>(vehicleDto.Doors.Length);

            foreach (WheelDto wheel in vehicleDto.Wheels)
            {
                wheels.Add(convert(wheel));
            }

            foreach (DoorDto door in vehicleDto.Doors)
            {
                doors.Add(convert(door));
            }

            IEngine engine = convert(vehicleDto.Engine);

            IEnrollment enrollment = convert(vehicleDto.Enrollment, enrollmentProvider);

            Vehicle vehicle = new Vehicle(wheels, doors, engine, enrollment, vehicleDto.Color);

            return vehicle;
        }

        private static VehicleDto convert(IVehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto
            {
                Wheels = new WheelDto[vehicle.Wheels.Length],
                Doors = new DoorDto[vehicle.Doors.Length],
                Engine = convert(vehicle.Engine),
                Color = vehicle.Color,
                Enrollment = convert(vehicle.Enrollment)
            };

            for (int i = 0; i < vehicle.Wheels.Length; i++)
            {
                vehicleDto.Wheels[i] = convert(vehicle.Wheels[i]);
            }

            for (int i = 0; i < vehicle.Doors.Length; i++)
            {
                vehicleDto.Doors[i] = convert(vehicle.Doors[i]);
            }

            return vehicleDto;
        }

        private static IDoor convert(DoorDto doorDto)
        {
            Door door = new Door(doorDto.IsOpen);

            return door;
        }

        private static DoorDto convert(IDoor door)
        {
            DoorDto doorDto = new DoorDto
            {
                IsOpen = door.IsOpen
            };
            return doorDto;
        }

        private static IWheel convert(WheelDto wheelDto)
        {
            Wheel wheel = new Wheel(wheelDto.Pressure);
            return wheel;
        }

        private static WheelDto convert(IWheel wheel)
        {
            WheelDto wheelDto = new WheelDto
            {
                Pressure = wheel.Pressure
            };
            return wheelDto;
        }

        private static IEnrollment convert(EnrollmentDto enrollmentDto, IEnrollmentProvider enrollmentProvider)
        {
            IEnrollment enrollment = enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
            return enrollment;
        }

        private static EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto
            {
                Serial = enrollment.Serial,
                Number = enrollment.Number
            };

            return enrollmentDto;
        }

        private class Door : IDoor
        {
            private bool isOpen;

            public Door()
            {
                this.isOpen = false;
            }
            public Door(bool isOpen)
            {
                this.isOpen = isOpen;
            }

            public bool IsOpen
            {
                get
                {
                    return this.isOpen;
                }
            }

            public void open()
            {
                Asserts.isTrue(this.isOpen == false);
                this.isOpen = true;
            }

            public void close()
            {
                this.isOpen = false;
            }
        }

        private class Engine : IEngine
        {
            private bool isStarted;
            private double horsepower;

            public Engine(double horsepower)
            {
                Asserts.isTrue(horsepower >= 1);
                this.horsepower = horsepower;
            }

            public Engine(double horsepower, bool isStarted)
            {
                this.horsepower = horsepower;
                this.isStarted = isStarted;
            }

            public bool IsStarted
            {
                get
                {
                    return this.isStarted;
                }
            }

            public int HorsePower
            {
                get
                {
                    return (int)this.horsepower;
                }
            }

            public void start()
            {
                Asserts.isTrue(this.isStarted == false);
                this.isStarted = true;
            }

            public void setHorsePower(double nHorsePower)
            {
                Asserts.isTrue(this.horsepower >= 1);
                this.horsepower = nHorsePower;
            }

            public void stop()
            {
                Asserts.isTrue(this.isStarted = false);
                this.isStarted = false;
            }
        }

        private class Wheel : IWheel
        {
            private double pressure;
            public double Pressure
            {
                set
                {
                    this.pressure = value;
                }
                get
                {
                    return this.pressure;
                }
            }

            public Wheel()
            {
                this.pressure = 0;
            }

            public Wheel(double pressure)
            {
                this.Pressure = pressure;
            }
        }

        private class Vehicle : IVehicle
        {
            private readonly List<IWheel> wheels;
            private readonly List<IDoor> doors;

            public Vehicle(List<IWheel> wheels, List<IDoor> doors, IEngine engine, IEnrollment enrollment, CarColor carColor)
            {
                if (doors.Count >= 0 && doors.Count <= 6)
                {
                    this.doors = doors;
                }
                if (wheels.Count > 0 && wheels.Count <= 4)
                {
                    this.wheels = wheels;
                }

                this.Engine = engine;
                this.Enrollment = enrollment;
            }

            public IEngine Engine { get; }

            public IEnrollment Enrollment { get; }

            public IWheel[] Wheels
            {
                get
                {
                    return this.wheels.ToArray();
                }
            }

            public IDoor[] Doors
            {
                get
                {
                    return this.doors.ToArray();
                }
            }

            public CarColor Color { get; }

            public void setWheelsPressure(double pression)
            {
                if (pression >= 0)
                {
                    foreach (Wheel iterWheel in this.wheels)
                    {
                        iterWheel.Pressure = pression;
                    }
                }
                else
                {
                    throw new ArgumentException("Pression must be greater than 0.");
                }

            }
        }
    }
}