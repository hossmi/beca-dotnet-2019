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
        private WheelDto[] wheelsArr;
        private DoorDto[] doorsArr;
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

            this.vehicleDto = vehicleDto;
            for (int i = 0; i < vehicleDto.Wheels.Length; i++)
            {
                Wheel wheel = new Wheel();
                //wheel.Pressure = this.vehicleDto.Wheels[i].Pressure;
                this.wheels.Add(wheel);
            }
            for (int i = 0; i < vehicleDto.Doors.Length; i++)
            {
                Door door = new Door();
                if (vehicleDto.Doors[i].IsOpen == true)
                {
                    door.open();
                }
                this.doors.Add(door);
            }
            this.vehicle.carColor = this.vehicleDto.Color;
            this.vehicle.Engine.Horsepower = this.vehicleDto.Engine.HorsePower;
            this.vehicle.Engine.IsStarted = this.vehicleDto.Engine.IsStarted;

            //this.color = vehicleDto.Color;
            this.engine.Horsepower = this.vehicleDto.Engine.HorsePower;
            this.engine.IsStarted = this.vehicleDto.Engine.IsStarted;
            this.enrollment = this.enrollmentProvider.import(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number);
            this.vehicle = new Vehicle(this.wheels, this.doors, this.engine, this.color, this.enrollment);
            return this.vehicle;
        }

        //Memoria ->Fichero
        public VehicleDto convert(Vehicle vehicle)
        {
            this.vehicleDto = new VehicleDto();
            this.wheelsArr = new WheelDto[vehicle.Wheels.Length];
            for (int aux = 0; aux < vehicle.Wheels.Length; aux++)
            {
                this.wheelDto = new WheelDto();
                this.wheelDto.Pressure = vehicle.Wheels[aux].Pressure;
                this.wheelsArr[aux] = this.wheelDto;
                aux++;
            }
            this.vehicleDto.Wheels = this.wheelsArr;

            this.doorsArr = new DoorDto[vehicle.Doors.Length];
            for (int aux = 0; aux < vehicle.Doors.Length; aux++)
            {
                this.doorDto = new DoorDto();
                this.doorDto.IsOpen = vehicle.Doors[aux].IsOpen;
                this.doorsArr[aux] = this.doorDto;
                aux++;
            }
            this.vehicleDto.Doors = this.doorsArr;
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