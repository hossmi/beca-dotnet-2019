using CarManagement.Models;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : IVehicleStorage
    {
        public int Count { get; }

        public void clear()
        {
            throw new System.NotImplementedException();
        }

        public Vehicle get(IEnrollment enrollment)
        {
            throw new System.NotImplementedException();
        }

        public void set(Vehicle vehicle)
        {
            throw new System.NotImplementedException();
        }
    }
}