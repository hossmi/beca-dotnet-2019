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
            IEnumerable<IEnrollment> enrollmentList = getEnrollments(this.connectionString);
            return View(enrollmentList);
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Delete()
        {
            
            return View();
        }

        private IEnumerable<IEnrollment> getEnrollments(string connectionString)
        {
            IEnrollmentProvider enrollmentprovider = new DefaultEnrollmentProvider();
            IVehicleBuilder vehicle = new VehicleBuilder(enrollmentprovider);
            SqlVehicleStorage sqlVehicle = new SqlVehicleStorage(connectionString, vehicle);
            List<IEnrollment> enrollments = new List<IEnrollment>();
            

            foreach (IEnrollment enrollment in sqlVehicle.get().Keys)
            {
                enrollments.Add(enrollment);
                
            }
            return enrollments;
            
 
        } 
    }
}