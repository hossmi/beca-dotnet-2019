using CarManagement.Models;
using System.Collections.Generic;
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
            Engine engine = new Engine(engineDto.HorsePower, engineDto.IsStarted);
            return engine;
        }

        public EngineDto convert(Engine engine)
        {
            EngineDto engineDto = new EngineDto
            {
                HorsePower = engine.HorsePower,
                IsStarted = engine.IsStarted
            };

            return engineDto;
        }

        public Vehicle convert(VehicleDto vehicleDto)
        {
            Wheel wheel = convert(vehicleDto.Wheels[0]);
            foreach (Wheel wheels in )
            {
               
            }


            Door door = convert(vehicleDto.Doors[0]);
            Engine engine = convert(vehicleDto.Engine);
            IEnrollment enrollment = convert(vehicleDto.Enrollment);
            
            Vehicle vehicle = new Vehicle(vehicleDto.Wheels, vehicleDto.Door, engine, vehicleDto.Color, enrollment);
            return vehicle;
        }

        public VehicleDto convert(Vehicle vehicle)
        {
            throw new System.NotImplementedException();
        }

        public Door convert(DoorDto doorDto)
        {
            Door door = new Door(doorDto.IsOpen);
            return door;
        }

        public DoorDto convert(Door door)
        {
            DoorDto doorDto = new DoorDto
            {
                IsOpen = door.IsOpen

            };

            return doorDto;
        }

        public Wheel convert(WheelDto wheelDto)
        {
            Wheel wheel = new Wheel(wheelDto.Pressure);
            return wheel;
        }

        public WheelDto convert(Wheel wheel)
        {
            WheelDto wheelDto = new WheelDto
            {
                Pressure = wheel.Pressure
            };

            return wheelDto;
        }

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            throw new System.NotImplementedException();
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            throw new System.NotImplementedException();
        }
    }
}