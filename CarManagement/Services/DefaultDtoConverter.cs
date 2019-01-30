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
            return convertToEngine(engineDto);
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
            return convertToDoor(doorDto);
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
            return convertToWheel(wheelDto);
        }

        public WheelDto convert(Wheel wheel)
        {
            return new WheelDto
            {
                Pressure = wheel.Pressure
            };
        }



        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return convertToIEnrollment(enrollmentDto);
        }

        

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            return new EnrollmentDto
            {
                Number = enrollment.Number,
                Serial = enrollment.Serial
            };
        }



        public Vehicle convert(VehicleDto vehicleDto)
        {
            CarColor color = vehicleDto.Color;

            List<Wheel> wheels = new List<Wheel>();
            foreach (WheelDto wheelDto in vehicleDto.Wheels)
            {
                Wheel r = convertToWheel(wheelDto);
                wheels.Add(r);
            }

            IEnrollment enrollment = convertToIEnrollment(vehicleDto.Enrollment);

            List<Door> doors = new List<Door>();
            foreach (DoorDto doorDto in vehicleDto.Doors)
            {
                Door door = convertToDoor(doorDto);
                doors.Add(door);
            }

            Engine engine = convertToEngine(vehicleDto.Engine);

            return new Vehicle(color, wheels, enrollment, doors, engine);
        }


        public VehicleDto convert(Vehicle vehicle)
        {
            return convertToVehicleDto(vehicle);
        }

        private VehicleDto convertToVehicleDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Color = vehicle.Color,
                Doors = vehicle.Doors,

            };
        }

        private static Engine convertToEngine(EngineDto engineDto)
        {
            return new Engine(engineDto.HorsePower, engineDto.IsStarted);
        }

        private static Door convertToDoor(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }

        private Wheel convertToWheel(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Pressure);
        }

        private IEnrollment convertToIEnrollment(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial,
                                                    enrollmentDto.Number);
        }

    }
}