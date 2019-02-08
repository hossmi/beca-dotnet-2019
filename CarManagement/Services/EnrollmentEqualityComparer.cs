﻿using System;
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
            return x.Number == y.Number && x.Serial == y.Serial;
        }

        public int GetHashCode(IEnrollment obj)
        {
            return ((int)obj.Serial[2] - 65) * 100000000 + ((int)obj.Serial[1] - 65) * 1000000 + ((int)obj.Serial[0] - 65) * 10000 + obj.Number;
        }
    }
}