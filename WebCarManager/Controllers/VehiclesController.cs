using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using CarManagement.Services;

namespace WebCarManager.Controllers
{
    public class VehiclesController : AbstractController
    {
        private readonly IVehicleStorage vehicleStorage;
        private readonly IEnrollmentProvider enrollmentProvider;
        private IVehicleBuilder vehicleBuilder;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.enrollmentProvider = getService<IEnrollmentProvider>();
            this.vehicleBuilder = getService<IVehicleBuilder>();
        }

        // GET: Vehicles
        public ActionResult Index()
        {
            List<EnrollmentDto> enrollments = new List<EnrollmentDto>();
            foreach (IEnrollment enrollment in this.vehicleStorage.get().Keys)
            {
                EnrollmentDto enrollmentDto = new EnrollmentDto()
                {
                    Serial = enrollment.Serial,
                    Number = enrollment.Number,
                };
                enrollments.Add(enrollmentDto);
            }

            return View(enrollments);
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

        public ActionResult Create()
        {
            NewVehicleData vehicleData = new NewVehicleData();
            return View(vehicleData);
        }

        public ActionResult Delete(string serial, int number)
        {
            EnrollmentDto enrollmentDto = new EnrollmentDto()
            {
                Serial = serial,
                Number = number,
            };
            return View(enrollmentDto);
        }

        [HttpPost]
        public ActionResult Delete(EnrollmentDto enrollmentDto)
        {
            IEnrollment enrollment = this.enrollmentProvider.import(enrollmentDto.Serial, enrollmentDto.Number);
            this.vehicleStorage.remove(enrollment);
            return this.RedirectToAction("Index");
            //return View(enrollmentDto);
        }


        [HttpPost]
        public ActionResult Create(NewVehicleData vehicleData)
        {
            IVehicle vehicle;
            VehicleBuilder builder = new VehicleBuilder(this.enrollmentProvider);

            builder.setColor(vehicleData.Color);
            builder.setEngine(vehicleData.HorsePower);
            builder.setDoors(vehicleData.DoorCount);

            for (int i = 0; i < vehicleData.WheelCount; i++)
            {
                builder.addWheel();
            }

            vehicle = builder.build();

            if (vehicleData.IsStarted == true)
            {
                vehicle.Engine.start();
            }

            foreach (IWheel wheel in vehicle.Wheels)
            {
                wheel.Pressure = vehicleData.Pressure;
            }
        
            this.vehicleStorage.set(vehicle);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(VehicleDto vehicleDto)
        {
            SetVehicleData(vehicleDto);

            return View(vehicleDto);
        }

        private VehicleDto GetVehicleData(string serial, int number)
        {
            IEnrollment enrollment = this.enrollmentProvider.import(serial, number);
            VehicleDto vehicleDto = this.vehicleStorage.get().whereEnrollmentIs(enrollment).Select(this.vehicleBuilder.export).Single();

            return vehicleDto;
        }

        private void SetVehicleData(VehicleDto vehicleDto)
        {
            IVehicle vehicle = this.vehicleBuilder.import(vehicleDto);
            this.vehicleStorage.set(vehicle);
        }

        public class NewVehicleData
        {
            private int wheelCount;
            private int doorCount;
            private int horsePower;
            private bool isStarted;
            private CarColor color;
            private double pressure;

            public int WheelCount { get => this.wheelCount; set => this.wheelCount = value; }
            public int DoorCount { get => this.doorCount; set => this.doorCount = value; }
            public int HorsePower { get => this.horsePower; set => this.horsePower = value; }
            public bool IsStarted { get => this.isStarted; set => this.isStarted = value; }
            public CarColor Color { get => this.color; set => this.color = value; }
            public double Pressure { get => this.pressure; set => this.pressure = value; }

        }
            


    }
}