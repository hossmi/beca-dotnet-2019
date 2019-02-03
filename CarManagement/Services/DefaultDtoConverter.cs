using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class DefaultDtoConverter 
    {
        private IEnrollmentProvider enrollmentProvider;
        private IEnrollment enrollment;
        private VehicleBuilder VehicleBuilder;



        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }


        public IEngine convert(EngineDto engineDto)
        {
            return convertToEngine(engineDto);
        }

        public EngineDto convert(IEngine engine)
        {
            return convertToEngineDto(engine);
        }

        
        public IDoor convert(DoorDto doorDto)
        {
            return convertToDoor(doorDto);
        }

        public DoorDto convert(IDoor door)
        {
            return convertToDoorDto(door);
        }

       

        public IWheel convert(WheelDto wheelDto)
        {
            return convertToWheel(wheelDto);
        }

        public WheelDto convert(IWheel wheel)
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



        public IVehicle convert(VehicleDto vehicleDto)
        {
            CarColor color = vehicleDto.Color;

            List<IWheel> wheels = new List<IWheel>();
            foreach (WheelDto wheelDto in vehicleDto.Wheels)
            {
                IWheel r = convertToWheel(wheelDto);
                wheels.Add(r);
            }

            IEnrollment enrollment = convertToIEnrollment(vehicleDto.Enrollment);

            List<IDoor> doors = new List<IDoor>();
            foreach (DoorDto doorDto in vehicleDto.Doors)
            {
                IDoor door = convertToDoor(doorDto);
                doors.Add(door);
            }

            IEngine engine = convertToEngine(vehicleDto.Engine);

            IVehicle vehicle = 

            return new Vehicle(color, wheels, enrollment, doors, engine);
        }

        public VehicleDto convert(IVehicle vehicle)
        {
            CarColor color = vehicle.Color;

            EngineDto engineDto = convertToEngineDto(vehicle.Engine);

            EnrollmentDto enrollmentDto = convertToIEnrollmentDto(vehicle.Enrollment);

            WheelDto[] wheelDtos = new WheelDto[vehicle.Wheels.Length];
            int auxWheel = 0;
            foreach (IWheel wheen in vehicle.Wheels)
            {
                WheelDto wheelDto = convertToWheelDto(wheen);
                wheelDtos[auxWheel] = wheelDto;
                auxWheel++;
            }

            DoorDto[] doorDtos = new DoorDto[vehicle.Doors.Length];
            int auxDoor = 0;
            foreach (IDoor door in vehicle.Doors)
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
                Number = enrollment.Number,
                Serial = enrollment.Serial,
            };
        }
        private IEnrollment convertToIEnrollment(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial,
                                                  enrollmentDto.Number);
        }

        private static IEngine convertToEngine(EngineDto engineDto)
        {
            return new Engine(engineDto.HorsePower, engineDto.IsStarted);
        }
        private static EngineDto convertToEngineDto(IEngine engine)
        {
            return new EngineDto
            {
                HorsePower = engine.HorsePower,
                IsStarted = engine.IsStarted
            };
        }

        private static DoorDto convertToDoorDto(IDoor door)
        {
            return new DoorDto
            {
                IsOpen = door.IsOpen
            };
        }
        private static IDoor convertToDoor(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }

        private static IWheel convertToWheel(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Pressure);
        }
        private static WheelDto convertToWheelDto(IWheel wheel)
        {
            return new WheelDto
            {
                Pressure = wheel.Pressure
            };
        }
    }
}