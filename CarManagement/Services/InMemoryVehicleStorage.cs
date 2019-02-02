﻿using System;
using System.Collections.Generic;
using CarManagement.Core.Models;



namespace CarManagement.Services
{
    public class InMemoryVehicleStorage : AbstractVehicleStorage
    {
             public InMemoryVehicleStorage() : base(new Dictionary<IEnrollment,IVehicle>())
        {
          
        }
        protected override void save(IEnumerable<IVehicle> vehicles)
        {

        }
    }
}