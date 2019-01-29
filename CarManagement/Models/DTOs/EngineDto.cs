using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    class EngineDto
    {
        public bool IsStarted { get; set; }
        public double HorsePower { get; set; }

        public EngineDto(Engine e)
        {
            this.IsStarted = e.IsStarted;
            this.HorsePower = e.HorsePower;
        }

        public Engine ConvertToEngine()
        {
            Engine e = new Engine((int)this.HorsePower);

            if (this.IsStarted)
                e.start();

            return e;
        }
    }
}
