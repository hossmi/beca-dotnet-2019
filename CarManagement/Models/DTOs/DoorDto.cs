using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    class DoorDto
    {
        public bool IsOpen { get; set; }

        public DoorDto(Door d)
        {
            this.IsOpen = d.IsOpen;
        }

        public Door ConvertToDoor()
        {
            Door d = new Door();

            if (this.IsOpen)
                d.open();
            else
                d.close();

            return d;
        }
    }
}
