using System;
using System.Collections.Generic;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        const int MAX_WHEELS = 4;
        const int MAX_DOORS = 6;
        const int MAXPOWER = 4000;
        const int MINPOWER = 1;
        private int wheelsCount;
        private int doorsCount;
        private int enginePower;
        //private bool engineStarted;
        private CarColor colorCode;
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.wheelsCount = 0;
            this.doorsCount = 0;
            this.enginePower = 0;
            this.colorCode = 0;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.wheelsCount < MAX_WHEELS, "Maximum number of wheels reached. Cannot add more wheels.");
            this.wheelsCount++;
        }

        public void setDoors(int doorsCount)
        {
            Asserts.isTrue(doorsCount >= 0,"Cannot create a vehicle with negative doors");
            Asserts.isTrue(doorsCount <= MAX_DOORS, $"Cannot create a vehicle with more than {MAX_DOORS} doors");
            this.doorsCount = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            Asserts.isTrue(horsePorwer >= MINPOWER, $"Cannot create an engine with less than {MINPOWER} Horse Power.");
            Asserts.isTrue(horsePorwer <= MAXPOWER, $"Cannot create an engine above {MAXPOWER} Horse Power.");
            this.enginePower = horsePorwer;
        }
        
        public void setColor(CarColor color)
        {
            //if (Enum.IsDefined(typeof(CarColor), color) == false)
            //    throw new ArgumentException($"Parameter {nameof(color)} has not a valid value.");
            Asserts.isEnumDefined<CarColor>(color,"The selected color does not match.");
            this.colorCode = color;
        }

        public IVehicle build()
        {
            Asserts.isTrue(this.wheelsCount > 0);

            List<IWheel> wheels = CreateObject<IWheel, Wheel>(this.wheelsCount);
            List<IDoor> doors = CreateObject<IDoor, Door>(this.doorsCount);
            Engine engine = CreateEngine(this.enginePower);
            IEnrollment enrollment = this.enrollmentProvider.getNew();

            Vehicle vehicle = new Vehicle(wheels, doors, engine, this.colorCode, enrollment);

            return vehicle;
        }

        private List<TInterface> CreateObject<TInterface, TInstance>(int count) 
            where TInstance : class, TInterface, new()
            where TInterface : class
        {
            List<TInterface> list = new List<TInterface>();
            for (int i = 0; i < count; i++)
            {
                TInterface obj = new TInstance();
                list.Add(obj);
            }
            return list;
        }

        private Engine CreateEngine(int power)
        {
            Engine engine = new Engine(power);
            return engine;
        }

        public void removeWheel()
        {
            Asserts.isTrue(this.wheelsCount > 0, "The vehicle does not have any more wheels to remove.");
            this.wheelsCount--;
        }

        private class Wheel : IWheel
        {
            private double pressure;

            public Wheel()
            {
                this.pressure = 1.0;
            }

            public double Pressure
            {
                get
                {
                    return this.pressure;
                }
                set
                {
                    Asserts.isTrue(value >= 1.0, "Cannot set pressure lower than 1.0");
                    Asserts.isTrue(value <= 5.0, "Cannot set pressure higher than 5.0");
                    this.pressure = value;
                }
            }
        }

        private class Door: IDoor 
        {
            private bool isOpen;

            public Door()
            {
                this.isOpen = false;
            }

            public void open()
            {
                Asserts.isFalse(this.isOpen, "Door is already open.");
                this.isOpen = true;
            }

            public void close()
            {
                Asserts.isTrue(this.isOpen, "Door is already close.");
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
                Asserts.isTrue(h >= MINPOWER, $"Cannot create an engine with less than {MINPOWER} Horse Power.");
                Asserts.isTrue(h <= MAXPOWER, $"Cannot create an engine above {MAXPOWER} Horse Power.");
                this.horsepower = h;
                //this.isStarted = false;
            }

            public void start()
            {
                Asserts.isFalse(this.isStarted, "Engine is already started.");
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
                Asserts.isTrue(this.isStarted, "Engine is already stopped.");
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

        public IVehicle import(VehicleDto vehicleDto)
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

        public VehicleDto export(IVehicle vehicleDto)
        {
            VehicleDto vDto = new VehicleDto();
            vDto.Color = vehicleDto.Color;
            vDto.Engine = convert(vehicleDto.Engine);
            vDto.Enrollment = convert(vehicleDto.Enrollment);
            vDto.Wheels = new WheelDto[vehicleDto.Wheels.Length];
            vDto.Doors = new DoorDto[vehicleDto.Doors.Length];

            int i = 0;
            foreach (IWheel w in vehicleDto.Wheels)
            {
                vDto.Wheels[i] = convert(w);
                i++;
            }

            int j = 0;
            foreach (IDoor d in vehicleDto.Doors)
            {
                vDto.Doors[j] = convert(d);
                j++;
            }

            return vDto;
        }

        public IEngine convert(EngineDto engineDto)
        {
            IEngine e = new Engine(engineDto.HorsePower);

            if (engineDto.IsStarted)
                e.start();
            //else
            //e.stop();

            return e;
        }

        public EngineDto convert(IEngine engine)
        {
            EngineDto eDto = new EngineDto();
            eDto.IsStarted = engine.IsStarted;
            eDto.HorsePower = engine.HorsePower;

            return eDto;
        }


        public IDoor convert(DoorDto doorDto)
        {
            IDoor d = new Door();

            if (doorDto.IsOpen)
                d.open();
            //else
                //d.close();

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

    }
}

