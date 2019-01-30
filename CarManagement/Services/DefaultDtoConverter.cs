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
            return convertToEngineDto(engine);
        }

        


        public Door convert(DoorDto doorDto)
        {
            return convertToDoor(doorDto);
        }

        public DoorDto convert(Door door)
        {
            return convertDootDto(door);
        }

       

        public Wheel convert(WheelDto wheelDto)
        {
            return convertToWheel(wheelDto);
        }


        public WheelDto convert(Wheel wheel)
        {
            return convertToWheelDto(wheel);
        }

        

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return convertToIEnrollment(enrollmentDto);
        }
        public EnrollmentDto convert(IEnrollment enrollment)
        {
            return convertToIEnrollmentDto(enrollment);
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
            CarColor color = vehicle.Color;

            EngineDto engineDto = convertToEngineDto(vehicle.Engine);

            EnrollmentDto enrollmentDto = convertToIEnrollmentDto(vehicle.Enrollment);

            WheelDto[] wheelDtos = new WheelDto[vehicle.WheelCount];
            int auxWheel = 0;
            foreach (Wheel wheen in vehicle.Wheels)
            {
                WheelDto wheelDto = convertToWheelDto(wheen);
                wheelDtos[auxWheel] = wheelDto;
                auxWheel++;
            }

            DoorDto[] doorDtos = new DoorDto[vehicle.DoorsCount];
            int auxDoor = 0;
            foreach (Door door in vehicle.Doors)
            {
                DoorDto doorDto = convertToDoorDto(door);
                doorDtos[auxDoor] = doorDto;
                auxDoor++;
            }

            return new VehicleDto
            {
                Color = color,
                Engine = engineDto,
                Enrollment = enrollmentDto,
                Wheels = wheelDtos,
                Doors = doorDtos
            };
        }
       

        private static EnrollmentDto convertToIEnrollmentDto(IEnrollment enrollment)
        {
            return new EnrollmentDto
            {
                 
            };
        }
        private IEnrollment convertToIEnrollment(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial,
                                                  enrollmentDto.Number);
        }

        private static Engine convertToEngine(EngineDto engineDto)
        {
            return new Engine(engineDto.HorsePower, engineDto.IsStarted);
        }
        private static EngineDto convertToEngineDto(Engine engine)
        {
            return new EngineDto
            {
                HorsePower = engine.HorsePower,
                IsStarted = engine.IsStarted
            };
        }

        private static DoorDto convertToDoorDto(Door door)
        {
            return new DoorDto
            {
                IsOpen = door.IsOpen
            };
        }
        private static Door convertToDoor(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }

        private static Wheel convertToWheel(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Pressure);
        }
        private static WheelDto convertToWheelDto(Wheel wheel)
        {
            return new WheelDto
            {
                Pressure = wheel.Pressure
            };
        }

        

    }
}