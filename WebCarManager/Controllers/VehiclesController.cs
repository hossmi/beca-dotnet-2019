using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using CarManagement.Services;
using CarManagement.Core.Services;
using System.Configuration;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using System.Linq;

namespace WebCarManager.Controllers
{
    public class VehiclesController : Controller
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


        public ActionResult Details(string serial, int number)
        {
            VehicleDto vehicleDto = getVehicleDto(serial, number, CONNECTION_STRING_KEY);

            return View(vehicleDto);
        }

        private static VehicleDto getVehicleDto(string serial, int number, string connection)
        {
            String connectionString = ConfigurationManager.AppSettings[connection];
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IVehicleStorage vehicleStorage = new SqlVehicleStorage(connectionString, vehicleBuilder);

            IEnrollment enrollment = enrollmentProvider.import(serial, number);

            IVehicle vehicle = vehicleStorage.get().whereEnrollmentIs(enrollment).Single();

            VehicleDto vehicleDto = vehicleBuilder.export(vehicle);


            return vehicleDto;
        }
    }
}