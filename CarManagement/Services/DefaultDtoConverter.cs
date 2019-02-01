using CarManagement.Models;
using CarManagement.Models.DTOs;

namespace CarManagement.Services
{
    public class DefaultDtoConverter : IDtoConverter
    {
        private IEnrollmentProvider enrollmentProvider;
        private EnrollmentDto enrollmentDto = new EnrollmentDto();
        private VehicleDto vehicleDto = new VehicleDto();
        private EngineDto engineDto = new EngineDto();
        private Engine engine = new Engine();
        private DoorDto doorDto = new DoorDto();
        private Door door = new Door();
        private WheelDto wheelDto = new WheelDto();
        private Wheel wheel = new Wheel();


        //Memoria ->Fichero
        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        //Fichero ->Memoria
        public Engine convert(EngineDto engineDto)
        {
            this.engine.Horsepower = engineDto.HorsePower;
            this.engine.IsStarted = engineDto.IsStarted;
            return this.engine;
        }

        //Memoria ->Fichero
        public EngineDto convert(Engine engine)
        {
            this.engineDto.HorsePower = engine.Horsepower;
            this.engineDto.IsStarted = engine.IsStarted;
            return this.engineDto;
        }

        //Fichero ->Memoria
        public Vehicle convert(VehicleDto vehicleDto)
        {
            throw new System.NotImplementedException();
        }

        //Memoria ->Fichero
        public VehicleDto convert(Vehicle vehicle)
        {
            this.vehicleDto.Color = vehicle.carColor;
            //this.vehicleDto.Doors = vehicle.Doors;
            return this.vehicleDto;
        }

        //Fichero ->Memoria
        public Door convert(DoorDto doorDto)
        {
            if(doorDto.IsOpen == true)
            {
                this.door.open();
            }
            return this.door;
        }

        //Memoria ->Fichero
        public DoorDto convert(Door door)
        {
            this.doorDto.IsOpen = door.IsOpen;
            return this.doorDto;
        }

        //Fichero ->Memoria
        public Wheel convert(WheelDto wheelDto)
        {
            this.wheel.Pressure = wheelDto.Pressure;
            return this.wheel;
        }

        //Memoria ->Fichero
        public WheelDto convert(Wheel wheel)
        {
            this.wheelDto.Pressure = wheel.Pressure;
            return this.wheelDto;
        }

        //Fichero ->Memoria
        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            throw new System.NotImplementedException();
        }

        //Memoria ->Fichero
        public EnrollmentDto convert(IEnrollment enrollment)
        {
            this.enrollmentDto.Number = enrollment.Number;
            this.enrollmentDto.Serial = enrollment.Serial;
            return this.enrollmentDto;
        }
    }
}