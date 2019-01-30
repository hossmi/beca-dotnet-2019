using CarManagement.Models;
using CarManagement.Models.DTOs;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class DefaultDtoConverter : IDtoConverter
    {
        private IEnrollmentProvider enrollmentProvider;

        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public Engine convert(EngineDto engineDto)
        {
            return new Engine(engineDto.HorsePower, engineDto.Model, engineDto.IsStarted);
        }

        public EngineDto convert(Engine engine)
        {
            return new EngineDto
            {
                HorsePower = engine.HorsePower,
                Model = engine.Model,
                IsStarted = engine.IsStarted
            };
        }

        public Vehicle convert(VehicleDto vehicleDto)
        {
            List<Door> doors = new List<Door>();
            foreach (DoorDto door in vehicleDto.Doors)
            {
                doors.Add(convert(door));
            }

            List<Wheel> wheels = new List<Wheel>();
            foreach (WheelDto wheel in vehicleDto.Wheels)
            {
                wheels.Add(convert(wheel));
            }

            return new Vehicle(convert(vehicleDto.Engine), doors, wheels, vehicleDto.Color, convert(vehicleDto.Enrollment));
        }

        public VehicleDto convert(Vehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto
            {
                Color = vehicle.Color,
                Engine = convert(vehicle.Engine),
                Enrollment = convert(vehicle.Enrollment)
            };
            List<DoorDto> dtoDoorsList = new List<DoorDto>();
            foreach (Door door in vehicle.Doors)
            {
                dtoDoorsList.Add(convert(door));
            }
            vehicleDto.Doors = dtoDoorsList.ToArray();

            List<WheelDto> dtoWheelsList = new List<WheelDto>();
            foreach (Wheel wheel in vehicle.Wheels)
            {
                dtoWheelsList.Add(convert(wheel));
            }
            vehicleDto.Wheels = dtoWheelsList.ToArray();

            return vehicleDto;
        }

        public Door convert(DoorDto doorDto)
        {
            return new Door(doorDto.Model, doorDto.IsOpen);
        }

        public DoorDto convert(Door door)
        {
            return new DoorDto
            {
                IsOpen = door.IsOpen,
                Model = door.Model
            };
        }

        public Wheel convert(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Model, wheelDto.Pressure);
        }

        public WheelDto convert(Wheel wheel)
        {
            return new WheelDto
            {
                Model = wheel.Model,
                Pressure = wheel.Pressure
            };
        }

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
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