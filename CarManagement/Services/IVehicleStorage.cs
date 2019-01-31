using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Models;

namespace CarManagement.Services
{
    public interface IVehicleStorage
    {
        int Count { get; }

        void set(Vehicle vehicle);
        Vehicle get(IEnrollment enrollment);
        void clear();
        Vehicle[] getAll();
    }
}
