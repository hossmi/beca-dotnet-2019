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
        }

        public ActionResult Index()
        {
            this.enrollmentDtoList = new List<EnrollmentDto>();
            this.enrollmentEnum = this.vehicleStorage
                .get()
                .Keys;
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
            this.vehicleDto = getVehicleData(serial, number);
            return View(this.vehicleDto);
        }
        // SET: Vehicles
        [HttpGet]
        public ActionResult Save(VehicleDto vehicleDto)
        {
            this.vehicleStorage.set(this.vehicleBuilder.import(vehicleDto));
            this.vehicleDto = new VehicleDto();
            this.vehicleDto = getVehicleData(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number);
            return View(this.vehicleDto);
        }

        private VehicleDto getVehicleData(string serial, int number)
        {
            IEnrollment enrollment = this.enrollmentProvider.import(serial, number);
            IEnumerable<IVehicle> vehicleList = this.vehicleStorage
                .get()
                .whereEnrollmentIs(enrollment);

            IVehicle Ivehicle = vehicleList
                //.Where(vehicle => vehicle.Enrollment == enrollment)
                //.Select(vehicle => vehicle)
                .Single();

            VehicleDto vehicleDto = new VehicleDto();
            vehicleDto = this.vehicleBuilder.export(Ivehicle);
            return vehicleDto;
        }
    }
}