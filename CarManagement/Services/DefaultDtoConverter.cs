using CarManagement.Models;
using System.Collections.Generic;
using CarManagement.Models.DTOs;

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
            Engine engine = new Engine(engineDto.HorsePower,engineDto.IsStarted);
            return engine;
        }

        public EngineDto convert(Engine engine)
        {
            EngineDto engineDto = new EngineDto
            {
                HorsePower = engine.HorsePower,
                IsStarted = engine.IsStarted
            };

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
            throw new System.NotImplementedException();
        }

        public DoorDto convert(Door door)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            throw new System.NotImplementedException();
        }
    }
}