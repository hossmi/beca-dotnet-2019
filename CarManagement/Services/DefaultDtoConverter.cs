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

        public Engine convert(EngineDto engineDto)
        {
            Engine e = new Engine(engineDto.HorsePower);

            if (engineDto.IsStarted)
                e.start();
            //else
                //e.stop();

            return e;
        }

        public EngineDto convert(Engine engine)
        {
            EngineDto eDto = new EngineDto();
            eDto.IsStarted = engine.IsStarted;
            eDto.HorsePower = engine.HorsePower;

            return eDto;
        }

        public Vehicle convert(VehicleDto vehicleDto)
        {
            Vehicle v;

            List<Wheel> wheels = new List<Wheel>();
            List<Door> doors = new List<Door>();
            Engine engine;
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

        public VehicleDto convert(Vehicle vehicle)
        {
            VehicleDto vDto = new VehicleDto();
            vDto.Color = vehicle.Color;
            vDto.Engine = convert(vehicle.Engine);
            vDto.Enrollment = convert(vehicle.Enrollment);
            vDto.Wheels = new WheelDto[vehicle.Wheels.Length];
            vDto.Doors = new DoorDto[vehicle.Doors.Length];

            int i = 0;
            foreach (Wheel w in vehicle.Wheels)
            {
                vDto.Wheels[i] = convert(w);
                i++;
            }

            int j = 0;
            foreach (Door d in vehicle.Doors)
            {
                vDto.Doors[j] = convert(d);
                j++;
            }

            return vDto;
        }

        public Door convert(DoorDto doorDto)
        {
            Door d = new Door();

            if (doorDto.IsOpen)
                d.open();
            else
                d.close();

            return d;
        }

        public DoorDto convert(Door door)
        {
            DoorDto dDto = new DoorDto();
            dDto.IsOpen = door.IsOpen;

            return dDto;
        }

        public Wheel convert(WheelDto wheelDto)
        {
            Wheel w = new Wheel();
            w.FillWheel(wheelDto.Pressure);

            return w;
        }

        public WheelDto convert(Wheel wheel)
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