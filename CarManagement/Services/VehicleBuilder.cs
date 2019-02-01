using System;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using System.Collections.Generic;
using CarManagement.Core;

namespace CarManagement.Services
{
    public class VehicleBuilder : IVehicleBuilder
    {
        private CarColor color;
        private int numberWheels;
        private int numberDoors;
        private int horsePorwer;
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            this.numberDoors = 0;
            this.numberWheels = 0;
        }

        public void addWheel()
        {
            Asserts.isTrue(this.numberWheels < 4);
            this.numberWheels++;
        }

        public void removeWheel()
        {
            Asserts.isTrue(this.numberWheels > 0);
            this.numberWheels--;
        }

        public void setDoors(int doorsCount)
        {
            Asserts.isTrue(0 <= doorsCount && doorsCount < 7);
            this.numberDoors = doorsCount;
        }

        public void setEngine(int horsePorwer)
        {
            Asserts.isTrue(horsePorwer > 0);
            this.horsePorwer = horsePorwer;
        }

        public void setColor(CarColor color)
        {
            Asserts.isEnumDefined(color);
            this.color = color;
        }

        /*private List<T> create<T>(int nItems) where T : class, new()
        {
            List<T> items = new List<T>();
            for (int i = 0; i < nItems; i++)
            {
                T aux = new T();
                items.Add(aux);
            }

            return items;
        }*/

        private List<TInterface> create<TInterface, TInstance>(int nItems) 
            where TInstance : class, TInterface, new()
            where TInterface : class
        {
            List<TInterface> items = new List<TInterface>();
            for (int i = 0; i < nItems; i++)
            {
                TInterface aux = new TInstance();
                items.Add(aux);
            }

            return items;
        }


        public IVehicle build()
        {
            Asserts.isTrue(0 < this.numberWheels && this.numberWheels <= 4);

            Engine engine = new Engine(this.horsePorwer);
            List<IDoor> doors = create<IDoor, Door>(this.numberDoors);
            List<IWheel> wheels = create<IWheel, Wheel>(this.numberWheels);

            Vehicle vehicle = new Vehicle(wheels, doors, engine, this.color, this.enrollmentProvider.getNew());
            return vehicle;
        }

        private class Door : IDoor
        {
            private bool openDoor;

            public Door()
            {
                this.openDoor = false;
            }

            public Door(bool openDoor)
            {
                this.openDoor = openDoor;
            }

            public void open()
            {
                Asserts.isFalse(this.openDoor);
                this.openDoor = true;
            }

            public void close()
            {
                Asserts.isTrue(this.openDoor);
                this.openDoor = false;
            }

            public bool IsOpen
            {
                get
                {
                    return this.openDoor;
                }
            }
        }

        private class Engine : IEngine
        {
            private bool startEngine;
            private int horsePorwer;

            public Engine(int horsePorwer)
            {
                this.startEngine = false;
                this.horsePorwer = horsePorwer;
            }

            public Engine(int horsePorwer, bool startEngine)
            {
                this.startEngine = startEngine;
                this.horsePorwer = horsePorwer;
            }

            public bool IsStarted
            {
                get
                {
                    return this.startEngine;
                }
            }

            public void start()
            {
                Asserts.isFalse(this.startEngine);
                this.startEngine = true;
            }

            public int HorsePower
            {
                get
                {
                    return this.horsePorwer;
                }
            }


            /*public int HorsePorwer
            {
                get
                {
                    return this.horsePorwer;
                }
                set
                {
                    this.horsePorwer = value;
                }
            }*/

            public void stop()
            {
                Asserts.isTrue(this.startEngine);
                this.startEngine = false;
            }
        }

        private class Vehicle : IVehicle
        {
            private IEnrollment enrollment;
            private List<IWheel> wheels;
            private List<IDoor> doors;
            private Engine engine;
            private CarColor color;

            public Vehicle(List<IWheel> wheels, List<IDoor> doors, Engine engine, CarColor color, IEnrollment enrollment)
            {
                this.wheels = wheels;
                this.doors = doors;
                this.engine = engine;
                this.color = color;
                this.enrollment = enrollment;
            }

            public int DoorsCount
            {
                get
                {
                    return this.doors.Count;
                }
            }

            public int WheelCount
            {
                get
                {
                    return this.wheels.Count;
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

            public IWheel[] Wheels
            {
                get
                {
                    return this.wheels.ToArray();
                }
            }
            public IDoor[] Doors
            {
                get
                {
                    return this.doors.ToArray();
                }
            }

            public CarColor Color
            {
                get
                {
                    return this.color;
                }
            }

            public void setWheelsPressure(double pression)
            {
                Asserts.isTrue(pression >= 0);
                for (int i = 0; i < this.WheelCount; i++)
                {
                    this.Wheels[i].Pressure = pression;
                }
            }
        }

        private class Wheel : IWheel
        {
            private double pression;

            public double Pressure
            {
                get
                {
                    return this.pression;
                }
                set
                {
                    this.pression = value;
                }
            }
        }


    }
}