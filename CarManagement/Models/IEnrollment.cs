using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models
{
    public interface IEnrollment
    {
        string Serial { get; }
        int Number { get; }
        string Print();
    }
}
