using System.Collections.Generic;
using CarManagement.Core;
using CarManagement.Core.Models;
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
        private bool engineStarted;
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
            private double pressure = 1.0;

            public double Pressure
            {
                get
                {
                    return this.pressure;
                }
                set
                {
                    Asserts.isTrue(value >= 0, "Cannot set pressure lower than 0");
                    this.pressure = value;
                }
            }
        }

        private class Door: IDoor 
        {
            private bool isOpen = false;

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
            private Engine engine;
            private CarColor color;
            private IEnrollment enrollment;
            private CarColor colorCode;

            public Vehicle(List<IWheel> wheels, List<IDoor> doors, Engine engine, CarColor colorCode, IEnrollment enrollment)
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