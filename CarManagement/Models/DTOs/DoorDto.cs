using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    [Serializable]
    public class DoorDto
    {
        public string Model { get; set; }
        public bool IsOpen { get; set; }
    }
}
