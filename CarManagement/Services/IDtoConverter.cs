using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;

namespace CarManagement.Services
{
    public interface IDtoConverter
    {
        IEngine convert(EngineDto engineDto);
        EngineDto convert(IEngine engine);
        IVehicle convert(VehicleDto vehicleDto);
        VehicleDto convert(IVehicle vehicle);
        IDoor convert(DoorDto doorDto);
        DoorDto convert(IDoor door);
        IWheel convert(WheelDto wheelDto);
        WheelDto convert(IWheel wheel);
        IEnrollment convert(EnrollmentDto enrollmentDto);
        EnrollmentDto convert(IEnrollment enrollment);
    }
}