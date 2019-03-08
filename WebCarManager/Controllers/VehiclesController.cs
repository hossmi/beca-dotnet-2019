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
        private readonly IEnrollmentProvider enrollmentProvider;
        private readonly VehicleBuilder vehicleBuilder;
        private readonly IEnumerable<IEnrollment> enrollmentEnum;
        private readonly IEnumerable<IVehicle> vehicleList;
        private EnrollmentDto enrollmentDto;
        private VehicleDto vehicleDto;
        private List<EnrollmentDto> enrollmentDtoList;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.enrollmentProvider = new DefaultEnrollmentProvider();
            this.vehicleBuilder = new VehicleBuilder(this.enrollmentProvider);
            this.vehicleList = this.vehicleStorage
                .get();
            this.enrollmentEnum = this.vehicleList
                .Select(vehicle => vehicle.Enrollment);
        }

        public ActionResult Index()
        {
            this.enrollmentDtoList = new List<EnrollmentDto>();
            foreach (IEnrollment enrollment in this.enrollmentEnum)
            {
                this.enrollmentDto = new EnrollmentDto(enrollment.Serial, enrollment.Number);
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
            this.vehicleDto = getVehicle(serial, number);
            return View(this.vehicleDto);
        }

        // SET: Vehicles
        [HttpGet]
        public ActionResult Save(VehicleDto vehicleDto)
        {
            this.vehicleStorage.set(this.vehicleBuilder.import(vehicleDto));
            this.vehicleDto = new VehicleDto();
            this.vehicleDto = getVehicle(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number);
            return View(this.vehicleDto);
        }

        private VehicleDto getVehicle(string serial, int number)
        {
                return this.vehicleBuilder.export(this.vehicleStorage
                .get()
                .whereEnrollmentIs(this.enrollmentProvider.import(serial, number))
                .Single());
        }
    }
}