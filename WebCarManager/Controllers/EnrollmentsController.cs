using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarManagement.Services;
using CarManagement.Core.Services;


namespace WebCarManager.Controllers
{
    public class EnrollmentsController : Controller
    {
        // GET: Enrollments
        public ActionResult Index()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            SqlVehicleStorage enrollments = new SqlVehicleStorage();
            return View();
        }
    }
}