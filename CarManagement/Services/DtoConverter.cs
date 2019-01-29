using CarManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    static class DtoConverter
    {
        static public DoorDto Convert(Door d)
        {
            DoorDto dDto =new DoorDto();
            dDto.IsOpen = d.IsOpen;

            return dDto;
        }

        static public Door Convert(DoorDto dDto)
        {
            Door d = new Door();

            if (dDto.IsOpen)
                d.open();
            else
                d.close();

            return d;
        }

        static public WheelDto Convert(Wheel w)
        {
            WheelDto wDto = new WheelDto();
            wDto.Pressure = w.Pressure;
            return wDto;
        }

        static public Wheel Convert(WheelDto wDto)
        {
            Wheel w = new Wheel();
            w.FillWheel(wDto.Pressure);

            return w;
        }

        static public EngineDto Convert(Engine e)
        {
            EngineDto eDto = new EngineDto();
            eDto.IsStarted = e.IsStarted;
            eDto.HorsePower = e.HorsePower;

            return eDto;
        }

        static public Engine Convert(EngineDto eDto)
        {
            Engine e = new Engine(eDto.HorsePower);

            if (eDto.IsStarted)
                e.start();
            else
                e.stop();

            return e;
        }

        static public EnrollmentDto Convert(IEnrollment e)
        {
            EnrollmentDto eDto = new EnrollmentDto();
            eDto.Serial = e.Serial;
            eDto.Number = e.Number;

            return eDto;
        }

        static public IEnrollment Convert(EnrollmentDto eDto, IEnrollmentProvider enrollmentProvider)
        {
            return enrollmentProvider.import(eDto.Serial, eDto.Number);
        }

        static public VehicleDto Convert(Vehicle v)
        {
            VehicleDto vDto = new VehicleDto();
            vDto.Color = v.Color;
            vDto.Engine = Convert(v.Engine);
            vDto.Enrollment = Convert(v.Enrollment);
            vDto.Wheels = new WheelDto[v.Wheels.Length];
            vDto.Doors = new DoorDto[v.Doors.Length];

            int i = 0;
            foreach (Wheel w in v.Wheels)
            {
                vDto.Wheels[i] = Convert(w);
                i++;
            }

            int j = 0;
            foreach (Door d in v.Doors)
            {
                vDto.Doors[j] = Convert(d);
                j++;
            }

            return vDto;
        }

        static public Vehicle Convert(VehicleDto vDto)
        {
            Vehicle v;

            List<Wheel> wheels = new List<Wheel>();
            List<Door> doors = new List<Door>();
            Engine engine;
            IEnrollment enrollment;

            foreach (WheelDto w in vDto.Wheels)
            {
                wheels.Add(Convert(w));
            }

            foreach (DoorDto d in vDto.Doors)
            {
                doors.Add(Convert(d));
            }
            engine = Convert(vDto.Engine);

            enrollment = Convert(vDto.Enrollment,);

            v = new Vehicle(wheels, doors, engine, vDto.Color, enrollment);

            return v;
        }
    }
}
