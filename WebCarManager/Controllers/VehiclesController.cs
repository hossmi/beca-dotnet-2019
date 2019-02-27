using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using CarManagement.Services;
using CarManagement.Core.Services;
using System.Configuration;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using System.Linq;

namespace WebCarManager.Controllers
{
    public class VehiclesController : AbstractController
    {
        private readonly IVehicleStorage vehicleStorage;
        private readonly IEnrollmentProvider enrollmentProvider;
        private readonly IVehicleBuilder vehicleBuilder;

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

        public ActionResult CreateNew()
        {
            VehicleDto vehicleDto = new VehicleDto();

            return View(vehicleDto);
        }

        [HttpPost]
        public ActionResult CreateNew(VehicleDto vehicleDto)
        {


            return View("Details", vehicleDto);
        }

        public ActionResult Delete(string serial, int number)
        {
            IEnrollment enrollment = this.enrollmentProvider.import(serial, number);

            vehicleStorage.remove(enrollment);

            return RedirectToAction("Index");
        }


        public ActionResult Details(string serial, int number)
        {
            VehicleDto vehicleDto = getVehicleDto(serial, number, this.vehicleStorage,
                this.enrollmentProvider, this.vehicleBuilder);

            return View(vehicleDto);
        }

        private static VehicleDto getVehicleDto(string serial, int number,
            IVehicleStorage vehicleStorage, IEnrollmentProvider enrollmentProvider,
            IVehicleBuilder vehicleBuilder)
        {
            IEnrollment enrollment = enrollmentProvider.import(serial, number);

            IVehicle vehicle = vehicleStorage.get().whereEnrollmentIs(enrollment).Single();

            VehicleDto vehicleDto = vehicleBuilder.export(vehicle);


            return vehicleDto;
        }
    }
}