﻿using CarManagement.Models;
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
            return new EngineDto( )
            {

                HorsePower = engineDto.HorsePower,
                IsStarted = engineDto.IsStarted,

            };
        }

        public EngineDto convert(Engine engine)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public Wheel convert(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Pressure);
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
            throw new System.NotImplementedException();
        }
    }
}