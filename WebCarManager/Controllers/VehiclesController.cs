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
    public class VehiclesController : Controller
    {
        private const string CONNECTION_STRING = "CarManagerConnectionString";
        // GET: Vehicles
        public ActionResult Index()
        {
            VehicleDto[] vehicles = new VehicleDto[]
            {
                new VehicleDto
                {
                    Enrollment = new EnrollmentDto
                    {
                        Serial = "XXX",
                        Number = 6666,
                    },
                    Color = CarColor.Black,
                },
                
            };

            return View(vehicles);
        }

        public ActionResult Detalles()
        {
            string connectionString = ConfigurationManager.AppSettings[CONNECTION_STRING];
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IVehicleStorage vehicleStorage = new SqlVehicleStorage(connectionString, vehicleBuilder);
            IEnumerable<IVehicle> vehicles = vehicleStorage.get();
            //vehicles.Select(vehicle => vehicleStorage.get().whereEnrollmentIs());
            return View(vehicles);
        }
    }
}