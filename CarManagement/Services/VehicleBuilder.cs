﻿using System;
using System.Collections.Generic;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using System.Linq;

namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private int numberWheel;
        private int numberDoor;
        private int horsePower;
        private CarColor color;
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.numberWheel = 0;
            this.numberDoor = 0;
            this.horsePower = 0;
            this.color = CarColor.Red;
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.numberWheel < 4);
            this.numberWheel++;
        }
        public void removeWheel()
        {
            Asserts.isTrue(this.numberWheel >0);
            this.numberWheel--;
        }
        public IVehicle build()
        {
            Asserts.isTrue(this.numberWheel > 0);

            List<IDoor> doors = createList<Door>(this.numberDoor)
                .Cast<IDoor>()
                .ToList();

            IEngine engine = new Engine(this.horsePower, false);
            
            List<IWheel> wheels = createList<Wheel>(this.numberWheel)
                .Cast<IWheel>()
                .ToList();
            
            IEnrollment enrollment = enrollmentProvider.getNew();
                        
            return new Vehicle(this.color, wheels, enrollment, doors, engine);
        }
        public void setColor(CarColor color)
        {
            Asserts.isEnumDefined(color);
            this.color = color;
        }
        public void setDoors(int doorsCount)
        {
            Asserts.isTrue( doorsCount>=0 && doorsCount <=6);
            this.numberDoor = doorsCount;
        }
        public void setEngine(int horsePower)
        {
            Asserts.isTrue(horsePower > 0);
            this.horsePower = horsePower;
        }
        public IVehicle import(VehicleDto vehicleDto)
        {
            CarColor color = vehicleDto.Color;

            List<IWheel> listWheels = vehicleDto
                .Wheels
                .Select(convert)
                .ToList();

            List<IDoor> listDoors = vehicleDto
                .Doors
                .Select(convert)
                .ToList();

            IEnrollment enrollment = convert(vehicleDto.Enrollment);

            IEngine engine = new Engine(vehicleDto.Engine.HorsePower, vehicleDto.Engine.IsStarted);

            return new Vehicle(color, listWheels, enrollment, listDoors, engine);
        }
        public VehicleDto export(IVehicle vehicle)
        {
            return convert(vehicle);
        }
        private IEnumerable<T> createList<T>(int numberItem) where T : class, new()
        {
            for (int x = 0; x < numberItem; x++)
            {
                yield return new T();
            }
        }
        

        private EnrollmentDto convert(IEnrollment enrollment)
        {
            return new EnrollmentDto
            {
                Number = enrollment.Number,
                Serial = enrollment.Serial,
            };
        }
        private IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial,
                                                  enrollmentDto.Number);
        }

        private IWheel convert(WheelDto weelDto)
        {
            return new Wheel(weelDto.Pressure);
        }
        private WheelDto convert(IWheel wheel)
        {
            return new WheelDto
            {
                Pressure = wheel.Pressure
            };
        }

        private IDoor convert(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }
        private DoorDto convert(IDoor door)
        {
            return new DoorDto
            {
                IsOpen = door.IsOpen
            };
        }

        private IEngine convert(EngineDto engineDto)
        {
            return new Engine(engineDto.HorsePower, engineDto.IsStarted);
        }
        private EngineDto convert(IEngine engine)
        {
            return new EngineDto
            {
                HorsePower = engine.HorsePower,
                IsStarted = engine.IsStarted
            };
        }
        
        private VehicleDto convert(IVehicle vehicle)
        {
            return new VehicleDto
            {
                Color = vehicle.Color,
                Engine = convert(vehicle.Engine),
                Enrollment = convert(vehicle.Enrollment),
                Wheels = vehicle.Wheels.Select(convert).ToArray(),
                Doors = vehicle.Doors.Select(convert).ToArray()
            };
        }
        private IVehicle convert(VehicleDto vehicleDto)
        {
            return new Vehicle( vehicleDto.Color
                ,vehicleDto.Wheels.Select(convert).ToList()
                ,convert(vehicleDto.Enrollment)
                ,vehicleDto.Doors.Select(convert).ToList()
                ,convert(vehicleDto.Engine)
                );
        }
        
        

        private class Wheel :IWheel
        {
            private readonly double presureMin = 1;
            private readonly double presureMax = 5;
            private double pressure;

            public double Pressure
            {
                set
                {
                    Asserts.isTrue(value >= presureMin && value <= presureMax);
                    this.pressure = value;
                }
                get
                {
                    return this.pressure;
                }
            }

            public Wheel()
            {
                this.Pressure = 1;
            }

            public Wheel(double pressure)
            {
                Asserts.isTrue(pressure >= presureMin && pressure<= presureMax);
                this.Pressure = pressure;
            }
        }

        private class Vehicle : IVehicle
        {
            private IReadOnlyList<IWheel> wheels;
            private readonly IEnrollment enrollment;
            private IReadOnlyList<IDoor> doors;
            private readonly IEngine engine;

            public Vehicle(CarColor color, List<IWheel> wheels, IEnrollment enrollment, List<IDoor> doors, IEngine engine)
            {
                this.Color = color;
                this.wheels = wheels;
                this.enrollment = enrollment;
                this.doors = doors;
                this.engine = engine;
            }

            public IEngine Engine
            {
                get
                {
                    return this.engine;
                }
            }

            public IEnrollment Enrollment { get { return this.enrollment; } }

            public IWheel[] Wheels
            {
                get
                {
                    IWheel[] wheelsArray = new IWheel[this.wheels.Count];

                    for (int i = 0; i < this.wheels.Count; i++)
                    {
                        wheelsArray[i] = this.wheels[i];
                    }

                    return wheelsArray;
                }
            }

            public IDoor[] Doors
            {
                get
                {
                    return this.doors
                        .Cast<IDoor>()
                        .ToArray();
                }
            }

            public CarColor Color { get; }

            public void setWheelsPressure(double pression)
            {
                foreach (Wheel wheel in this.wheels)
                {
                    wheel.Pressure = pression;
                }
            }
        }

        private class Engine: IEngine
        {
            private int horsePower;
            private bool isStarted;


            public Engine(int horsePower, bool isIstarted)
            {
                Asserts.isTrue(horsePower > 0);
                this.horsePower = horsePower;
                this.isStarted = isIstarted;
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
                    return this.horsePower;
                }
                set
                {
                    setHorsePower(value);
                }
            }

            public void start()
            {
                Asserts.isFalse(this.IsStarted);
                this.isStarted = true;
            }

            public void stop()
            {
                Asserts.isTrue(this.IsStarted);
                this.isStarted = false;
            }

            private void setHorsePower(int horsePower)
            {
                Asserts.isTrue(horsePower > 0);
                this.horsePower = horsePower;
            }
        }
        private class Door :IDoor
        {
            private bool openDoor;

            public Door()
            {
                this.openDoor = false;
            }

            public Door(bool openClose)
            {
                //Asserts.isFalse(this.openDoor);
                this.openDoor = openClose;
            }

            public bool IsOpen
            {
                get
                {
                    return this.openDoor;
                }
            }

            public void open()
            {
                Asserts.isFalse(this.IsOpen);
                this.openDoor = true;
            }

            public void close()
            {
                Asserts.isTrue(this.openDoor);
                this.openDoor = false;
            }
        }

        public IEnrollment import(string serial, int number)
        {
            return this.enrollmentProvider.import(serial, number);
        }
    }
}