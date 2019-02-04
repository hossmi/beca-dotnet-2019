using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class DefaultDtoConverter 
    {/*
        private IEnrollmentProvider enrollmentProvider;
        //private IEnrollment enrollment;
        private VehicleBuilder vehicleBuilder;
        /*
        
        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }
        public DefaultDtoConverter()
        {
            this.vehicleBuilder = new VehicleBuilder();
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
            return this.vehicleBuilder.convert(vehicleDto);
        }
        


        
        
        
        /*
        private IEngine convertToEngine(EngineDto engineDto)
        {
            return this.vehicleBuilder.convert(engineDto);
        }
        

        /*
        private IDoor convertToDoor(DoorDto doorDto)
        {
            return this.vehicleBuilder.convert(doorDto);
        }
        

        /*
        private IWheel convertToWheel(WheelDto wheelDto)
        {
            return this.vehicleBuilder.convert(wheelDto);
        }*/
        
    }
}