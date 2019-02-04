using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class DefaultDtoConverter 
    {
        private IEnrollmentProvider enrollmentProvider;
        private IVehicleBuilder vehicleBuilder;

        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.vehicleBuilder = new VehicleBuilder(this.enrollmentProvider);
        }

        public IVehicle convert(VehicleDto vehicleDto)
        {
            return this.vehicleBuilder.import(vehicleDto);
        }

        public VehicleDto convert(IVehicle vehicle)
        {
            return this.vehicleBuilder.export(vehicle);
        }

        /*public IEngine convert(EngineDto engineDto)
        {
            return this.vehicleBuilder.import(engineDto.HorsePower, engineDto.IsStarted);
        }

        public EngineDto convert(IEngine engine)
        {
            EngineDto engineDto = new EngineDto
            {
                HorsePower = engine.HorsePower,
                IsStarted = engine.IsStarted
            };

            return engineDto;
        }

        

        public IDoor convert(DoorDto doorDto)
        {
            return new Door(doorDto.IsOpen);
        }

        public DoorDto convert(IDoor door)
        {
            DoorDto doorDto = new DoorDto
            {
                IsOpen = door.IsOpen
            };

            return doorDto;
        }

        public IWheel convert(WheelDto wheelDto)
        {
            Wheel wheel = new Wheel
            {
                Pressure = wheelDto.Pressure
            };

            return wheel;
        }

        public WheelDto convert(IWheel wheel)
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

        private DoorDto[] createDoorsDto(IVehicle vehicle)
        {
            DoorDto[] doorsDto = new DoorDto[vehicle.Doors.Length];
            for (int i = 0; i < vehicle.Doors.Length; i++)
            {
                doorsDto[i] = convert(vehicle.Doors[i]);
            }

            return doorsDto;
        }

        /*private List<IDoor> createDoors(VehicleDto vehicleDto)
        {
            List<IDoor> doors = new List<IDoor>();
            for (int i = 0; i < vehicleDto.Doors.Length; i++)
            {
                doors.Add(convert(vehicleDto.Doors[i]));
            }

            return doors;
        }

        private WheelDto[] createWheelDto(IVehicle vehicle)
        {
            WheelDto[] wheelsDto = new WheelDto[vehicle.Wheels.Length];
            for (int i = 0; i < vehicle.Wheels.Length; i++)
            {
                wheelsDto[i] = convert(vehicle.Wheels[i]);
            }

            return wheelsDto;
        }

        /*private List<Wheel> createWheels(VehicleDto vehicleDto)
        {
            List<Wheel> wheels = new List<Wheel>();
            for (int i = 0; i < vehicleDto.Wheels.Length; i++)
            {
                wheels.Add(convert(vehicleDto.Wheels[i]));
            }

            return wheels;
        }*/
    }
}