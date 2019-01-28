using CarManagement.Models;

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
        public int Count { get; }

        public void clear()
        {
            throw new System.NotImplementedException();
        }

        public Vehicle get(IEnrollment defaultEnrollment)
        {
            throw new System.NotImplementedException();
        }

        public void set(Vehicle motoVehicle)
        {
            throw new System.NotImplementedException();
        }
    }
}