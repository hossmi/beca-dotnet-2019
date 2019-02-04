using CarManagement.Models;
using System.Collections.Generic;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class DefaultDtoConverter 
    {
        private readonly IEnrollmentProvider enrollmentProvider;

        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public IEngine convert(EngineDto engineDto)
        {
            return new Engine(engineDto.HorsePower, engineDto.IsStarted);
        }

        public EngineDto convert(IEngine engine)
        {
            return new EngineDto
            {
                HorsePower = engine.HorsePower,
                IsStarted = engine.IsStarted
            };
        }

        public IVehicle convert(VehicleDto vehicleDto)
        {
            List<IDoor> doors = new List<IDoor>();
            foreach (DoorDto door in vehicleDto.Doors)
            {
                doors.Add(this.convert(door));
            }

            List<IWheel> wheels = new List<IWheel>();
            foreach (WheelDto wheel in vehicleDto.Wheels)
            {
                wheels.Add(this.convert(wheel));
            }

            return new Vehicle(this.convert(vehicleDto.Engine), doors, wheels, vehicleDto.Color, this.convert(vehicleDto.Enrollment));
        }

        public VehicleDto convert(IVehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto
            {
                Color = vehicle.Color,
                Engine = this.convert(vehicle.Engine),
                Enrollment = this.convert(vehicle.Enrollment)
            };
            List<DoorDto> dtoDoorsList = new List<DoorDto>();
            foreach (Door door in vehicle.Doors)
            {
                dtoDoorsList.Add(this.convert(door));
            }
            vehicleDto.Doors = dtoDoorsList.ToArray();

            List<WheelDto> dtoWheelsList = new List<WheelDto>();
            foreach (Wheel wheel in vehicle.Wheels)
            {
                dtoWheelsList.Add(this.convert(wheel));
            }
            vehicleDto.Wheels = dtoWheelsList.ToArray();

            return vehicleDto;
        }

        public IDoor convert(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }

        public DoorDto convert(IDoor door)
        {
            return new DoorDto
            {
                IsOpen = door.IsOpen,
            };
        }

        public IWheel convert(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Pressure);
        }

        public WheelDto convert(IWheel wheel)
        {
            return new WheelDto
            {
                Pressure = wheel.Pressure,
            };
        }

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            return new EnrollmentDto
            {
                Serial = enrollment.Serial,
                Number = enrollment.Number
            };
        }
    }
}