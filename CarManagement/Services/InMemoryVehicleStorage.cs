using CarManagement.Builders;
using CarManagement.Models;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
        //private IDictionary<IEnrollment, Vehicle> vehicles = new Dictionary<IEnrollment, Vehicle>();

        //public int Count
        //{
        //    get
        //    {
        //        return this.vehicles.Count;
        //    }
        //}

        //public void clear()
        //{
        //    this.vehicles.Clear();
        //}

        //public Vehicle get(IEnrollment enrollment)
        //{
        //    //Asserts.isTrue(vehicles.Count > 0,"La lista de vehiculos esta vacia");
        //    Vehicle vehicle;
        //    bool vehicleFound = this.vehicles.TryGetValue(enrollment, out vehicle);
        //    Asserts.isTrue(vehicleFound,"Cannot find vehicle");
        //    return vehicle;
        //}

        //public void set(Vehicle vehicle)
        //{
        //    Asserts.isFalse(this.vehicles.ContainsKey(vehicle.Enrollment), "A vehicle already exists with the same enrollment");
        //    this.vehicles.Add(vehicle.Enrollment, vehicle);
        //}

        protected override IDictionary<IEnrollment, Vehicle> load()
        {
            return new Dictionary<IEnrollment, Vehicle>();
        }

        protected override void save(IEnumerable<Vehicle> vehicles)
        {
            //this is already implemented in the parent class.
        }
    }
}