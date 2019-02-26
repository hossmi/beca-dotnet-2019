using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace WebCarManager.Controllers
{
    public class VehiclesController : AbstractController
    {
        private readonly IVehicleStorage vehicleStorage;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
        }

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
                    HorsePower = 666,
                    IsStarted = true,
                },
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
                    Color = CarManagement.Core.Models.CarColor.Red,
                }
            };

            return View(vehicles);
        }
        public ActionResult Prueba()
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
                    HorsePower = 666,
                    IsStarted = true,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = true },
                    new Door { IsOpen = false },
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 4},
                    new Wheel{Pressure = 4},
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 2},
                },
                    Color = CarManagement.Core.Models.CarColor.Red,
            
                }
            };

            return View(vehicles);
        }
    }
}
