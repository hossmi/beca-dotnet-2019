using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Services
{
    public interface IEnrollmentProvider
    {
        string NewEnrollment { get; }
    }
}
