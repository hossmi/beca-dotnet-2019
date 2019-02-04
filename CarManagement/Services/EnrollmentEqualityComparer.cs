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
        public bool Equals(IEnrollment x, IEnrollment y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(IEnrollment obj)
        {
            throw new NotImplementedException();
        }
    }
}
