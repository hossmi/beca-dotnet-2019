using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    class EnrollmentDto
    {
        string Serial { get; set; }
        int Number { get; set; }

        public EnrollmentDto(IEnrollment e)
        {
            this.Serial = e.Serial;
            this.Number = e.Number;
        }

        public IEnrollment ConvertToIEnrollment()
        {
            IEnrollment e = new IEnrollment();



            return e;
        }
    }
}
