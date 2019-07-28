using System.Web.Mvc;
using CarManagement.Core.Models.DTOs;
using WebCarManager.Models;

namespace WebCarManager.Controllers
{
    public class VehiclesController : Controller
    {
        private Vehicle vehicle;
        public VehiclesController()
        {
            this.vehicle = new Vehicle();
        }

        public ActionResult Index()
        {
            this.ViewBag.Message = "Index";
            return View(this.vehicle.enrollmentDTOList());
        }

        // GET: Vehicles
        [HttpGet]
        public ActionResult Edit(string serial, int number)
        {
            this.ViewBag.Message = "Edit Vehicle";
            return View(this.vehicle.getVehicle(serial, number));
        }

        // SET: Vehicles
        [HttpPost]
        public ActionResult Edit(VehicleDto vehicleDto)
        {
            this.ViewBag.Message = "Edited Vehicle";
            return View(this.vehicle.Edit(vehicleDto));
        }
    }
}