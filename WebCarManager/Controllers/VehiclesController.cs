using CarManagement.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarManagement.Services;
using CarManagement.Extensions;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using System.Data.SqlClient;
using CarManagement.Services.CarManagement.Builders;
using System.Configuration;

namespace WebCarManager.Controllers
{
    public class VehiclesController : Controller
    {
        private const string CONNECTION_STRING_KEY = "CarManagerConnectionString";
        private readonly string connectionString = ConfigurationManager.AppSettings[CONNECTION_STRING_KEY];

        // GET: Vehicles
        public ActionResult Index()
        {
            getEnrollments(this.connectionString);
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Delete()
        {
            
            return View();
        }

        private void getEnrollments(string connectionString)
        {
            IEnrollmentProvider enrollmentprovider = new DefaultEnrollmentProvider();
            IVehicleBuilder vehicle = new VehicleBuilder(enrollmentprovider);
            SqlVehicleStorage sqlVehicle = new SqlVehicleStorage(connectionString, vehicle);
            List<EnrollmentDto> enrollments = new List<EnrollmentDto>();
            foreach (EnrollmentDto enrollment in sqlVehicle.get().Keys)
            {
                enrollments.Add(enrollment);
            }
 
        } 
    }
}