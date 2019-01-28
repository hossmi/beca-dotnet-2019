using CarManagement.Models;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {
        public int Count { get; }

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