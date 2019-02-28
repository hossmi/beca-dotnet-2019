using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarManagement.Core.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Services;
using System;
using CarManagement.Core;
using WebCarManager.Services;

namespace WebCarManager.Controllers
{
    public class VehiclesController : AbstractController
    {
        private readonly IVehicleStorage vehicleStorage;
        private readonly IEnrollmentProvider enrollmentProvider;
        private readonly IVehicleBuilder vehicleBuilder;

        public VehiclesController()
        {
            this.vehicleStorage = getService<IVehicleStorage>();
            this.enrollmentProvider = getService<IEnrollmentProvider>();
            this.vehicleBuilder = getService<IVehicleBuilder>();
        }

        // GET: Vehicles
        public ActionResult Index()
        {
            IEnumerable<IEnrollment> enrollments = this.vehicleStorage.get().Keys;

            return View(enrollments);
        }

        public ActionResult CreateNew()
        {
            VehicleDto vehicleDto = new VehicleDto();

            return View(vehicleDto);
        }

        [HttpPost]
        public ActionResult CreateNew(VehicleDto vehicleDto)
        {
            if (validateValues(vehicleDto))
            {
                this.vehicleBuilder.setColor(vehicleDto.Color);
                this.vehicleBuilder.setEngine(vehicleDto.Engine.HorsePower);
                this.vehicleBuilder.setDoors(vehicleDto.Doors.Length);

                for (int i = 0; i < vehicleDto.Wheels.Length; i++)
                {
                    this.vehicleBuilder.addWheel();
                }

                IVehicle vehicle = this.vehicleBuilder.build();


                if (vehicle.Engine.IsStarted)
                {
                    vehicle.Engine.start();
                }

                foreach (IWheel wheel in vehicleDto.Wheels)
                {
                    wheel.Pressure = wheel.Pressure;
                }

                this.vehicleStorage.set(vehicle);

                return View("Details", vehicleDto);
            }


            return View();
        }

        private bool validateValues(VehicleDto vehicleDto)
        {
            bool validateValueColor = false;
            bool validateValueNumberWheel = false;
            bool validateValueNumberDooor = false;
            bool validateValueHorsePower = false;
            bool validateValuesWheelsPressure = true;
            int numberMinWheel = 1;
            int numberManWheel = 4;
            int numberMinDoor = 0;
            int numberMaxDoor = 6;
            int pressureMin = 1;
            int pressureMax = 5;
            int numberWheelsCorrectPressure = vehicleDto.Wheels.Length;


            validateValueColor = Enum.IsDefined(typeof(VehicleDto), vehicleDto.Color);
            validateValueNumberWheel = (vehicleDto.Wheels.Length >= numberMinWheel && vehicleDto.Wheels.Length <= numberManWheel);
            validateValueNumberDooor = (vehicleDto.Doors.Length >= numberMinDoor && vehicleDto.Doors.Length <= numberMaxDoor);
            validateValueHorsePower = (vehicleDto.Engine.HorsePower > 0);
                
            
            //OJO no esta bien el validate presión....son las 5 de la mañana y no  veo la solución
            
            foreach (WheelDto wheelDto in vehicleDto.Wheels)
            {
                if ( (wheelDto.Pressure >= pressureMin && wheelDto.Pressure <= pressureMax) 
                    && validateValuesWheelsPressure )
                {
                    validateValuesWheelsPressure = true;
                }
            }

            return (validateValueColor && validateValueNumberWheel && validateValueNumberDooor && validateValueHorsePower && validateValuesWheelsPressure);
        }



        public ActionResult Delete(string serial, int number)
        {
            IEnrollment enrollment = this.enrollmentProvider.import(serial, number);

            vehicleStorage.remove(enrollment);

            return RedirectToAction("Index");
        }


        public ActionResult Details(string serial, int number)
        {
            VehicleDto vehicleDto = getVehicleDto(serial, number, this.vehicleStorage,
                this.enrollmentProvider, this.vehicleBuilder);

            return View(vehicleDto);
        }

        private static VehicleDto getVehicleDto(string serial, int number,
            IVehicleStorage vehicleStorage, IEnrollmentProvider enrollmentProvider,
            IVehicleBuilder vehicleBuilder)
        {
            IEnrollment enrollment = enrollmentProvider.import(serial, number);

            IVehicle vehicle = vehicleStorage.get().whereEnrollmentIs(enrollment).Single();

            VehicleDto vehicleDto = vehicleBuilder.export(vehicle);


            return vehicleDto;
        }
    }
}