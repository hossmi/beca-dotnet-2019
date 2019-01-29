using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.Models.DTOs
{
    static class DtoMapper
    {
        public static EngineDto convert(Engine engine)
        {
            EngineDto engineDto = new EngineDto();
            engineDto.HorsePower = engine.HorsePower;
            engineDto.Model = engine.Model;
            engineDto.IsStarted = engine.IsStarted;

            return engineDto;
        }
        public static Engine convert(EngineDto engine)
        {
            return new Engine(engine.HorsePower, engine.Model, engine.IsStarted);
        }
        public static WheelDto convert(Wheel wheel)
        {
            WheelDto wheelDto = new WheelDto();
            wheelDto.Model = wheel.Model;
            wheelDto.Pressure = wheel.Pressure;

            return wheelDto;
        }
        public static Wheel convert(WheelDto wheel)
        {
            return new Wheel(wheel.Model,wheel.Pressure);
        }
        public static DoorDto convert(Door door)
        {
            DoorDto doorDto = new DoorDto();
            doorDto.IsOpen = door.IsOpen;
            doorDto.Model = door.Model;

            return doorDto;
        }
        public static Door convert(DoorDto door)
        {
            return new Door(door.Model,door.IsOpen);
        }
        public static EnrollmentDto convert(IEnrollment enrollment)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            enrollmentDto.Serial = enrollment.Serial;
            enrollmentDto.Number = enrollment.Number;

            return enrollmentDto;
        }
        public static IEnrollment convert(EnrollmentDto enrollment)
        {
            return new Enrollment(enrollment.Number,enrollment.Serial);
        }
        public static VehicleDto convert(Vehicle vehicle)
        {
            VehicleDto vehicleDto = new VehicleDto();
            vehicleDto.Color = vehicle.Color;
            vehicleDto.Engine = convert(vehicle.Engine);
            vehicleDto.Enrollment = convert(vehicle.Enrollment);
            List<DoorDto> dtoDoorsList = new List<DoorDto>();
            foreach (Door door in vehicle.Doors)
            {
                dtoDoorsList.Add(convert(door));
            }
            vehicleDto.Doors = dtoDoorsList.ToArray();

            List<WheelDto> dtoWheelsList = new List<WheelDto>();
            foreach (Wheel wheel in vehicle.Wheels)
            {
                dtoWheelsList.Add(convert(wheel));
            }
            vehicleDto.Wheels = dtoWheelsList.ToArray();

            return vehicleDto;

        }
        public static Vehicle convert(VehicleDto vehicle)
        {
            List<Door> doors = new List<Door>();
            foreach (DoorDto door in vehicle.Doors)
            {
                doors.Add(convert(door));
            }

            List<Wheel> wheels = new List<Wheel>();
            foreach (WheelDto wheel in vehicle.Wheels)
            {
                wheels.Add(convert(wheel));
            }

            return new Vehicle(convert(vehicle.Engine), doors, wheels, vehicle.Color, convert(vehicle.Enrollment));
        }
    }
}
