using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using CarManagement.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCarManager.Controllers
{
    public class VehiclesController : AbstractController
    {
        private readonly IVehicleStorage vehicleStorage;
        private readonly IVehicleBuilder vehicleBuilder;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.vehicleBuilder = getService<IVehicleBuilder>();
        }

        // GET: Vehicles
        public ActionResult Index()
        {
            IEnumerable<IEnrollment> enrollments = this.vehicleStorage.get().Keys;

            /*VehicleDto[] vehicles = new VehicleDto[]
            {
                new VehicleDto
                {
                    Enrollment = new EnrollmentDto
                    {
                        Serial = "JVC",
                        Number = 300,
                    },
                    Color = CarColor.Black,
                },
                
            };*/

            return View(enrollments);
        }

        public ActionResult Details(string serial, int number)
        {
            this.ViewBag.Title = "Details";

            IEnrollment enrollment = this.vehicleBuilder.import(serial, number);
            IVehicle vehicle = this.vehicleStorage.get()
                .whereEnrollmentIs(enrollment).Single();
            
            return View(vehicle);
        }
    }
}