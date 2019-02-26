using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
        //private string connectionString = ConfigurationManager.AppSettings["CarManagerConnectionString"];
        private string connectionString = @"Data Source = localhost\SQLEXPRESS;DataBase=CarManagement;Persist Security Info=True;User ID = test; Password=123456;MultipleActiveResultSets=true;";

        private IEnrollmentProvider enrollmentProvider;
        private IVehicleBuilder vehiclebuilder;
        private SqlVehicleStorage sqlVehicleStorage;
        private IEnumerable<IEnrollment> enrollmentEnum;
        private EnrollmentDto enrollmentDto;
        private List<EnrollmentDto> enrollmentDtoList;
        private VehicleDto vehicleDto;
        private CarColor carcolor;
        private EngineDto engineDto;
        private List<WheelDto> wheelsDto;
        private List<DoorDto> doorsDto;
        private IEnrollment enrollment;
        private IVehicle vehicle;
        private WheelDto wheelDto;
        private DoorDto doorDto;

        // GET: Vehicles
        public ActionResult Index()
        {
            this.enrollmentProvider = new DefaultEnrollmentProvider();
            this.vehiclebuilder = new VehicleBuilder(this.enrollmentProvider);
            this.sqlVehicleStorage = new SqlVehicleStorage(this.connectionString, this.vehiclebuilder);
            this.enrollmentEnum = this.sqlVehicleStorage.get().Keys;
            this.enrollmentDto = new EnrollmentDto();
            this.enrollmentDtoList = new List<EnrollmentDto>();
            this.enrollmentDto = new EnrollmentDto();
            foreach (IEnrollment enrollment2 in this.enrollmentEnum)
            {
                this.enrollmentDto.Number = enrollment2.Number;
                this.enrollmentDto.Serial = enrollment2.Serial;

                this.enrollmentDtoList.Add(this.enrollmentDto);
            }
            this.ViewBag.Message = "Enrollment list";
            return View(this.enrollmentDtoList);
        }
        [HttpGet]
        public ActionResult Edit(string serial, int number)
        {
            this.enrollmentProvider = new DefaultEnrollmentProvider();
            this.vehiclebuilder = new VehicleBuilder(this.enrollmentProvider);
            this.sqlVehicleStorage = new SqlVehicleStorage(this.connectionString, this.vehiclebuilder);

            this.enrollment = this.vehiclebuilder.import(serial, number);
            this.vehicle = this.sqlVehicleStorage.get()
                .whereEnrollmentIs(this.enrollment)
                .Select(vehicle => vehicle)
                .Single();
            this.vehicleDto = new VehicleDto();
            this.carcolor = new CarColor();
            this.engineDto = new EngineDto();
            this.wheelsDto = new List<WheelDto>();
            this.doorsDto = new List<DoorDto>();
            this.enrollmentDto = new EnrollmentDto();
            this.enrollmentDto.Number = this.vehicle.Enrollment.Number;
            this.enrollmentDto.Serial = this.vehicle.Enrollment.Serial;
            this.carcolor = this.vehicle.Color;
            this.engineDto.HorsePower = this.vehicle.Engine.HorsePower;
            this.engineDto.IsStarted = this.vehicle.Engine.IsStarted;
            foreach (IWheel wheel in this.vehicle.Wheels)
            {
                this.wheelDto = new WheelDto();
                this.wheelDto.Pressure = wheel.Pressure;
                this.wheelsDto.Add(this.wheelDto);
            }
            foreach (IDoor door in this.vehicle.Doors)
            {
                this.doorDto = new DoorDto();
                this.doorDto.IsOpen = door.IsOpen;
                this.doorsDto.Add(this.doorDto);
            }
            this.vehicleDto.Color = this.carcolor;
            this.vehicleDto.Enrollment = this.enrollmentDto;
            this.vehicleDto.Engine = this.engineDto;
            this.vehicleDto.Doors = this.doorsDto.ToArray();
            this.vehicleDto.Wheels = this.wheelsDto.ToArray();
            return View(this.vehicleDto);
        }
    }
}