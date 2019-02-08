using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using CarManagement.Services.CarManagement.Builders;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class DefaultDtoConverter 
    {
        private IEnrollmentProvider enrollmentProvider;
        

        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            

        }

        public EngineDto convert(IEngine engine)
        {
            EngineDto toDto = new EngineDto();
            toDto.HorsePower = engine.HorsePower;
            toDto.IsStarted  = engine.IsStarted;
            return toDto;

        }

        public DoorDto convert(IDoor door)
        {
            DoorDto toDto = new DoorDto ();
            toDto.IsOpen = door.IsOpen;
            return toDto;
       
        }

        public WheelDto convert(IWheel wheel)
        {
            WheelDto toDto = new WheelDto();
            toDto.Pressure = wheel.Pressure;
            return toDto;
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto toDto = new EnrollmentDto();
            toDto.Number = enrollment.Number;
            toDto.Serial = enrollment.Serial;
            return toDto;
        }

        public VehicleDto convert(IVehicle vehicle)
        {


            VehicleDto vehicleDto = new VehicleDto();
            vehicleDto.Color = vehicle.Color;
            vehicleDto.Enrollment = convertTo(vehicle.Enrollment);
            vehicleDto.Engine = convertTo(vehicle.Engine);
            List<DoorDto> dtoDoorsList = new List<DoorDto>();
            foreach (IDoor door in vehicle.Doors)
            {
                dtoDoorsList.Add(convert(door));
            }
            vehicleDto.Doors = dtoDoorsList.ToArray();
            List<WheelDto> dtoWheelsList = new List<WheelDto>();
            foreach (IWheel wheel in vehicle.Wheels)
            {
                dtoWheelsList.Add(convert(wheel));
            }
            vehicleDto.Wheels = dtoWheelsList.ToArray();

            return vehicleDto;

        }

        private EnrollmentDto convertTo(IEnrollment enrollment)
        {
            EnrollmentDto toDto = new EnrollmentDto();
            toDto.Number = enrollment.Number;
            toDto.Serial = enrollment.Serial;
            return toDto;
        }
        private EngineDto convertTo(IEngine engine)
        {
            EngineDto toDto = new EngineDto();
            toDto.HorsePower = engine.HorsePower;
            toDto.IsStarted = engine.IsStarted;
            return toDto;

        }
        private DoorDto converTo(IDoor door)
        {
            DoorDto toDto = new DoorDto();
            toDto.IsOpen = door.IsOpen;
            return toDto;

        }
        private WheelDto converTo(IWheel wheel)
        {
            WheelDto toDto = new WheelDto();
            toDto.Pressure = wheel.Pressure;
            return toDto;
        }

    }
}