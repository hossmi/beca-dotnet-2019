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
            VehicleDto[] vehicleDtos = new VehicleDto[]
            {
                new VehicleDto
                {
                     Enrollment= new EnrollmentDto()
                     {
                          Number=666,
                           Serial="BBB"
                     },
                      Color = CarManagement.Core.Models.CarColor.Black
                }
            };

            return View(vehicleDtos);
        }
    }
}