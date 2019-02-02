using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using CarManagement.Services.CarManagement.Builders;
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

        public IEngine convert(EngineDto engineDto)
        {
            IEngine toMemory = new Engine(engineDto .HorsePower);
            if (engineDto .IsStarted == true)
            {
                toMemory.start(); 
            }
            toMemory.stop();
            return toMemory;
        }

        public EngineDto convert(IEngine engine)
        {
            EngineDto toDto = new EngineDto();
            toDto.HorsePower = engine.HorsePower;
            toDto.IsStarted  = engine.IsStarted;
            return toDto;

        }
        public IDoor convert(DoorDto doorDto)
        {
            IDoor toMemory = new Door();
            if (doorDto.IsOpen == true)
            {
                toMemory.open();
            }
            toMemory.close();
            return toMemory;
        }

        public DoorDto convert(IDoor door)
        {
            DoorDto toDto = new DoorDto ();
            toDto.IsOpen = door.IsOpen;
            return toDto;
       
        }

        public IWheel convert(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Pressure);

        }
       
        public WheelDto convert(IWheel wheel)
        {
            WheelDto toDto = new WheelDto();
            toDto.Pressure = wheel.Pressure;
            return toDto;
        }

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        public EnrollmentDto  convert(IEnrollment enrollment)
        {
            EnrollmentDto toDto = new EnrollmentDto();
            toDto.Number = enrollment.Number;
            toDto.Serial = enrollment.Serial;
            return toDto;           
        }

        public IVehicle convert(VehicleDto vehicleDto)
        {
            List<IWheel> listWheels = new List<IWheel>();
            List<IDoor> listDoor = new List<IDoor>();
            IEnrollment enrollment = convertTo(vehicleDto.Enrollment);
            foreach (WheelDto wheelsDto in vehicleDto.Wheels)
            {
                IWheel setwheel = convertTo(wheelsDto);
                listWheels.Add(setwheel);

            }
            foreach (DoorDto doorDto in vehicleDto.Doors)
            {
                IDoor setdoor = convertTo(doorDto);
                listDoor.Add(setdoor);

            }
            IEngine engine = convertTo(vehicleDto.Engine);
    
      
            return new Vehicle(vehicleDto.Color, listWheels, enrollment, listDoor, engine);
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

        //Metodos privados
        private IWheel convertTo(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Pressure);

        }
        private IDoor convertTo(DoorDto doorDto)
        {
            IDoor toMemory = new Door();
            if (doorDto.IsOpen == true)
            {
                toMemory.open();
            }
            toMemory.close();
            return toMemory;
        }
        private IEnrollment convertTo(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
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
        private IEngine convertTo(EngineDto engineDto)
        {

            IEngine toMemory = new Engine(engineDto .HorsePower);
            if (engineDto.IsStarted == true)
            {
                toMemory.start();
            }
            toMemory.stop();
            return toMemory;
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