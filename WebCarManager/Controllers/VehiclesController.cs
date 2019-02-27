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
        private readonly IVehicleBuilder vehicleBuilder;
        private readonly IEnrollmentProvider enrollmentProvider;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.vehicleBuilder = getService<IVehicleBuilder>();
            this.enrollmentProvider = getService<IEnrollmentProvider>();
        }

        // GET: Vehicles
        public ActionResult Index()
        {
            IEnumerable<IEnrollment> enrollments = this.vehicleStorage.get().Keys;

            return View(enrollments);
        }

        public ActionResult Create()
        {
            return View(new VehicleDto { Engine= new EngineDto { HorsePower= 50, IsStarted= false, } });
        }
        [HttpPost]
        public ActionResult Created(VehicleDto vehicleDto)
        {
            return View(vehicleDto);
        }

        public ActionResult Edit(string serial, int number)
        {
            VehicleDto vehicle = getVehicleAsDto(serial, number);

            return View(vehicle);
        }


        [HttpPost]
        public ActionResult Edit(VehicleDto vehicle)
        {
            return View(vehicle);
        }

        public ActionResult Details(string serial, int number)
        {
            VehicleDto vehicle = getVehicleAsDto(serial, number);
            return View(vehicle);
        }

        public ActionResult Delete(string serial, int number)
        {

            return View
                (
                this.enrollmentProvider
                    .import
                    (
                        serial,
                        number
                    )
                );
        }
        [HttpPost]
        public ActionResult Deleted(string serial, int number)
        {

            return View
                (
                this.enrollmentProvider
                    .import
                    (
                        serial,
                        number
                    )
                );
        }

        private VehicleDto getVehicleAsDto(string serial, int number)
        {
            return this.vehicleBuilder
            .export
            (
                this.vehicleStorage
                .get()
                .whereEnrollmentIs
                (
                    this.enrollmentProvider
                    .import
                    (
                        serial,
                        number
                    )
                )
                .Single()
            );
        }
    }
}