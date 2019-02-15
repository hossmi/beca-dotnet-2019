using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
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
            Engine engine = new Engine(engineDto.HorsePower, engineDto.IsStarted);
            return engine;
        }

        public EngineDto convert(Engine engine)
        {
            EngineDto engineDto = new EngineDto();
            engineDto.HorsePower = engine.HorsePower;
            engineDto.IsStarted = engine.IsStarted;

            return engineDto;
        }

        public Vehicle convert(VehicleDto vehicleDto)
        {
            List<Door> doors = new List<Door>();
            foreach (DoorDto door in vehicleDto.Doors)
            {
                doors.Add(convert(door));
            }

            List<Wheel> wheels = new List<Wheel>();
            foreach (WheelDto wheel in vehicleDto.Wheels)
            {
                wheels.Add(convert(wheel));
            }

            return new Vehicle(convert(vehicleDto.Engine), doors, wheels, vehicleDto.Color, convert(vehicleDto.Enrollment));
        }

        public VehicleDto convert(Vehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto();
            vehicleDto.Color = vehicle.Color;
            vehicleDto.Engine = convert(vehicle.Engine);
            vehicleDto.Enrollment = convert(vehicle.Enrollment);
            List<DoorDto> dtoDoorsList = new List<DoorDto>();
            foreach (Door door in vehicle.Doors)
            {
                dtoDoorsList.Add(convert(door));
            }
            vehicleDto.Doors = dtoDoorsList.ToArray();

            List<WheelDto> dtoWheelsList = new List<WheelDto>();
            foreach (Wheel wheel in vehicle.Wheels)
            {
                dtoWheelsList.Add(convert(wheel));
            }
            vehicleDto.Wheels = dtoWheelsList.ToArray();

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
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            enrollmentDto.Serial = enrollment.Serial;
            enrollmentDto.Number = enrollment.Number;

            return enrollmentDto;
        }
    }
}