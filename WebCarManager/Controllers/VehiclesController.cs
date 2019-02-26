using System.Web.Mvc;
using CarManagement.Core.Models.DTOs;

namespace WebCarManager.Controllers
{
    public class VehiclesController : Controller
    {
        
        // GET: Vehicles
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
    }
}