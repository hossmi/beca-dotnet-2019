using System;
using System.Linq;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public void addWheel()
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

        public void setColor(CarColor color)
        {
            throw new NotImplementedException();
        }

        public IVehicle build()
        {
            throw new NotImplementedException();
        }

        public void removeWheel()
        {
            throw new NotImplementedException();
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

        public VehicleDto export(IVehicle vehicleDto)
        {
            throw new NotImplementedException();
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
                this.IsStarted = true;
            }

            public void stop()
            {
                this.IsStarted = false;
            }
        }

        private class PrvDoor : IDoor
        {
            public bool IsOpen { get; set; }

            public void close()
            {
                this.IsOpen = false;
            }

            public void open()
            {
                this.IsOpen = true;
            }
        }

        private class PrvWheel : IWheel
        {
            public double Pressure { get; set; }
        }
    }
}