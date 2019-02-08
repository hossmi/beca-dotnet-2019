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
            List<Wheel> wheels = new List<Wheel>(vehicleDto.Wheels.Length);
            List<Door> doors = new List<Door>(vehicleDto.Doors.Length);

            foreach (WheelDto wheel in vehicleDto.Wheels)
            {
                wheels.Add(convert(wheel));
            }

            foreach (DoorDto door in vehicleDto.Doors)
            {
                doors.Add(convert(door));
            }

            Engine engine = convert(vehicleDto.Engine);

            IEnrollment enrollment = convert(vehicleDto.Enrollment);

            Vehicle vehicle = new Vehicle(wheels, doors, engine, enrollment, vehicleDto.Color);

            return vehicle;
        }

        public VehicleDto convert(Vehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto
            {
                Wheels = new WheelDto[vehicle.Wheels.Length],
                Doors = new DoorDto[vehicle.Doors.Length],
                Engine = convert(vehicle.Engine),
                Color = vehicle.Color,
                Enrollment = convert(vehicle.Enrollment)
            };

            for (int i = 0; i < vehicle.Wheels.Length; i++)
            {
                vehicleDto.Wheels[i] = convert(vehicle.Wheels[i]);
            }

            for (int i = 0; i < vehicle.Doors.Length; i++)
            {
                vehicleDto.Doors[i] = convert(vehicle.Doors[i]);
            }

            return vehicleDto;
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
            IEnrollment enrollment = this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
            return enrollment;
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto
            {
                Serial = enrollment.Serial,
                Number = enrollment.Number
            };

            return enrollmentDto;
        }
    }
}