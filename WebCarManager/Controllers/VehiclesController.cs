using System.Collections.Generic;
using System.Web.Mvc;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Services;

namespace WebCarManager.Controllers
{
    public class VehiclesController : Controller
    {
        // GET: Vehicles
        public ActionResult Index()
        {
            string connectionString = @"Server=localhost\SQLEXPRESS;Database=CarManagement;User Id=test;Password=123456; MultipleActiveResultSets=True;";
            VehicleBuilder vehicleBuilder = new VehicleBuilder(new DefaultEnrollmentProvider());
            SqlVehicleStorage vehicleStorage = new SqlVehicleStorage(connectionString, vehicleBuilder);
            List<VehicleDto> vehicles = new List<VehicleDto>();

            foreach (IVehicle vehicle in vehicleStorage.get())
            {
                vehicles.Add(vehicleBuilder.export(vehicle));
            }

            return View(vehicles);
        }

        public ActionResult Details(string serial, int number)
        {
            this.ViewData.Add("Serial", serial);
            this.ViewData.Add("Number", number);

            return View();
        }
    }
}