using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    class EngineDto
    {
        public string Model { get; set; }
        public bool IsStarted { get; set; }
        public int HorsePower { get; set; }
    }
}
