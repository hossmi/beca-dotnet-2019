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
            EngineDto engineDto = new EngineDto
            {
                HorsePower = engine.HorsePorwer,
                IsStarted = engine.IsStarted
            };

            return engineDto;
        }

        public Vehicle convert(VehicleDto vehicleDto)
        {
            Vehicle vehicle = new Vehicle(createWheels(vehicleDto), createDoors(vehicleDto), 
                convert(vehicleDto.Engine), vehicleDto.Color, convert(vehicleDto.Enrollment));

            return vehicle;
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
            DoorDto doorDto = new DoorDto
            {
                IsOpen = door.IsOpen
            };

            return doorDto;
        }

        public Wheel convert(WheelDto wheelDto)
        {
            Wheel wheel = new Wheel
            {
                Pressure = wheelDto.Pressure
            };

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
            IEnrollment enrollment = this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);

            return enrollment;
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto
            {
                Serial = enrollment.Serial,
                Number = enrollment.Number,
            };
            

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

        private List<Door> createDoors(VehicleDto vehicleDto)
        {
            List<Door> doors = new List<Door>();
            for(int i = 0; i < vehicleDto.Doors.Length; i++)
            {
                doors.Add(convert(vehicleDto.Doors[i]));
            }

            return doors;
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

        private List<Wheel> createWheels(VehicleDto vehicleDto)
        {
            List<Wheel> wheels = new List<Wheel>();
            for (int i = 0; i < vehicleDto.Wheels.Length; i++)
            {
                wheels.Add(convert(vehicleDto.Wheels[i]));
            }

            return wheels;
        }
    }
}