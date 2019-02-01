using System;
using System.Collections.Generic;
using CarManagement.Models;
using System.Collections;



namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
             public InMemoryVehicleStorage() : base(new Dictionary<IEnrollment,Vehicle>())
        {
          
        }
        protected override void save(IEnumerable<Vehicle> vehicles)
        {

        }
    }
}