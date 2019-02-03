using CarManagement.Models;
using CarManagement.Models.DTOs;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class DefaultDtoConverter : IDtoConverter
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
        private List<Door> doors;
        private CarColor color;
        
        //Memoria ->Fichero
        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
        }

        //Fichero ->Memoria
        public Engine convert(EngineDto engineDto)
        {
            this.engine = new Engine();
            this.engine.Horsepower = engineDto.HorsePower;
            this.engine.IsStarted = engineDto.IsStarted;
            return this.engine;
        }

        //Memoria ->Fichero
        public EngineDto convert(Engine engine)
        {
            this.engineDto = new EngineDto();
            this.engineDto.HorsePower = engine.Horsepower;
            this.engineDto.IsStarted = engine.IsStarted;
            return this.engineDto;
        }

        //Fichero ->Memoria
        public Vehicle convert(VehicleDto vehicleDto)
        {
            this.vehicleDto = new VehicleDto();
            this.color = new CarColor();
            this.wheels = new List<Wheel>();
            this.doors = new List<Door>();
            this.engine = new Engine();

            for (int i = 0; i < vehicleDto.Wheels.Length; i++)
            {
                this.wheels.Add(convert(vehicleDto.Wheels[i]));
            }
            for (int i = 0; i < vehicleDto.Doors.Length; i++)
            {
                this.doors.Add(convert(vehicleDto.Doors[i]));
            }
            this.vehicle.carColor = this.vehicleDto.Color;
            this.vehicle.Engine = convert(vehicleDto.Engine);
            this.color = vehicleDto.Color;
            this.enrollment = convert(vehicleDto.Enrollment);
            this.vehicle = new Vehicle(this.wheels, this.doors, this.engine, this.color, this.enrollment);
            return this.vehicle;
        }

        //Memoria ->Fichero
        public VehicleDto convert(Vehicle vehicle)
        {
            this.vehicleDto = new VehicleDto();
            for (int i = 0; i < vehicle.Wheels.Length; i++)
            {
                this.vehicleDto.Wheels[i] = convert(vehicle.Wheels[i]);
                i++;
            }

            for (int i = 0; i < vehicle.Doors.Length; i++)
            {
                this.vehicleDto.Doors[i] = convert(vehicle.Doors[i]);
                i++;
            }
            this.vehicleDto.Color = vehicle.carColor;
            this.vehicleDto.Enrollment = convert(vehicle.Enrollment);
            return this.vehicleDto;
        }

        //Fichero ->Memoria
        public Door convert(DoorDto doorDto)
        {
            this.door = new Door();
            if (doorDto.IsOpen == true)
            {
                this.door.open();
            }
            return this.door;
        }

        //Memoria ->Fichero
        public DoorDto convert(Door door)
        {
            this.doorDto = new DoorDto();
            this.doorDto.IsOpen = door.IsOpen;
            return this.doorDto;
        }

        //Fichero ->Memoria
        public Wheel convert(WheelDto wheelDto)
        {
            this.wheel = new Wheel();
            this.wheel.Pressure = wheelDto.Pressure;
            return this.wheel;
        }

        //Memoria ->Fichero
        public WheelDto convert(Wheel wheel)
        {
            this.wheelDto = new WheelDto();
            this.wheelDto.Pressure = wheel.Pressure;
            return this.wheelDto;
        }

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