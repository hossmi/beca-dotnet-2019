using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    [Serializable]
    public class WheelDto
    {
        public string Model { get; set; }
        public double Pressure { get; set; }
    }
}
