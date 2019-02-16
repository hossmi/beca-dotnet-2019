using System;
using System.Collections.Generic;
using System.Linq;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private const double DEFAULT_PRESSURE = 2.2;
        private readonly IEnrollmentProvider enrollmentProvider;
        private int wheels;
        private int doorsCount;
        private int horsePower;
        private CarColor color;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.wheels = 0;
            this.doorsCount = 0;
            this.horsePower = 0;
            this.color = CarColor.White;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.wheels < 4);
            this.wheels++;
        }

        public void removeWheel()
        {
            Asserts.isTrue(0 < this.wheels);
            this.wheels--;
        }

        public void setDoors(int doorsCount)
        {
            Asserts.isTrue(0 <= doorsCount && doorsCount <= 6);
            this.doorsCount = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            Asserts.isTrue(0 < horsePorwer);
            this.horsePower = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            Asserts.isEnumDefined(color);
            this.color = color;
        }

        public IVehicle build()
        {
            Asserts.isTrue(0 < this.wheels && this.wheels <= 4);
            Asserts.isTrue(0 < this.horsePower);
            Asserts.isTrue(0 <= this.doorsCount && this.doorsCount <= 6);

            return new PrvVehicle
            {
                Color = this.color,
                Enrollment = this.enrollmentProvider.getNew(),
                Engine = new PrvEngine
                {
                    HorsePower = this.horsePower,
                    IsStarted = false,
                },
                Doors = build(this.doorsCount, () => new PrvDoor
                {
                    IsOpen = false,
                })
                    .ToArray(),
                Wheels = build(this.wheels, () => new PrvWheel
                {
                    Pressure = DEFAULT_PRESSURE,
                })
                    .ToArray(),
            };
        }

        public IVehicle import(VehicleDto vehicleDto)
        {
            return new PrvVehicle
            {
                Color = vehicleDto.Color,
                Enrollment = this.enrollmentProvider
                    .import(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number),
                Engine = new PrvEngine
                {
                    HorsePower = vehicleDto.Engine.HorsePower,
                    IsStarted = vehicleDto.Engine.IsStarted,
                },
                Doors = vehicleDto
                    .Doors
                    .Select(d => new PrvDoor
                    {
                        IsOpen = d.IsOpen,
                    })
                    .ToArray(),
                Wheels = vehicleDto
                    .Wheels
                    .Select(w => new PrvWheel
                    {
                        Pressure = w.Pressure,
                    })
                    .ToArray(),
            };
        }

        public VehicleDto export(IVehicle vehicle)
        {
            return new VehicleDto
            {
                Color = vehicle.Color,
                Enrollment = new EnrollmentDto
                {
                    Serial = vehicle.Enrollment.Serial,
                    Number = vehicle.Enrollment.Number,
                },
                Engine = new EngineDto
                {
                    HorsePower = vehicle.Engine.HorsePower,
                    IsStarted = vehicle.Engine.IsStarted,
                },
                Doors = vehicle
                    .Doors
                    .Select(d => new DoorDto
                    {
                        IsOpen = d.IsOpen,
                    })
                    .ToArray(),
                Wheels = vehicle
                    .Wheels
                    .Select(w => new WheelDto
                    {
                        Pressure = w.Pressure,
                    })
                    .ToArray(),
            };
        }

        public IEnrollment import(string serial, int number)
        {
            return this.enrollmentProvider.import(serial, number);
        }

        private static IEnumerable<T> build<T>(int n, Func<T> creator)
        {
            for (int i = 0; i < n; i++)
                yield return creator();
        }

        private class PrvVehicle : IVehicle
        {
            public CarColor Color { get; set; }
            public IDoor[] Doors { get; set; }
            public IEngine Engine { get; set; }
            public IEnrollment Enrollment { get; set; }
            public IWheel[] Wheels { get; set; }
        }

        private class PrvEngine : IEngine
        {
            public int HorsePower { get; set; }
            public bool IsStarted { get; set; }

            public void start()
            {
                Asserts.isFalse(this.IsStarted);
                this.IsStarted = true;
            }

            public void stop()
            {
                Asserts.isTrue(this.IsStarted);
                this.IsStarted = false;
            }
        }

        private class PrvDoor : IDoor
        {
            public bool IsOpen { get; set; }

            public void close()
            {
                Asserts.isTrue(this.IsOpen);
                this.IsOpen = false;
            }

            public void open()
            {
                Asserts.isFalse(this.IsOpen);
                this.IsOpen = true;
            }
        }

        private class PrvWheel : IWheel
        {
            private double pressure;

            public double Pressure
            {
                get => this.pressure; set
                {
                    Asserts.isTrue(1 <= value && value <= 5);
                    this.pressure = value;
                }
            }
        }
    }
}