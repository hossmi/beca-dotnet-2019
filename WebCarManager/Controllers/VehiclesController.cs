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
        private readonly IVehicleBuilder vehicleBuilder;
        private IEnumerable<IEnrollment> enrollmentEnum;
        private IEnumerable<IVehicle> vehicleList;
        private List<EnrollmentDto> enrollmentDtoList;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.vehicleBuilder = getService<IVehicleBuilder>();
        }

        public ActionResult Index()
        {
            this.vehicleList = this.vehicleStorage
                .get();
            this.enrollmentDtoList = new List<EnrollmentDto>();
            this.enrollmentEnum = this.vehicleList
                .Select(vehicle => vehicle.Enrollment);
            foreach (IEnrollment enrollment in this.enrollmentEnum)
            {
                this.enrollmentDtoList.Add(
                    new EnrollmentDto(
                        enrollment.Serial,
                        enrollment.Number));
            }
            this.ViewBag.Message = "Enrollment list";
            return View(this.enrollmentDtoList);
        }

        // GET: Vehicles
        [HttpGet]
        public ActionResult Edit(string serial, int number)
        {
            this.ViewBag.Message = "Edit Vehicle";
            return View(getVehicle(serial, number));
        }

        // SET: Vehicles
        [HttpGet]
        public ActionResult Save(VehicleDto vehicleDto)
        {
            this.vehicleStorage.set(this.vehicleBuilder.import(vehicleDto));
            this.ViewBag.Message = "Edited Vehicle";
            return View(
                getVehicle(
                    vehicleDto.Enrollment.Serial, 
                    vehicleDto.Enrollment.Number));
        }

        private VehicleDto getVehicle(string serial, int number)
        {
            return this.vehicleBuilder.export(
                this.vehicleStorage
                    .get()
                    .whereEnrollmentIs(this.vehicleBuilder.import(serial, number))
                    .Single());
            
        }
    }
}