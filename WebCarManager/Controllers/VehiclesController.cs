using System.Linq;
using System.Web.Mvc;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace WebCarManager.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly IVehicleBuilder vehicleBuilder;
        private readonly IVehicleStorage vehicleStorage;
        private readonly IEnrollmentProvider enrollmentProvider;

        public ActionResult Index()
        {
            VehicleDto[] vehicles = new VehicleDto[]
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
            };

            return View(vehicles);
        }

        public ActionResult Details(string serial, int number)
        {
            this.ViewBag.Title = "Details";

            IEnrollment enrollment = this.vehicleBuilder.import(serial, number);
            IVehicle vehicle = this.vehicleStorage.get().whereEnrollmentIs(enrollment).Single();

            return View(vehicle);

        }

        public ActionResult Delete(string serial, int number)
        {
            IEnrollment enrollment = this.enrollmentProvider.import(serial, number);
            IVehicle vehicle = this.vehicleStorage.get().whereEnrollmentIs(enrollment).Single();

            return View(vehicle);
        }
    }
}