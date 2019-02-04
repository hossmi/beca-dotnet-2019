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
        private CarColor color;
        private int numberWheels;
        private int numberDoors;
        private int horsePorwer;
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.numberDoors = 0;
            this.numberWheels = 0;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.numberWheels < 4);
            this.numberWheels++;
        }

        public void removeWheel()
        {
            Asserts.isTrue(this.numberWheels > 0);
            this.numberWheels--;
        }

        public void setDoors(int doorsCount)
        {
            Asserts.isTrue(0 <= doorsCount && doorsCount < 7);
            this.numberDoors = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            Asserts.isTrue(horsePorwer > 0);
            this.horsePorwer = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            Asserts.isEnumDefined(color);
            this.color = color;
        }

        public IVehicle build()
        {
            Asserts.isTrue(0 < this.numberWheels && this.numberWheels <= 4);

            Engine engine = new Engine(this.horsePorwer);
            List<IDoor> doors = create<IDoor, Door>(this.numberDoors);
            List<IWheel> wheels = create<IWheel, Wheel>(this.numberWheels);

            Vehicle vehicle = new Vehicle(wheels, doors, engine, this.color, this.enrollmentProvider.getNew());
            return vehicle;
        }

        public IVehicle import(VehicleDto vehicleDto)
        {
            Engine engine = new Engine(vehicleDto.Engine.HorsePower, vehicleDto.Engine.IsStarted);
            List<IWheel> wheels = createWheels(vehicleDto);
            List<IDoor> doors = createDoors(vehicleDto);
            IEnrollment enrollment = this.enrollmentProvider.import(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number);

            IVehicle vehicle = new Vehicle(wheels, doors, engine, vehicleDto.Color, enrollment);

            return vehicle;
        }

        public VehicleDto export(IVehicle vehicleDto)
        {
            VehicleDto vehicleDtoFinal = new VehicleDto
            {
                Doors = createDoorsDto(vehicleDto),
                Wheels = createWheelDto(vehicleDto),
                Engine = convert(vehicleDto.Engine),
                Enrollment = convert(vehicleDto.Enrollment),
                Color = vehicleDto.Color
            };

            return vehicleDtoFinal;
        }

        private static EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto
            {
                Serial = enrollment.Serial,
                Number = enrollment.Number,
            };

            return enrollmentDto;
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

        private static IDoor convert(DoorDto doorDto)
        {
            IDoor door = new Door(doorDto.IsOpen);
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
            IWheel wheel = new Wheel
            {                
                Pressure = wheelDto.Pressure
            };

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

        private static DoorDto[] createDoorsDto(IVehicle vehicle)
        {
            DoorDto[] doorsDto = new DoorDto[vehicle.Doors.Length];
            for (int i = 0; i < vehicle.Doors.Length; i++)
            {
                doorsDto[i] = convert(vehicle.Doors[i]);
            }

            return doorsDto;
        }

        private static List<IDoor> createDoors(VehicleDto vehicleDto)
        {
            List<IDoor> doors = new List<IDoor>();
            for (int i = 0; i < vehicleDto.Doors.Length; i++)
            {
                doors.Add(convert(vehicleDto.Doors[i]));
            }

            return doors;
        }

        private static WheelDto[] createWheelDto(IVehicle vehicle)
        {
            WheelDto[] wheelsDto = new WheelDto[vehicle.Wheels.Length];
            for (int i = 0; i < vehicle.Wheels.Length; i++)
            {
                wheelsDto[i] = convert(vehicle.Wheels[i]);
            }

            return wheelsDto;
        }

        private static List<IWheel> createWheels(VehicleDto vehicleDto)
        {
            List<IWheel> wheels = new List<IWheel>();
            for (int i = 0; i < vehicleDto.Wheels.Length; i++)
            {
                wheels.Add(convert(vehicleDto.Wheels[i]));
            }

            return wheels;
        }

        private List<TInterface> create<TInterface, TInstance>(int nItems)
            where TInstance : class, TInterface, new()
            where TInterface : class
        {
            List<TInterface> items = new List<TInterface>();
            for (int i = 0; i < nItems; i++)
            {
                TInterface aux = new TInstance();
                items.Add(aux);
            }

            return items;
        }

        private class Door : IDoor
        {
            private bool openDoor;

            public Door()
            {
                this.openDoor = false;
            }

            public Door(bool openDoor)
            {
                this.openDoor = openDoor;
            }

            public void open()
            {
                Asserts.isFalse(this.openDoor);
                this.openDoor = true;
            }

            public void close()
            {
                Asserts.isTrue(this.openDoor);
                this.openDoor = false;
            }

            public bool IsOpen
            {
                get
                {
                    return this.openDoor;
                }
            }
        }

        private class Engine : IEngine
        {
            private bool startEngine;
            private int horsePower;

            public Engine(int horsePorwer)
            {
                this.startEngine = false;
                this.horsePower = horsePorwer;
            }

            public Engine(int horsePorwer, bool startEngine)
            {
                this.startEngine = startEngine;
                this.horsePower = horsePorwer;
            }

            public bool IsStarted
            {
                get
                {
                    return this.startEngine;
                }
            }

            public void start()
            {
                Asserts.isFalse(this.startEngine);
                this.startEngine = true;
            }

            public int HorsePower
            {
                get
                {
                    return this.horsePower;
                }
            }

            public void setHorsePorwer(int horsePower)
            {
                this.horsePower = horsePower;
            }

            public void stop()
            {
                Asserts.isTrue(this.startEngine);
                this.startEngine = false;
            }
        }

        private class Vehicle : IVehicle
        {
            private IEnrollment enrollment;
            private List<IWheel> wheels;
            private List<IDoor> doors;
            private Engine engine;
            private CarColor color;

            public Vehicle(List<IWheel> wheels, List<IDoor> doors, Engine engine, CarColor color, IEnrollment enrollment)
            {
                this.wheels = wheels;
                this.doors = doors;
                this.engine = engine;
                this.color = color;
                this.enrollment = enrollment;
            }

            public int DoorsCount
            {
                get
                {
                    return this.doors.Count;
                }
            }

            public int WheelCount
            {
                get
                {
                    return this.wheels.Count;
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
            public IDoor[] Doors
            {
                get
                {
                    return this.doors.ToArray();
                }
            }

            public CarColor Color
            {
                get
                {
                    return this.color;
                }
            }

            public void setWheelsPressure(double pression)
            {
                Asserts.isTrue(pression >= 1 && pression <= 5);
                for (int i = 0; i < this.WheelCount; i++)
                {
                    this.Wheels[i].Pressure = pression;
                }
            }
        }

        private class Wheel : IWheel
        {
            private double pression;

            public Wheel()
            {
                this.pression = 1;
            }

            public double Pressure
            {
                get
                {
                    return this.pression;
                }
                set
                {
                    Asserts.isTrue(value >= 1 && value <=5);
                    this.pression = value;
                }
            }
        }

        
    }
}