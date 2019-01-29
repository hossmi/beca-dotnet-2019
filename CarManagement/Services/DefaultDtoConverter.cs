using CarManagement.Models;
using CarManagement.Models.DTOs;
using System.Collections.Generic;

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
            return new Engine(engineDto.HorsePower, engineDto.IsStarted);
        }

        public EngineDto convert(Engine engine)
        {
            EngineDto engineDto = new EngineDto();
            engineDto.HorsePower = engine.HorsePorwer;
            engineDto.IsStarted = engine.IsStarted;

            return engineDto;
        }

        public Vehicle convert(VehicleDto vehicleDto)
        {
            throw new System.NotImplementedException();
        }

        public VehicleDto convert(Vehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto
            {
                Doors = createDoorsDto(vehicle),
                Wheels = createWheelDto(vehicle),
                Engine = convert(vehicle.Engine),
                Enrollment = convert(vehicle.Enrollment),
                Color = vehicle.getColor()
            };

            return vehicleDto;
        }

        public Door convert(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }

        public DoorDto convert(Door door)
        {
            DoorDto doorDto = new DoorDto();
            doorDto.IsOpen = door.IsOpen;

            return doorDto;
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
            return enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            enrollmentDto.Serial = enrollment.Serial;
            enrollmentDto.Number = enrollmentDto.Number;

            return enrollmentDto;
        }

        private DoorDto[] createDoorsDto(Vehicle vehicle)
        {
            DoorDto[] doorsDto = new DoorDto[vehicle.DoorsCount];
            for(int i = 0; i < vehicle.DoorsCount; i++)
            {
                doorsDto[i] = convert(vehicle.Doors[i]);
            }

            return doorsDto;
        }

        private WheelDto[] createWheelDto(Vehicle vehicle)
        {
            WheelDto[] wheelsDto = new WheelDto[vehicle.WheelCount];
            for (int i = 0; i < vehicle.WheelCount; i++)
            {
                wheelsDto[i] = convert(vehicle.Wheels[i]);
            }

            return wheelsDto;
        }
    }
}