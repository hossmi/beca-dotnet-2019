using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    class MapsDto
    {
        public EngineDto convert(Engine engine)
        {
            EngineDto engineDto = new EngineDto();
            engineDto.HorsePower = engine.HorsePorwer;
            engineDto.IsStarted = engine.IsStarted;

            return engineDto;
        }

        public Engine convert(EngineDto engineDto)
        {
            Engine engine = new Engine(engineDto.HorsePower);
            engineDto.IsStarted = engine.IsStarted;

            return engine;
        }

        public EnrollmentDto convert()
        {
            throw new System.NotImplementedException();
        }
    }
}
