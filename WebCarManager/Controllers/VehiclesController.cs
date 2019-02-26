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
        private const string CONNECTION_STRING = @"Server=localhost\SQLEXPRESS;Database=CarManagement;User Id=test2;Password=123456;MultipleActiveResultSets=True";

        private readonly IVehicleStorage vehicleStorage;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
        }

        // GET: Vehicles
        public ActionResult Index()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IVehicleStorage vehicleStorage = new SqlVehicleStorage(CONNECTION_STRING, vehicleBuilder);

            IEnumerable<IEnrollment> enrollments = vehicleStorage.get().Keys;

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