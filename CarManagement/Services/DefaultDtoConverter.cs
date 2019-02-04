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
    }
}