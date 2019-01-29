using CarManagement.Models;
using CarManagement.Models.DTOs;
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

        Engine IDtoConverter.convert(EngineDto engineDto)
        {
            Engine e = new Engine(engineDto.HorsePower);

            if (engineDto.IsStarted)
                e.start();
            else
                e.stop();

            return e;
        }

        EngineDto IDtoConverter.convert(Engine engine)
        {
            EngineDto eDto = new EngineDto();
            eDto.IsStarted = engine.IsStarted;
            eDto.HorsePower = engine.HorsePower;

            return eDto;
        }

        Vehicle IDtoConverter.convert(VehicleDto vehicleDto)
        {
            Vehicle v;

            List<Wheel> wheels = new List<Wheel>();
            List<Door> doors = new List<Door>();
            Engine engine;
            IEnrollment enrollment;

            foreach (WheelDto w in vehicleDto.Wheels)
            {
                wheels.Add(IDtoConverter.convert(w));
   
            }

            foreach (DoorDto d in vehicleDto.Doors)
            {
                doors.Add(IDtoConverter.convert(d));
            }
            engine = IDtoConverter.convert(vehicleDto.Engine);

            enrollment = IDtoConverter.convert(vehicleDto.Enrollment);

            v = new Vehicle(wheels, doors, engine, vehicleDto.Color, enrollment);

            return v;
        }

        VehicleDto IDtoConverter.convert(Vehicle vehicle)
        {
            VehicleDto vDto = new VehicleDto();
            vDto.Color = vehicle.Color;
            vDto.Engine = IDtoConverter.convert(vehicle.Engine);
            vDto.Enrollment = IDtoConverter.convert(vehicle.Enrollment);
            vDto.Wheels = new WheelDto[vehicle.Wheels.Length];
            vDto.Doors = new DoorDto[vehicle.Doors.Length];

            int i = 0;
            foreach (Wheel w in vehicle.Wheels)
            {
                vDto.Wheels[i] = IDtoConverter.convert(w);
                i++;
            }

            int j = 0;
            foreach (Door d in vehicle.Doors)
            {
                vDto.Doors[j] = IDtoConverter.convert(d);
                j++;
            }

            return vDto;
        }

        Door IDtoConverter.convert(DoorDto doorDto)
        {
            Door d = new Door();

            if (doorDto.IsOpen)
                d.open();
            else
                d.close();

            return d;
        }

        DoorDto IDtoConverter.convert(Door door)
        {
            DoorDto dDto = new DoorDto();
            dDto.IsOpen = door.IsOpen;

            return dDto;
        }

        Wheel IDtoConverter.convert(WheelDto wheelDto)
        {
            Wheel w = new Wheel();
            w.FillWheel(wheelDto.Pressure);

            return w;
        }

        WheelDto IDtoConverter.convert(Wheel wheel)
        {
            WheelDto wDto = new WheelDto();
            wDto.Pressure = wheel.Pressure;
            return wDto;
        }

        IEnrollment IDtoConverter.convert(EnrollmentDto enrollmentDto)
        {
            return enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
        }

        EnrollmentDto IDtoConverter.convert(IEnrollment enrollment)
        {
            EnrollmentDto eDto = new EnrollmentDto();
            eDto.Serial = enrollment.Serial;
            eDto.Number = enrollment.Number;

            return eDto;
        }
    }
}