using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    public class EngineDto
    {
        
        public EngineDto(bool isStarted, int horsePower)
        {
            IsStarted = isStarted;
            HorsePower = horsePower;
        }

        public bool IsStarted { get; set; }
        public int HorsePower { get; set; }
    }
}
