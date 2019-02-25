using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using CarManagement.Services;
using CarManagement.Core.Services;
using System.Configuration;
using CarManagement.Core.Models;

namespace WebCarManager.Controllers
{
    public class EnrollmentsController : Controller
    {
        // GET: Enrollments
        private const string CONNECTION_STRING_KEY = "CarManagerConnectionString";
        private string connectionString;

        public ActionResult Index()
        {
            this.connectionString = ConfigurationManager.AppSettings[CONNECTION_STRING_KEY];
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IVehicleStorage vehicleStorage = new SqlVehicleStorage(this.connectionString, vehicleBuilder);

            IEnumerable<IEnrollment> enrollments = vehicleStorage.get().Keys;

            return View(enrollments);
        }
    }
}