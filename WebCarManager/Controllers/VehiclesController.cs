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
        private VehicleBuilder vehicleBuilder;
        private IEnumerable<IEnrollment> enrollmentEnum;
        private EnrollmentDto enrollmentDto;
        private VehicleDto vehicleDto;
        private List<EnrollmentDto> enrollmentDtoList;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.enrollmentProvider = new DefaultEnrollmentProvider();
            this.vehicleBuilder = new VehicleBuilder(this.enrollmentProvider);
            this.vehicleDto = new VehicleDto();
        }

        public ActionResult Index()
        {
            this.enrollmentDtoList = new List<EnrollmentDto>();
            this.enrollmentEnum = this.vehicleStorage
                .get()
                .Keys;
            foreach (IEnrollment enrollment in this.enrollmentEnum)
            {
                this.enrollmentDto = new EnrollmentDto();
                //this.vehicleBuilder.export(enrollment);
                this.enrollmentDto = this.vehicleBuilder.export(enrollment);
                this.enrollmentDtoList.Add(this.enrollmentDto);
            }
            this.ViewBag.Message = "Enrollment list";
            return View(this.enrollmentDtoList);
        }
        // GET: Vehicles
        [HttpGet]
        public ActionResult Edit(string serial, int number)
        {
            this.vehicleDto = getVehicleData(serial, number);
            return View(this.vehicleDto);
        }
        // SET: Vehicles
        [HttpGet]
        public ActionResult Save(VehicleDto vehicleDto)
        {
            this.vehicleStorage.set(this.vehicleBuilder.import(vehicleDto));
            return View(getVehicleData(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number));
        }

        private VehicleDto getVehicleData(string serial, int number)
        {
            return this.vehicleDto = this.vehicleBuilder.export(this.vehicleStorage.get()
            .whereEnrollmentIs(this.enrollmentProvider.import(serial, number))
            .Select(vehicle => vehicle)
            .Single());
        }
    }
}