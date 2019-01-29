using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Models;

namespace CarManagement.Services
{
    public class EnrollmentEqualityComparer : IEqualityComparer<IEnrollment>
    {
        public bool Equals(IEnrollment x, IEnrollment y)
        {
            if (x.Serial == y.Serial && x.Number == y.Number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(IEnrollment obj)
        {
            return obj.GetHashCode();
        }
    }
}
