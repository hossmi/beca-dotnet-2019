namespace CarManagement.Core.Models
{
    public interface IVehicle
    {
        CarColor Color { get; }
        IDoor[] Doors { get; }
        IEngine Engine { get; }
        IEnrollment Enrollment { get; }
        IWheel[] Wheels { get; }

    }
}