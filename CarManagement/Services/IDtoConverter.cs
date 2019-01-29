using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public interface IDtoConverter
    {
        Engine convert(EngineDto engineDto);
        EngineDto convert(Engine engine);
        Vehicle convert(VehicleDto vehicleDto);
        VehicleDto convert(Vehicle vehicle);
        Door convert(DoorDto doorDto);
        DoorDto convert(Door door);
        Wheel convert(WheelDto wheelDto);
        WheelDto convert(Wheel wheel);
        IEnrollment convert(EnrollmentDto enrollmentDto);
        EnrollmentDto convert(IEnrollment enrollment);
    }
}