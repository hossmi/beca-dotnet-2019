using CarManagement.Models;
using CarManagement.Models.DTOs;
using System;
using System.Collections.Generic;

namespace CarManagement.Services
{
    public class DefaultDtoConverter : IDtoConverter
    {
        private IEnrollmentProvider enrollmentProvider;

        public DefaultDtoConverter(IEnrollmentProvider enrollmentProvider)
        {
            this.enrollmentProvider = enrollmentProvider;
            
        }

        public Engine convert(EngineDto engineDto)
        {
            Engine toMemory = new Engine(engineDto .HorsePower);
            if (engineDto .IsStarted == true)
            {
                toMemory.start(); 
            }
            toMemory.stop();
            return toMemory;
        }

        public EngineDto convert(Engine engine)
        {
            EngineDto toDto = new EngineDto();
            toDto.HorsePower = engine.HorsePower;
            toDto.IsStarted  = engine.IsStarted;
            return toDto;

        }
        public Door convert(DoorDto doorDto)
        {
            Door toMemory = new Door();
            if (doorDto.IsOpen == true)
            {
                toMemory.open();
            }
            toMemory.close();
            return toMemory;
        }

        public DoorDto convert(Door door)
        {
            DoorDto toDto = new DoorDto ();
            toDto.IsOpen = door.IsOpen;
            return toDto;
       
        }

        public Wheel convert(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Pressure);

        }
       
        public WheelDto convert(Wheel wheel)
        {
            WheelDto toDto = new WheelDto();
            toDto.Pressure = wheel.Pressure;
            return toDto;
        }

        public IEnrollment convert(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        public EnrollmentDto  convert(IEnrollment enrollment)
        {
            EnrollmentDto toDto = new EnrollmentDto();
            toDto.Number = enrollment.Number;
            toDto.Serial = enrollment.Serial;
            return toDto;           
        }

        public Vehicle convert(VehicleDto vehicleDto)
        {
            List<Wheel> listWheels = new List<Wheel>();
            List<Door> listDoor = new List<Door>();
            IEnrollment enrollment = convertTo(vehicleDto.Enrollment);
            Engine engine = convertTo(vehicleDto.Engine);

            foreach (WheelDto wheelsDto in vehicleDto.Wheels)
            {
                Wheel setwheel = convertTo(wheelsDto);
                listWheels.Add(setwheel);

            }
            foreach (DoorDto doorDto in vehicleDto.Doors)
            {
                Door setdoor = convertTo(doorDto);
                listDoor.Add(setdoor);

            }

            return new Vehicle(vehicleDto.Color, listWheels, enrollment, listDoor, engine);
        }

        public VehicleDto convert(Vehicle vehicle)
        {

            VehicleDto  vehicleDto = convert(vehicle);
            return vehicleDto;

        }



        //Metodos privados
        private Wheel convertTo(WheelDto wheelDto)
        {
            return new Wheel(wheelDto.Pressure);

        }

        private Door convertTo(DoorDto doorDto)
        {
            Door toMemory = new Door();
            if (doorDto.IsOpen == true)
            {
                toMemory.open();
            }
            toMemory.close();
            return toMemory;
        }
        private IEnrollment convertTo(EnrollmentDto enrollmentDto)
        {
            return this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }
        private Engine convertTo(EngineDto engineDto)
        {
            Engine toMemory = new Engine(engineDto.HorsePower);
            if (engineDto.IsStarted == true)
            {
                toMemory.start();
            }
            toMemory.stop();
            return toMemory;
        }
    }
}