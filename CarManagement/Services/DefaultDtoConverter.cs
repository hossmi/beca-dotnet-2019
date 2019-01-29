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

        Engine IDtoConverter.convert(EngineDto engineDto)
        {
            return new Engine(engineDto.HorsePower, engineDto.IsStarted);
        }

        EngineDto IDtoConverter.convert(Engine engine)
        {
            EngineDto engineDto = new EngineDto();
            engineDto.HorsePower = engine.HorsePorwer;
            engineDto.IsStarted = engine.IsStarted;

            return engineDto;
        }

        Vehicle IDtoConverter.convert(VehicleDto vehicleDto)
        {
            throw new System.NotImplementedException();
        }

        VehicleDto IDtoConverter.convert(Vehicle vehicle)
        {
            throw new System.NotImplementedException();
        }

        Door IDtoConverter.convert(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }

        DoorDto IDtoConverter.convert(Door door)
        {
            DoorDto doorDto = new DoorDto();
            doorDto.IsOpen = door.IsOpen;

            return doorDto;
        }

        Wheel IDtoConverter.convert(WheelDto wheelDto)
        {
            throw new System.NotImplementedException();
        }

        WheelDto IDtoConverter.convert(Wheel wheel)
        {
            throw new System.NotImplementedException();
        }

        IEnrollment IDtoConverter.convert(EnrollmentDto enrollmentDto)
        {
            return enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        EnrollmentDto IDtoConverter.convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            enrollmentDto.Serial = enrollment.Serial;
            enrollmentDto.Number = enrollmentDto.Number;

            return enrollmentDto;
        }
    }
}