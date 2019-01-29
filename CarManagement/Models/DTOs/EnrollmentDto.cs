using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    [Serializable]
    class EnrollmentDto : IEnrollment
    {
        public string Serial { get; set; }
        public int Number { get; set; }
    }
}
