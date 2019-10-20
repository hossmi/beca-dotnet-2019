using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using System.Collections.Generic;
using System.Linq;
using WebCarManager.Controllers;

namespace WebCarManager.Models
{
    public class Vehicle : AbstractController
    {
        private readonly IVehicleStorage vehicleStorage;
        private readonly IVehicleBuilder vehicleBuilder;
        private IEnumerable<IEnrollment> enrollmentEnum;
        private IList<EnrollmentDto> enrollmentDtoList;
        private VehicleDto vehicleDto;

        public Vehicle()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.vehicleBuilder = getService<IVehicleBuilder>();
        }
        public IList<EnrollmentDto> enrollmentDTOList()
        {
            this.enrollmentEnum = this.vehicleStorage
                .get()
                .Select(vehicle => vehicle.Enrollment);
            this.enrollmentDtoList = new List<EnrollmentDto>();
            foreach (IEnrollment enrollment in this.enrollmentEnum)
                this.enrollmentDtoList.Add(
                    new EnrollmentDto(
                        enrollment.Serial,
                        enrollment.Number));
            return this.enrollmentDtoList;
}
        public VehicleDto getVehicle(string serial, int number)
        {
            return this.vehicleBuilder.export(
                this.vehicleStorage
                    .get()
                    .whereEnrollmentIs(this.vehicleBuilder.import(serial, number))
                    .Single());
        }
        public void set(VehicleDto vehicleDto)
        {
            this.vehicleStorage.set(this.vehicleBuilder.import(vehicleDto));
        }
        public VehicleDto Edit(CarColor color, EngineDto engineDto, int Doors, int Wheels, EnrollmentDto enrollmentDto)
        {
            this.vehicleDto = new VehicleDto();
            this.vehicleDto.Enrollment = enrollmentDto;
            this.vehicleDto.Color = color;
            this.vehicleDto.Engine = engineDto;
            this.vehicleDto.Doors = new DoorDto[Doors];
            for (int i = 0; i < Doors; i++)
                this.vehicleDto.Doors[i] = new DoorDto();
            this.vehicleDto.Wheels = new WheelDto[Wheels];
            for (int i = 0; i < Wheels; i++)
                this.vehicleDto.Wheels[i] = new WheelDto();
            set(this.vehicleDto);
            return getVehicle(this.vehicleDto.Enrollment.Serial, this.vehicleDto.Enrollment.Number);
        }
    }
}