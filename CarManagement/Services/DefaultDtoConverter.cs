using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public class DefaultDtoConverter : IDtoConverter
    {
        private IEnrollmentProvider enrollmentProvider;

        //Memoria ->Fichero
        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        //Fichero ->Memoria
        public Engine convert(EngineDto engineDto)
        {
            throw new System.NotImplementedException();
        }

        //Memoria ->Fichero
        public EngineDto convert(Engine engine)
        {
            EngineDto engineDto = new EngineDto();
            engineDto.HorsePower = engine.Horsepower;
            engineDto.IsStarted = engine.IsStarted;

            return engineDto;
        }

        //Fichero ->Memoria
        public Vehicle convert(VehicleDto vehicleDto)
        {
            throw new System.NotImplementedException();
        }

        //Memoria ->Fichero
        public VehicleDto convert(Vehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto();
            vehicleDto.Color = vehicle.carcolor;
            vehicleDto.Doors = vehicle.Doors;
            return vehicleDto;
        }

        //Fichero ->Memoria
        public Door convert(DoorDto doorDto)
        {
            throw new System.NotImplementedException();
        }

        //Memoria ->Fichero
        public DoorDto convert(Door door)
        {
            DoorDto doorDto = new DoorDto();
            doorDto.IsOpen = door.IsOpen;
            return doorDto;
        }

        //Fichero ->Memoria
        public Wheel convert(WheelDto wheelDto)
        {
            throw new System.NotImplementedException();
        }

        //Memoria ->Fichero
        public WheelDto convert(Wheel wheel)
        {
            WheelDto wheelDto = new WheelDto();
            wheelDto.Pressure = wheel.Pressure;
            return wheelDto;
        }

        //Fichero ->Memoria
        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            throw new System.NotImplementedException();
        }

        //Memoria ->Fichero
        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            enrollmentDto.Number = enrollment.Number;
            enrollmentDto.Serial = enrollment.Serial;
            return enrollmentDto;
        }
    }
}