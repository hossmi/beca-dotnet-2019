using System.Collections.Generic;
using System.Web.Mvc;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using CarManagement.Services;

namespace WebCarManager.Controllers
{
    public class VehiclesController : Controller
    {
        private const string CONNECTION_STRING = @"Server=localhost\SQLEXPRESS;Database=CarManagement;User Id=test2;Password=123456;MultipleActiveResultSets=True";
        // GET: Vehicles
        public ActionResult Index()
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            VehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IVehicleStorage vehicleStorage = new SqlVehicleStorage(CONNECTION_STRING, vehicleBuilder);

            IEnumerable<IEnrollment> enrollments = vehicleStorage.get().Keys;
                
            /*new VehicleDto[]
            {
                new VehicleDto
                {
                    Enrollment = new EnrollmentDto
                    {
                        Serial= "XXX",
                        Number = 666,
                    },
                    Color = CarManagement.Core.Models.CarColor.Red,
                }
            };*/

            return View(enrollments);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(string serial, int number)
        {
            return View();
        }

        public ActionResult Details(string serial, int number)
        {
            return View();
        }

        public ActionResult Delete(string serial, int number)
        {
            return View();
        }
    }
}