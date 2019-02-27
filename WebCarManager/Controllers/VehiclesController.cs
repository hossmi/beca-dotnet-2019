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
    public class VehiclesController : AbstractController
    {
        private const string CONNECTION_STRING_KEY = "CarManagerConnectionString";
        private readonly string connectionString = ConfigurationManager.AppSettings[CONNECTION_STRING_KEY];
        private IVehicleStorage vehicleStorage;
        private IEnrollmentProvider enrollmentProvider;
        private IEnrollment enrollment;
        //private ;

        // GET: Vehicles
        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.enrollmentProvider = getService<IEnrollmentProvider>();
            this.enrollment = getService<IEnrollment>();
        }
        public ActionResult Index()
        {
            //IEnumerable<IEnrollment> enrollmentList = getEnrollments(this.connectionString);
            IEnumerable<IEnrollment> enrollmentList = this.vehicleStorage.get().Keys;
            return View(enrollmentList);
        }
        public ActionResult Edit()
        {

            return View();
        }
        public ActionResult Delete(string serial, int number)
        {
            IEnrollment enrollment = this.enrollmentProvider.import(serial, number);
            IVehicle vehicle = this.vehicleStorage.get().whereEnrollmentIs(enrollment).Single();
            return View(vehicle);
        }

        [HttpPost]
        public ActionResult Delete(VehicleDto vehicleDto)
        {
            //this.Session["infoMessage"] = "The vehicle has been deleted";
            //IEnrollment enrollment = this.enrollmentProvider.import(vehicleDto.Enrollment.Serial, vehicleDto.Enrollment.Number);
            //this.vehicleStorage.remove(enrollment);
            return View();
        }

        //private IEnumerable<IEnrollment> getEnrollments(string connectionString)
        //{
        //    IEnrollmentProvider enrollmentprovider = new DefaultEnrollmentProvider();
        //    IVehicleBuilder vehicle = new VehicleBuilder(enrollmentprovider);
        //    SqlVehicleStorage sqlVehicle = new SqlVehicleStorage(connectionString, vehicle);
        //    List<IEnrollment> enrollments = new List<IEnrollment>();


        //    foreach (IEnrollment enrollment in sqlVehicle.get().Keys)
        //    {
        //        enrollments.Add(enrollment);

        //    }
        //    return enrollments;


        //} 
    }
}