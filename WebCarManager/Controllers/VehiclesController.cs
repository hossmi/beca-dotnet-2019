using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using CarManagement.Services;

namespace WebCarManager.Controllers
{
    public class VehiclesController : Controller
    {
        private const string connectionString = @"Server=localhost\SQLEXPRESS;Database=CarManagement;User Id=test;Password=123456; MultipleActiveResultSets=True;";

        // GET: Vehicles
        public ActionResult Index()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            SqlVehicleStorage vehicleStorage = new SqlVehicleStorage(connectionString, vehicleBuilder);

            return View(vehicleStorage.get().Keys);
        }

        public ActionResult Details(string serial, int number)
        {
            VehicleDto vehicle = GetVehicleData(serial, number);

            return View(vehicle);
        }

        public ActionResult Edit(string serial, int number)
        {
            VehicleDto vehicle = GetVehicleData(serial, number);

            return View(vehicle);
        }

        [HttpPost]
        public ActionResult Edit(VehicleDto vehicleDto)
        {
            SetVehicleData(vehicleDto);
            return View(vehicleDto);
        }

        private VehicleDto GetVehicleData(string serial, int number)
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            SqlVehicleStorage vehicleStorage = new SqlVehicleStorage(connectionString, vehicleBuilder);
            IEnrollment enrollment = enrollmentProvider.import(serial,number);
            IVehicle vehicle = vehicleStorage.get().whereEnrollmentIs(enrollment).Single();
            VehicleDto vehicleDto = vehicleBuilder.export(vehicle);
                       
            return vehicleDto;
        }

        private void SetVehicleData(VehicleDto vehicleDto)
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            SqlVehicleStorage vehicleStorage = new SqlVehicleStorage(connectionString, vehicleBuilder);
            IVehicle vehicle = vehicleBuilder.import(vehicleDto);
            vehicleStorage.set(vehicle);
        }
    }
}