using System.Web.Mvc;
using CarManagement.Core.Models;
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
                    Engine = new EngineDto
                    {
                        HorsePower = 100,
                        IsStarted = true
                    },
                    Color = CarManagement.Core.Models.CarColor.Red,
                    Doors = new DoorDto[]
                    {
                        new DoorDto { IsOpen = true },
                        new DoorDto { IsOpen = false },
                    },
                    Wheels = new WheelDto[]
                    {
                        new WheelDto{Pressure = 4},
                        new WheelDto{Pressure = 4},
                        new WheelDto{Pressure = 2},
                        new WheelDto{Pressure = 2},
                    },

                }
            };

            return View(vehicles);
        }
    }
}