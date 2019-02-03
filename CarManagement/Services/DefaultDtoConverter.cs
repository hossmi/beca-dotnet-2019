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
        private WheelDto wheelDto;
        private Wheel wheel;
        private List<Wheel> wheels;
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
            this.color = vehicleDto.Color;
            this.wheels = new List<Wheel>();
            this.doors = new List<Door>();
            for (int i = 0; i < vehicleDto.Wheels.Length; i++)
            {
                Wheel wheel = new Wheel();
                this.wheels.Add(wheel);
            }
            for (int i = 0; i < vehicleDto.Doors.Length; i++)
            {
                Door door = new Door();
                this.doors.Add(door);
            }
            this.vehicle.Engine.Horsepower = vehicleDto.Engine.HorsePower;
            this.vehicle.Engine.IsStarted = vehicleDto.Engine.IsStarted;
            this.enrollment = this.enrollmentProvider.import(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number);
            this.vehicle = new Vehicle(this.wheels, this.doors, this.vehicle.Engine, this.color, this.enrollment);
            return this.vehicle;
        }

        //Memoria ->Fichero
        public VehicleDto convert(Vehicle vehicle)
        {
            this.vehicleDto = new VehicleDto();
            System.Array.Copy(vehicle.Wheels, this.vehicleDto.Wheels, vehicle.Wheels.Length);
            System.Array.Copy(vehicle.Doors, this.vehicleDto.Doors, vehicle.Doors.Length);
            this.vehicleDto.Color = vehicle.carColor;
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