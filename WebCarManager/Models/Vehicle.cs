using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using System.Collections.Generic;
using System.Linq;
using WebCarManager.Controllers;

namespace WebCarManager.Models
{
    public class Vehicle : AbstractController
    {
        private readonly IVehicleStorage vehicleStorage;
        private readonly IVehicleBuilder vehicleBuilder;
        private IEnumerable<IEnrollment> enrollmentEnum;
        private IList<EnrollmentDto> enrollmentDtoList;
        private VehicleDto vehicleDto;

        public Vehicle()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.vehicleBuilder = getService<IVehicleBuilder>();
        }
        public IList<EnrollmentDto> enrollmentDTOList()
        {
            this.enrollmentEnum = this.vehicleStorage
                .get()
                .Select(vehicle => vehicle.Enrollment);
            this.enrollmentDtoList = new List<EnrollmentDto>();
            foreach (IEnrollment enrollment in this.enrollmentEnum)
            {
                this.enrollmentDtoList.Add( new EnrollmentDto() { Serial = enrollment.Serial, Number = enrollment.Number });
            }
            return this.enrollmentDtoList;
}
        public VehicleDto getVehicle(string serial, int number)
        {
            return this.vehicleBuilder.export(
                this.vehicleStorage
                    .get()
                    .whereEnrollmentIs(this.vehicleBuilder.import(serial, number))
                    .Single());
        }
        public void set(VehicleDto vehicleDto)
        {
            this.vehicleStorage.set(this.vehicleBuilder.import(vehicleDto));
        }
        private int[] fixPressure(int wheels, int[] pressure)
        {
            List<int> lista = pressure.ToList();
            if (lista.Count < wheels)
            {
                for (int i = pressure.Length; i < wheels; i++)
                {
                    lista.Add(1);
                }
            }
            return lista.ToArray();
        }
        public VehicleDto Edit(CarColor color, EngineDto engineDto, int Doors,  int wheels, int[] pressure, EnrollmentDto enrollmentDto)
        {
            this.vehicleDto = new VehicleDto() 
            { 
                Enrollment = enrollmentDto, 
                Color = color, 
                Engine = engineDto, 
                Doors = new DoorDto[Doors] 
            };

            for (int i = 0; i < Doors; i++)
            {
                this.vehicleDto.Doors[i] = new DoorDto();
            }

            int[] newPressure;
            if (pressure.Length < wheels)
            {
                newPressure = fixPressure(wheels, pressure);
            }
            else
            {
                newPressure = pressure;
            }

            this.vehicleDto.Wheels = new WheelDto[wheels];
            if (newPressure.Length > 0)
            {
                for (int i = 0; i < wheels; i++)
                {
                    this.vehicleDto.Wheels[i] = new WheelDto(newPressure[i]);
                }
            }
            else
            {
                for (int i = 0; i < wheels; i++)
                {
                    this.vehicleDto.Wheels[i] = new WheelDto();
                }
            }

            set(this.vehicleDto);
            return getVehicle(this.vehicleDto.Enrollment.Serial, this.vehicleDto.Enrollment.Number);
        }
    }
}