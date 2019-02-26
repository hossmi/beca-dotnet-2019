using System.Collections.Generic;
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

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
        }

        // GET: Vehicles
        public ActionResult Index()
        {
            IEnumerable<IEnrollment> enrollments = this.vehicleStorage.get().Keys;

            return View(enrollments);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(string serial, int number)
        {
            return View();
        }

        public ActionResult Details(string serial, int number)
        {
            return View();
        }

        public ActionResult Delete(string serial, int number)
        {
            return View();
        }
    }
}