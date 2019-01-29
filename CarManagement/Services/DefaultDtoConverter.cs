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
            return new Engine(engineDto.HorsePower, engineDto.IsStarted);
        }

        public EngineDto convert(Engine engine)
        {
            EngineDto engineDto = new EngineDto();
            engineDto.HorsePower = engine.HorsePorwer;
            engineDto.IsStarted = engine.IsStarted;

            return engineDto;
        }

        public Vehicle convert(VehicleDto vehicleDto)
        {
            throw new System.NotImplementedException();
        }

        public VehicleDto convert(Vehicle vehicle)
        {
            throw new System.NotImplementedException();
        }

        public Door convert(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }

        public DoorDto convert(Door door)
        {
            DoorDto doorDto = new DoorDto();
            doorDto.IsOpen = door.IsOpen;

            return doorDto;
        }

        public Wheel convert(WheelDto wheelDto)
        {
            throw new System.NotImplementedException();
        }

        public WheelDto convert(Wheel wheel)
        {
            throw new System.NotImplementedException();
        }

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            enrollmentDto.Serial = enrollment.Serial;
            enrollmentDto.Number = enrollmentDto.Number;

            return enrollmentDto;
        }
    }
}