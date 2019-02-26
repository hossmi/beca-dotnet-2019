using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace WebCarManager.Controllers
{
    public class VehiclesController : AbstractController
    {
        private readonly IVehicleStorage vehicleStorage;
        private readonly IEnrollmentImporter enrollmentImporter;
        private readonly IVehicleBuilder vehicleBuilder;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.enrollmentImporter = getService<IEnrollmentImporter>();
            this.vehicleBuilder = getService<IVehicleBuilder>();
        }

        public ActionResult Index()
        {
            IEnumerable<IEnrollment> vehicles = this.vehicleStorage
                .get()
                .Keys;

            return View(vehicles);
        }

        public ActionResult View(string serial, int number)
        {
            return getView(serial, number);
        }

        public ActionResult Edit(string serial, int number)
        {
            return getView(serial, number);
        }

        [HttpPost]
        public ActionResult Edit(VehicleDto vehicle)
        {
            IVehicle builtVehicle = this.vehicleBuilder.import(vehicle);
            this.vehicleStorage.set(builtVehicle);
            return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult Delete(EnrollmentDto enrollment)
        {
            IEnrollment builtEnrollment = this.enrollmentImporter.import(enrollment.Serial, enrollment.Number);
            this.vehicleStorage.remove(builtEnrollment);
            return RedirectToAction("index");
        }

        private ActionResult getView(string serial, int number)
        {
            IEnrollment enrollment = this.enrollmentImporter.import(serial, number);

            VehicleDto vehicle = this.vehicleStorage
                .get()
                .whereEnrollmentIs(enrollment)
                .Select(this.vehicleBuilder.export)
                .SingleOrDefault();

            if (vehicle == null)
            {
                return RedirectToAction("index");
            }
            else
                return View(vehicle);
        }

    }
}