using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class DefaultDtoConverter 
    {
        private IEnrollmentProvider enrollmentProvider;
        private IEnrollment enrollment;
        private EnrollmentDto enrollmentDto;
        private VehicleDto vehicleDto;
        private Vehicle vehicle;
        private EngineDto engineDto;
        private Engine engine;
        private DoorDto doorDto;
        private Door door;
        private Wheel wheel;
        private List<Wheel> wheels;
        private WheelDto wheelDto;
        private WheelDto[] wheelsDto;
        private List<Door> doors;
        private DoorDto[] doorsDto;
        private CarColor color;
        
        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        public IEngine convert(EngineDto engineDto)
        {
            this.engine = new Engine();
            this.engine.Horsepower = engineDto.HorsePower;
            this.engine.IsStarted = engineDto.IsStarted;
            return this.engine;
        }

        public EngineDto convert(IEngine engine)
        {
            this.engineDto = new EngineDto();
            this.engineDto.HorsePower = engine.Horsepower;
            this.engineDto.IsStarted = engine.IsStarted;
            return this.engineDto;
        }

        public IVehicle convert(VehicleDto vehicleDto)
        {
            this.color = new CarColor();
            this.wheels = new List<Wheel>();
            this.doors = new List<Door>();
            this.engine = new Engine();
            this.color = new CarColor();
            for (int i = 0; i < vehicleDto.Wheels.Length; i++)
            {
                this.wheels.Add(convert(vehicleDto.Wheels[i]));
            }
            for (int i = 0; i < vehicleDto.Doors.Length; i++)
            {
                this.doors.Add(convert(vehicleDto.Doors[i]));
            }
            this.engine = convert(vehicleDto.Engine);
            this.color = vehicleDto.Color;
            this.enrollment = convert(vehicleDto.Enrollment);
            this.vehicle = new Vehicle(this.wheels, this.doors, this.engine, this.color, this.enrollment);
            return this.vehicle;
        }

        public VehicleDto convert(IVehicle vehicle)
        {
            this.vehicleDto = new VehicleDto();
            this.vehicleDto.Color = new CarColor();
            this.vehicleDto.Doors = new DoorDto[vehicle.Doors.Length];
            this.vehicleDto.Wheels = new WheelDto[vehicle.Wheels.Length];
            this.vehicleDto.Enrollment = new EnrollmentDto();
            this.vehicleDto.Engine = new EngineDto();

            for (int i = 0; i < vehicle.Wheels.Length; i++)
            {
                this.vehicleDto.Wheels[i] = convert(vehicle.Wheels[i]);
            }

            for (int i = 0; i < vehicle.Doors.Length; i++)
            {
                this.vehicleDto.Doors[i] = convert(vehicle.Doors[i]);
            }

            this.vehicleDto.Color = vehicle.carColor;
            this.vehicleDto.Enrollment = convert(vehicle.Enrollment);
            this.vehicleDto.Engine = convert(vehicle.Engine);
            return this.vehicleDto;
        }

        public IDoor convert(DoorDto doorDto)
        {
            this.door = new Door();
            if (doorDto.IsOpen == true)
            {
                this.door.open();
            }
            return this.door;
        }

        public DoorDto convert(IDoor door)
        {
            this.doorDto = new DoorDto();
            this.doorDto.IsOpen = door.IsOpen;
            return this.doorDto;
        }

        public IWheel convert(WheelDto wheelDto)
        {
            this.wheel = new Wheel();
            this.wheel.Pressure = wheelDto.Pressure;
            return this.wheel;
        }

        public WheelDto convert(IWheel wheel)
        {
            this.wheelDto = new WheelDto();
            this.wheelDto.Pressure = wheel.Pressure;
            return this.wheelDto;
        }

        //IEnrollment
        //Fichero ->Memoria
        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        //Memoria ->Fichero
        public EnrollmentDto convert(IEnrollment enrollment)
        {
            this.enrollmentDto = new EnrollmentDto();
            this.enrollmentDto.Number = enrollment.Number;
            this.enrollmentDto.Serial = enrollment.Serial;
            return this.enrollmentDto;
        }
    }
}