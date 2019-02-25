using CarManagement.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                    Enrollment = new EnrollmentDto()
                    {
                        Serial = "AAAA",
                        Number = 0000

                    },
                    Color = CarManagement.Core.Models.CarColor.Green
                }
            };
            return View(vehicles);
        }
        public ActionResult Edit()
        {
            return View();
        }
    }
}