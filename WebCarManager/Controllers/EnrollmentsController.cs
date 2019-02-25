using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarManagement.Services;
using CarManagement.Core.Services;
using System.Configuration;

namespace WebCarManager.Controllers
{
    public class EnrollmentsController : Controller
    {
        // GET: Enrollments
        private readonly connectionString = "CarManagerConnectionString";
        public ActionResult Index()
        {
            this.connectionString = ConfigurationManager.AppSettings[CONNECTION_STRING_KEY];
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            SqlVehicleStorage enrollments = new SqlVehicleStorage();
            return View();
        }
    }
}