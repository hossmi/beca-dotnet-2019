using CarManagement.Models;
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

        Engine IDtoConverter.convert(EngineDto engineDto)
        {
            throw new System.NotImplementedException();
        }

        EngineDto IDtoConverter.convert(Engine engine)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        DoorDto IDtoConverter.convert(Door door)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        EnrollmentDto IDtoConverter.convert(IEnrollment enrollment, IEnrollmentProvider enrollmentProvider)
        {
            throw new System.NotImplementedException();
        }
    }
}