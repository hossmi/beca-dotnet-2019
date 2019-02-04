using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core.Models;
using CarManagement.Services;

namespace BusinessCore.Tests.Services
{
    public class ArrayVehicleStorage : AbstractVehicleStorage
    {
        public ArrayVehicleStorage() : base(initialVehicles())
        {
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
        }

        private static IDictionary<IEnrollment, IVehicle> initialVehicles()
        {

            throw new NotImplementedException();
        }



    }
}
