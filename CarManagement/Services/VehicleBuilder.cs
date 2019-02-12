using System;
using System.Collections.Generic;
using System.Linq;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    //Comentario pre commit de la muerte


    namespace CarManagement.Builders
    {
        public class VehicleBuilder : IVehicleBuilder
        {
            private int numberWheel;
            private int numberDoor;
            private int engine;
            private CarColor color;
            private readonly IEnrollmentProvider enrollmentProvider;





            public VehicleBuilder(IEnrollmentProvider enrollmentProvider)
            {
                this.numberWheel = 0;
                this.numberDoor = 0;
                this.engine = 1;
                this.color = CarColor.Red;
                this.enrollmentProvider = enrollmentProvider;
               



            }

            public void addWheel()
            {

                this.numberWheel++;
                Asserts.isTrue(this.numberWheel <= 4);
            }

            public void removeWheel()
            {
                Asserts.isTrue(this.numberWheel > 0);
                this.numberWheel--;
            }

            public void setDoors(int doorsCount)
            {
               Asserts.isTrue(0 <= doorsCount && doorsCount <= 6);
                this.numberDoor = doorsCount;
            }

            public void setEngine(int horsePorwer)
            {

                Asserts.isTrue(horsePorwer > 0);
                this.engine = horsePorwer;
            }

            public void setColor(CarColor color)
            {
                this.color = color;
                Asserts.isTrue((CarColor)0 <= color && color < (CarColor)7);
            }

            private List<TInterface> generateList<TInterface, TInstance>(int count)
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

            public IVehicle build()
            {

     
                //Generamos puertas
                Asserts.isTrue(this.numberDoor >= 0);
                List<IDoor> doors = generateList<IDoor, Door>(this.numberDoor);

                //Generamos motor

                Engine engine = new Engine(this.engine);

                //Generamos ruedas

                List<IWheel> wheels = generateList<IWheel, Wheel>(this.numberWheel);
                Asserts.isTrue(wheels.Count > 0);
             
                //Generamos matricula

                IEnrollment enrollment = this.enrollmentProvider.getNew();

                //Generamos coche

                return new Vehicle(this.color, wheels, enrollment, doors, engine);

            }
            private class Wheel : IWheel
            {
                private double pressure;

                public Wheel()
                {
                   
                    this.pressure = 1;

                }

                public Wheel(double pressure)
                {
                    Asserts.isTrue(1 <= pressure && pressure <= 5);
                    this.Pressure = pressure;
                }
                public double Pressure
                {
                    set
                    {
                        Asserts.isTrue(1 <= value && value <= 5);
                        this.pressure = value;
                    }
                    get
                    {
                        return this.pressure;
                    }
                }

          
            }

            private class Vehicle : IVehicle
            {
                private CarColor color;
                private IReadOnlyList<IWheel> wheels;
                private IEnrollment enrollment;
                private IReadOnlyList<IDoor> doors;
                private IEngine engine;


                public Vehicle(CarColor color, List<IWheel> wheels, IEnrollment enrollment, List<IDoor> doors, IEngine engine)
                {
                    this.color = color;
                    this.wheels = wheels;
                    this.enrollment = enrollment;
                    this.doors = doors;
                    this.engine = engine;
                    
                }

                public IEngine Engine
                {
                    get
                    {
                        return this.engine;
                    }
                }

                public IEnrollment Enrollment { get { return this.enrollment; } }

                public IWheel[] Wheels
                {
                    get
                    {
                        IWheel[] wheelsArray = new IWheel[this.wheels.Count];

                        for (int i = 0; i < this.wheels.Count; i++)
                        {
                            wheelsArray[i] = this.wheels[i];
                        }

                        return wheelsArray;
                    }
                }

                public IDoor[] Doors
                {
                    get
                    {
                        return this.doors
                            .Cast<IDoor>()
                            .ToArray();
                    }
                }

                public CarColor Color { get; }

                public void setWheelsPressure(double pression)
                {

                    foreach (Wheel wheel in this.wheels)
                    {
                        
                        wheel.Pressure = pression;
                        
                    }
                }
            }

            private class Engine : IEngine
            {
                private int horsePower;
                private bool mode = false;
                public Engine()
                {
                    this.mode = false;
                }

                public Engine(int horsePower)
                {
                    Asserts.isTrue(horsePower > 0);
                    this.horsePower = horsePower;
                }

                public Engine(int horsePower, bool mode)
                {
                    Asserts.isTrue(horsePower > 0);
                    this.horsePower = horsePower;
                    this.mode = mode;
                }

                public int Model
                {
                    get
                    {
                        return this.horsePower;
                    }
                    set
                    {
                        setHorsePower(value);
                    }
                }
                public bool IsStarted
                {
                    get
                    {
                        return this.mode;
                    }
                }

                public int HorsePower
                {
                    get
                    {
                        return this.horsePower;
                    }
                    set
                    {
                        setHorsePower(value);
                    }
                }

                public void start()
                {
                    Asserts.isFalse(this.IsStarted);
                    this.mode = true;
                    
                }

                public void stop()
                {
                    Asserts.isTrue(this.IsStarted);
                    this.mode = false;
                    
                }

                private void setHorsePower(int horsePower)
                {
                   
                    this.horsePower = horsePower;
                }
            }

            private class Door : IDoor
            {

                private bool openDoor;

                public Door()
                {
                    this.openDoor = false;
                }

                public Door(bool openClose)
                {
                    Asserts.isFalse(this.openDoor);
                    this.openDoor = openClose;
                }

                public bool IsOpen
                {
                    get
                    {
                        return this.openDoor;
                    }
                }

                public void open()
                {
                    Asserts.isFalse(this.IsOpen);
                    this.openDoor = true;
                }

                public void close()
                {
                    Asserts.isTrue(this.openDoor);
                    this.openDoor = false;
                }
            }

            public IVehicle import(VehicleDto vehicleDto)
            {
                IEngine toEngine = convert(vehicleDto.Engine);
                IEnrollment toEnrollment = convert(vehicleDto.Enrollment);
                List<IWheel> listWheels = new List<IWheel>();
                List<IDoor> listDoor = new List<IDoor>();
                
             
                foreach (WheelDto wheelsDto in vehicleDto.Wheels)
                {
                    IWheel setwheel = convert(wheelsDto);
                    listWheels.Add(setwheel);

                }
                foreach (DoorDto doorDto in vehicleDto.Doors)
                {
                    IDoor setdoor = convert(doorDto);
                    listDoor.Add(setdoor);

                }
                return new Vehicle(vehicleDto.Color, listWheels, toEnrollment, listDoor, toEngine);

            }
            public VehicleDto export(IVehicle vehicle)
            {
                DefaultDtoConverter converter = new DefaultDtoConverter (this.enrollmentProvider);
                return converter.convert(vehicle);
            }
            private  IDoor convert(DoorDto doorDto)
            {
                IDoor toMemory = new Door();
                if (doorDto.IsOpen == true)
                {
                    toMemory.open();
                }
                //toMemory.close();
                return toMemory;
            }
            private  IWheel convert(WheelDto wheelDto)
            {
                return new Wheel(wheelDto.Pressure);

            }
            private IEnrollment convert(EnrollmentDto enrollmentDto)
            {
                return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
            }
            private  IEngine convert(EngineDto engineDto)
            {
                IEngine toMemory = new Engine(engineDto.HorsePower);
                if (engineDto.IsStarted == true)
                {
                    toMemory.start();
                }
                else
                {
                    
                }

                return toMemory;
            }
        }
    }
}