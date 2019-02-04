using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

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
            IEngine engine = new IEngine(engineDto.HorsePower);

            if (engineDto != null)
                engine.IsStarted = engineDto.IsStarted;

            return engine;
        }

        public EngineDto convert(IEngine engine)
        {
            EngineDto engineDto = new EngineDto();
            engineDto.HorsePower = engine.HorsePower;
            engineDto.IsStarted = engine.IsStarted;

            return engineDto;
        }

        public IDoor convert(DoorDto doorDto)
        {
            Door door = new Door();

            if (doorDto != null)
                door.IsOpen = doorDto.IsOpen;

            return door;
        }

        public DoorDto convert(IDoor door)
        {
            DoorDto doorDto = new DoorDto();
            doorDto.IsOpen = door.IsOpen;

            return doorDto;
        }

        public IWheel convert(WheelDto wheelDto)
        {
            Wheel wheel = new Wheel();

            if (wheelDto != null)
                wheel.Pressure = wheelDto.Pressure;

            return wheel;
        }

        public WheelDto convert(IWheel wheel)
        {
            WheelDto wheelDto = new WheelDto();
            wheelDto.Pressure = wheel.Pressure;

            return wheelDto;
        }

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            enrollmentDto.Serial = enrollment.Serial;
            enrollmentDto.Number = enrollment.Number;

            return enrollmentDto;
        }

        public IVehicle convert(VehicleDto vehicleDto)
        {
            IVehicle vehicle;
            List<Door> doors = new List<Door>();
            List<Wheel> wheels = new List<Wheel>();

            foreach (DoorDto doorDto in vehicleDto.Doors)
            {
                doors.Add(convert(doorDto));
            }

            foreach (WheelDto wheelDto in vehicleDto.Wheels)
            {
                wheels.Add(convert(wheelDto));
            }

            IEngine engine = convert(vehicleDto.Engine);
            IEnrollment enrollment = convert(vehicleDto.Enrollment);
            CarColor Color = vehicleDto.Color;

            vehicle = new Vehicle(doors, wheels, engine, enrollment, Color);
            return vehicle;
        }

        public VehicleDto convert(IVehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto();

            DoorDto[] doorsDto = new DoorDto[vehicle.DoorsCount];
            WheelDto[] wheelsDto = new WheelDto[vehicle.WheelCount];



            for (int i = vehicle.DoorsCount - 1; i > 0; i--)
            {
                doorsDto[i] = convert(vehicle.Doors[i]);
            }

            for (int i = vehicle.WheelCount - 1; i > 0; i--)
            {
                wheelsDto[i] = convert(vehicle.Wheels[i]);
            }

            EngineDto engineDto = convert(vehicle.Engine);
            EnrollmentDto enrollmentDto = convert(vehicle.Enrollment);

            vehicleDto.Doors = doorsDto;
            vehicleDto.Wheels = wheelsDto;
            vehicleDto.Color = vehicle.Color;
            vehicleDto.Engine = engineDto;
            vehicleDto.Enrollment = enrollmentDto;

            return vehicleDto;

            public IDoor convert(DoorDto doorDto)
            {
                throw new System.NotImplementedException();
            }

            public DoorDto convert(IDoor door)
            {
                DoorDto doorDto = new DoorDto();
                doorDto.IsOpen = door.IsOpen;

                return doorDto;
            }

            public IWheel convert(WheelDto wheelDto)
            {
                Wheel wheel = new Wheel();

                if (wheelDto != null)
                    wheel.Pressure = wheelDto.Pressure;

                return wheel;
            }

            public WheelDto convert(IWheel wheel)
            {
                WheelDto wheelDto = new WheelDto();
                wheelDto.Pressure = wheel.Pressure;

                return wheelDto;
            }

            public IEnrollment convert(EnrollmentDto enrollmentDto)
            {
                return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
            }

            public EnrollmentDto convert(IEnrollment enrollment)
            {
                EnrollmentDto enrollmentDto = new EnrollmentDto();
                enrollmentDto.Serial = enrollment.Serial;
                enrollmentDto.Number = enrollment.Number;

                return enrollmentDto;
            }

            public Vehicle convert(VehicleDto vehicleDto)
            {
                Vehicle vehicle;
                List<Door> doors = new List<Door>();
                List<Wheel> wheels = new List<Wheel>();

                foreach (DoorDto doorDto in vehicleDto.Doors)
                {
                    doors.Add(convert(doorDto));
                }

                foreach (WheelDto wheelDto in vehicleDto.Wheels)
                {
                    wheels.Add(convert(wheelDto));
                }

                Engine engine = convert(vehicleDto.Engine);
                IEnrollment enrollment = convert(vehicleDto.Enrollment);
                CarColor Color = vehicleDto.Color;

                vehicle = new Vehicle(doors, wheels, engine, enrollment, Color);
                return vehicle;
            }

            public VehicleDto convert(Vehicle vehicle)
            {
                VehicleDto vehicleDto = new VehicleDto();

                DoorDto[] doorsDto = new DoorDto[vehicle.DoorsCount];
                WheelDto[] wheelsDto = new WheelDto[vehicle.WheelCount];



                for (int i = vehicle.DoorsCount - 1; i > 0; i--)
                {
                    doorsDto[i] = convert(vehicle.Doors[i]);
                }

                for (int i = vehicle.WheelCount - 1; i > 0; i--)
                {
                    wheelsDto[i] = convert(vehicle.Wheels[i]);
                }

                EngineDto engineDto = convert(vehicle.Engine);
                EnrollmentDto enrollmentDto = convert(vehicle.Enrollment);

                vehicleDto.Doors = doorsDto;
                vehicleDto.Wheels = wheelsDto;
                vehicleDto.Color = vehicle.Color;
                vehicleDto.Engine = engineDto;
                vehicleDto.Enrollment = enrollmentDto;

                return vehicleDto;
            }
        }

    }