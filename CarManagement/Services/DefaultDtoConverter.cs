using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class DefaultDtoConverter : IDtoConverter
    {
        private IEnrollmentProvider enrollmentProvider;

        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public IEngine convert(EngineDto engineDto)
        {
            throw new System.NotImplementedException();
        }

        public EngineDto convert(IEngine engine)
        {
            throw new System.NotImplementedException();
        }

        public IVehicle convert(VehicleDto vehicleDto)
        {
            throw new System.NotImplementedException();
        }

        public VehicleDto convert(IVehicle vehicle)
        {
            throw new System.NotImplementedException();
        }

        public IDoor convert(DoorDto doorDto)
        {
            throw new System.NotImplementedException();
        }

        public DoorDto convert(IDoor door)
        {
            throw new System.NotImplementedException();
        }

        public IWheel convert(WheelDto wheelDto)
        {
            throw new System.NotImplementedException();
        }

        public WheelDto convert(IWheel wheel)
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