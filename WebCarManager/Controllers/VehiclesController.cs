using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using CarManagement.Services;

namespace WebCarManager.Controllers
{
    public class VehiclesController : AbstractController
    {
        private readonly IVehicleStorage vehicleStorage;
        private IEnrollmentProvider enrollmentProvider;
        private IVehicleBuilder vehiclebuilder;
        private IEnumerable<IEnrollment> enrollmentEnum;
        private EnrollmentDto enrollmentDto;
        private List<EnrollmentDto> enrollmentDtoList;
        private VehicleBuilder vehicleBuilder;
        private VehicleDto vehicleDto;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.enrollmentProvider = new DefaultEnrollmentProvider();
            this.vehiclebuilder = new VehicleBuilder(this.enrollmentProvider);
            this.enrollmentEnum = this.vehicleStorage.get().Keys;
            this.enrollmentDto = new EnrollmentDto();
            this.enrollmentDtoList = new List<EnrollmentDto>();
            this.enrollmentDto = new EnrollmentDto();
            this.enrollmentProvider = new DefaultEnrollmentProvider();
            this.vehicleBuilder = new VehicleBuilder(this.enrollmentProvider);
            this.vehicleDto = new VehicleDto();
        }

        public ActionResult Index()
        {
            foreach (IEnrollment enrollment in this.enrollmentEnum)
            {
                this.enrollmentDto.Number = enrollment.Number;
                this.enrollmentDto.Serial = enrollment.Serial;
                this.enrollmentDtoList.Add(this.enrollmentDto);
            }
            this.ViewBag.Message = "Enrollment list";
            return View(this.enrollmentDtoList);
        }
        // GET: Vehicles
        [HttpGet]
        public ActionResult Edit(string serial, int number)
        {
            this.vehicleDto = new VehicleDto();
            this.vehicleDto = getVehicleData(serial, number);
            return View(this.vehicleDto);
        }
        // SET: Vehicles
        [HttpGet]
        public ActionResult Save(VehicleDto vehicleDto)
        {
            this.vehicleStorage.set(convert(vehicleDto));
            return View(getVehicleData(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number));
        }

        private VehicleDto getVehicleData(string serial, int number)
        {
            return convert(selectIvehicle(this.vehicleStorage, convert(serial, number)));
        }
        private static IVehicle selectIvehicle(IVehicleStorage vehicleStorage, IEnrollment enrollment)
        {
            return vehicleStorage.get()
            .whereEnrollmentIs(enrollment)
            .Select(vehicle => vehicle)
            .Single();
        }
        private IEnrollment convert(string serial, int number)
        {
            return this.enrollmentProvider.import(serial, number);
        }
        private VehicleDto convert(IVehicle vehicle)
        {
            this.vehicleDto = this.vehicleBuilder.export(vehicle);
            return this.vehicleDto;
        }
        private IVehicle convert(VehicleDto vehicleDto)
        {
            this.vehicleBuilder = new VehicleBuilder(this.enrollmentProvider);
            return this.vehicleBuilder.import(vehicleDto);
        }
    }
}