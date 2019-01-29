using CarManagement.Models;
using CarManagement.Models.DTOs;
using System;
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
            Engine engine = new Engine(engineDto.HorsePower);
            engine.IsStarted = engineDto.IsStarted;

            return engine;
        }

        public EngineDto convert(Engine engine)
        {
            EngineDto engineDto = new EngineDto();
            engineDto.HorsePower = engine.HorsePower;
            engineDto.IsStarted = engine.IsStarted;

            return engineDto;
        }

        public Door convert(DoorDto doorDto)
        {
            Door door = new Door();
            door.IsOpen = doorDto.IsOpen;

            return door;
        }

        public DoorDto convert(Door door)
        {
            DoorDto doorDto = new DoorDto();
            doorDto.IsOpen = door.IsOpen;

            return doorDto;
        }

        public Wheel convert(WheelDto wheelDto)
        {
            Wheel wheel = new Wheel();
            wheel.Pressure = wheelDto.Pressure;

            return wheel;
        }

        public WheelDto convert(Wheel wheel)
        {
            WheelDto wheelDto = new WheelDto();
            wheelDto.Pressure = wheel.Pressure;

            return wheelDto;
        }

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            enrollmentDto.Serial = enrollment.Serial;
            enrollmentDto.Number = enrollment.Number;

            return enrollmentDto;
        }

        public Vehicle convert(VehicleDto vehicleDto)
        {
            Vehicle vehicle;
            List<Door> doors = new List<Door>();
            List<Wheel> wheels = new List<Wheel>();

            foreach (DoorDto doorDto in vehicleDto.Doors)
            {
                doors.Add(convert(doorDto));
            }

            foreach (WheelDto wheelDto in vehicleDto.Wheels)
            {
                wheels.Add(convert(wheelDto));
            }

            Engine engine = convert(vehicleDto.Engine);
            IEnrollment enrollment = convert(vehicleDto.Enrollment);
            CarColor carColor = vehicleDto.Color;

            vehicle = new Vehicle(doors, wheels, engine, enrollment, carColor);
            return vehicle;
        }

        public VehicleDto convert(Vehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto();

            DoorDto[] doorsDto = new DoorDto[vehicle.DoorsCount];
            WheelDto[] wheelsDto = new WheelDto[vehicle.WheelCount];

         

            for (int i = vehicle.Doors.Length - 1; i > 0;i--)
            {
                doorsDto[i] = vehicle.Doors[i];
            }

                foreach (Wheel wheel in vehicle.Wheels)
                {
                    wheels.Add(wheel);
                }

            doorsDto = doors.ToArray();

            Engine engine = convert(vehicleDto.Engine);
            IEnrollment enrollment = convert(vehicleDto.Enrollment);
            CarColor carColor = vehicleDto.Color;

            return vehicleDto;
        }
    }
}