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
            {
                this.enrollmentDtoList.Add(
                    new EnrollmentDto(
                        enrollment.Serial,
                        enrollment.Number));
            }
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
        public VehicleDto Edit(VehicleDto vehicleDto)
        {
            set(vehicleDto);
            return getVehicle(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number);
        }
    }
}