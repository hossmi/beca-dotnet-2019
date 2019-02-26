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
        private IVehicleBuilder vehicleBuilder;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.enrollmentProvider = getService<IEnrollmentProvider>();
            this.vehicleBuilder = getService<IVehicleBuilder>();
        }

        // GET: Vehicles
        public ActionResult Index()
        {
            IEnumerable<IEnrollment> enrollments = this.vehicleStorage.get().Keys;

            return View(enrollments);
        }

        public ActionResult Details(string serial, int number)
        {
            VehicleDto vehicle = GetVehicleData(serial, number);

            return View(vehicle);
        }

        public ActionResult Edit(string serial, int number)
        {
            VehicleDto vehicle = GetVehicleData(serial, number);

            return View(vehicle);
        }

        public ActionResult Create()
        {
            VehicleDto vehicleDto = new VehicleDto();
            return View(vehicleDto);
        }

        [HttpPost]
        public ActionResult Create(VehicleDto vehicleDto)
        {
            SetVehicleData(vehicleDto);

            return View(vehicleDto);
        }

        [HttpPost]
        public ActionResult Edit(VehicleDto vehicleDto)
        {
            SetVehicleData(vehicleDto);

            return View(vehicleDto);
        }

        private VehicleDto GetVehicleData(string serial, int number)
        {
            IEnrollment enrollment = this.enrollmentProvider.import(serial,number);
            VehicleDto vehicleDto = this.vehicleStorage.get().whereEnrollmentIs(enrollment).Select(this.vehicleBuilder.export).Single();

            return vehicleDto;
        }

        private void SetVehicleData(VehicleDto vehicleDto)
        {
            IVehicle vehicle = this.vehicleBuilder.import(vehicleDto);
            this.vehicleStorage.set(vehicle);
        }
    }
}