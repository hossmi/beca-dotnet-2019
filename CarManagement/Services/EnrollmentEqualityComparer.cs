using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core.Models;

namespace CarManagement.Services
{
    public class EnrollmentEqualityComparer : IEqualityComparer<IEnrollment>
    {
        public bool Equals(IEnrollment y, IEnrollment x)
        {
            if (y.Serial == x.Serial && y.Number == x.Number)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(IEnrollment obj)
        {
            return (($"{obj.Serial}-{obj.Number.ToString("0000")}").GetHashCode());
        }
    }
}
