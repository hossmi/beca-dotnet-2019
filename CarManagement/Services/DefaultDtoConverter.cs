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
            EngineDto engineDto = new EngineDto();
            engineDto.HorsePower = engine.HorsePower;
            engineDto.Model = engine.Model;
            engineDto.IsStarted = engine.IsStarted;

            return engineDto;
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
            VehicleDto vehicleDto = new VehicleDto();
            vehicleDto.Color = vehicle.Color;
            vehicleDto.Engine = convert(vehicle.Engine);
            vehicleDto.Enrollment = convert(vehicle.Enrollment);
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
            DoorDto doorDto = new DoorDto();
            doorDto.IsOpen = door.IsOpen;
            doorDto.Model = door.Model;

            return doorDto;
        }

        public Wheel convert(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Model, wheelDto.Pressure);
        }

        public WheelDto convert(Wheel wheel)
        {
            WheelDto wheelDto = new WheelDto();
            wheelDto.Model = wheel.Model;
            wheelDto.Pressure = wheel.Pressure;

            return wheelDto;
        }

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            enrollmentDto.Serial = enrollment.Serial;
            enrollmentDto.Number = enrollment.Number;

            return enrollmentDto;
        }
    }
}