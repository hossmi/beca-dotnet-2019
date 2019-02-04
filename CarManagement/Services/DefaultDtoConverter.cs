using CarManagement.Core;
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
            this.vehicleBuilder = new VehicleBuilder(enrollmentProvider);

        }

        public IEngine convert(EngineDto engineDto)
        {
            IEngine e = new Engine(engineDto.HorsePower);

            if (engineDto.IsStarted)
                e.start();
       
            return e;
        }

        public EngineDto convert(IEngine engine)
        {
            EngineDto eDto = new EngineDto();
            eDto.IsStarted = engine.IsStarted;
            eDto.HorsePower = engine.HorsePower;

            return eDto;
        }

        public IVehicle convert(VehicleDto vehicleDto)
        {
            IVehicle v;

            List<IWheel> wheels = new List<IWheel>();
            List<IDoor> doors = new List<IDoor>();
            IEngine engine;
            IEnrollment enrollment;

            foreach (WheelDto w in vehicleDto.Wheels)
            {
                wheels.Add(convert(w));

            }

            foreach (DoorDto d in vehicleDto.Doors)
            {
                doors.Add(convert(d));
            }
            engine = convert(vehicleDto.Engine);

            enrollment = convert(vehicleDto.Enrollment);

            v = new Vehicle(wheels, doors, engine, vehicleDto.Color, enrollment);

            return v;
        }

        public VehicleDto convert(IVehicle vehicle)
        {
            VehicleDto vDto = new VehicleDto();
            vDto.Color = vehicle.Color;
            vDto.Engine = convert(vehicle.Engine);
            vDto.Enrollment = convert(vehicle.Enrollment);
            vDto.Wheels = new WheelDto[vehicle.Wheels.Length];
            vDto.Doors = new DoorDto[vehicle.Doors.Length];

            int i = 0;
            foreach (IWheel w in vehicle.Wheels)
            {
                vDto.Wheels[i] = convert(w);
                i++;
            }

            int j = 0;
            foreach (IDoor d in vehicle.Doors)
            {
                vDto.Doors[j] = convert(d);
                j++;
            }

            return vDto;
        }

        public IDoor convert(DoorDto doorDto)
        {
            IDoor d = new Door();

            if (doorDto.IsOpen)
                d.open();
            else
                d.close();

            return d;
        }

        public DoorDto convert(IDoor door)
        {
            DoorDto dDto = new DoorDto();
            dDto.IsOpen = door.IsOpen;

            return dDto;
        }

        public IWheel convert(WheelDto wheelDto)
        {
            IWheel w = new Wheel();
            w.Pressure = wheelDto.Pressure;

            return w;
        }

        public WheelDto convert(IWheel wheel)
        {
            WheelDto wDto = new WheelDto();
            wDto.Pressure = wheel.Pressure;
            return wDto;
        }

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        public EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto eDto = new EnrollmentDto();
            eDto.Serial = enrollment.Serial;
            eDto.Number = enrollment.Number;

            return eDto;
        }

        private class Wheel : IWheel
        {
            private double pressure = 1.0;

            public double Pressure
            {
                get
                {
                    return this.pressure;
                }
                set
                {
                    Asserts.isTrue(value >= 1);
                    Asserts.isTrue(value <= 5);
                    this.pressure = value;
                }
            }
        }

        private class Door : IDoor
        {
            private bool isOpen = false;

            public void open()
            {
                Asserts.isFalse(this.isOpen);
                this.isOpen = true;
            }

            public void close()
            {
                Asserts.isTrue(this.isOpen);
                this.isOpen = false;
            }

            public bool IsOpen
            {
                get
                {
                    return this.isOpen;
                }
            }
        }

        private class Engine : IEngine
        {

            private const int MAXPOWER = 4000;
            private const int MINPOWER = 1;

            private int horsepower;
            private bool isStarted;

            public Engine(int h)
            {
                Asserts.isTrue(h >= MINPOWER);
                Asserts.isTrue(h <= MAXPOWER);
                this.horsepower = h;
            }

            public void start()
            {
                Asserts.isFalse(this.isStarted);
                this.isStarted = true;
            }

            public bool IsStarted
            {
                get
                {
                    return this.isStarted;
                }
            }

            public int HorsePower
            {
                get
                {
                    return this.horsepower;
                }
            }

            public void stop()
            {
                Asserts.isTrue(this.isStarted);
                this.isStarted = false;
            }
        }

        private class Vehicle : IVehicle
        {
            private List<IDoor> doors;
            private List<IWheel> wheels;
            private IEngine engine;
            private CarColor color;
            private IEnrollment enrollment;
            private CarColor colorCode;

            public Vehicle(List<IWheel> wheels, List<IDoor> doors, IEngine engine, CarColor colorCode, IEnrollment enrollment)
            {
                this.wheels = wheels;
                this.doors = doors;
                this.engine = engine;
                this.colorCode = colorCode;
                this.enrollment = enrollment;
            }

            public IDoor[] Doors
            {
                get
                {
                    return this.doors.ToArray();
                }

            }

            public IWheel[] Wheels
            {
                get
                {
                    return this.wheels.ToArray();
                }

            }

            public IEngine Engine
            {
                get
                {
                    return this.engine;
                }
            }

            public IEnrollment Enrollment
            {
                get
                {
                    return this.enrollment;
                }
            }

            public List<IWheel> SetWheels
            {
                set
                {
                    this.wheels = value;
                }
            }

            public List<IDoor> SetDoors
            {
                set
                {
                    this.doors = value;
                }
            }

            public Engine SetEngine
            {
                set
                {
                    this.engine = value;
                }
            }

            public CarColor SetCarColor
            {
                set
                {
                    this.color = value;
                }
            }

            public CarColor Color { get; }

            public void setWheelsPressure(double pression)
            {
                foreach (Wheel w in this.wheels)
                {
                    w.Pressure = pression;
                }
            }

            public IEnrollment SetEnrollment
            {
                set
                {
                    this.enrollment = value;
                }
            }
        }


    }
}