using System.Web.Mvc;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using CarManagement.Services;

namespace WebCarManager.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly string connectionString;
        private IEnrollmentProvider enrollmentProvider;
        private IVehicleBuilder vehiclebuilder;
        private SqlVehicleStorage sqlVehicleStorage;
        private VehicleDto[] vehicles;

        // GET: Vehicles
        public ActionResult Index()
        {
            this.enrollmentProvider = new DefaultEnrollmentProvider();
            //IEnrollment enrollment = enrollmentProvider.getNew();
            this.vehiclebuilder = new VehicleBuilder(this.enrollmentProvider);
            this.sqlVehicleStorage = new SqlVehicleStorage(this.connectionString, this.vehiclebuilder);
            this.vehicles = new VehicleDto[]
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
                    Color = CarColor.Red,
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
            this.sqlVehicleStorage.get();

            return View(this.vehicles);
        }
    }
}