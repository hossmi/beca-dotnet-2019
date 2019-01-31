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
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(IEnrollment enrollment)
        {
            return $"{enrollment.Serial}-{enrollment.Number.ToString("0000")}".GetHashCode();
        }
    }
}
