using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public class DefaultDtoConverter : IDtoConverter
    {
        private IEnrollmentProvider enrollmentProvider;

        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }


        public Engine convert(EngineDto engineDto)
        {
            return convertEngine(engineDto);
        }

        public EngineDto convert(Engine engine)
        {
            return new EngineDto
            {
                HorsePower = engine.HorsePower,
                IsStarted = engine.IsStarted
            };
        }

        

        public Door convert(DoorDto doorDto)
        {
            return convertDoor(doorDto);
        }

        public DoorDto convert(Door door)
        {
            return new DoorDto
            {
                IsOpen = door.IsOpen
            };
        }



        public Wheel convert(WheelDto wheelDto)
        {
            throw new System.NotImplementedException();
        }

        public WheelDto convert(Wheel wheel)
        {
            throw new System.NotImplementedException();
        }



        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            throw new System.NotImplementedException();
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            throw new System.NotImplementedException();
        }



        public Vehicle convert(VehicleDto vehicleDto)
        {
            throw new System.NotImplementedException();
        }


        public VehicleDto convert(Vehicle vehicle)
        {
            throw new System.NotImplementedException();
        }

        

        private static Engine convertEngine(EngineDto engineDto)
        {
            return new Engine(engineDto.HorsePower, engineDto.IsStarted);
        }

        private static Door convertDoor(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }
    }
}