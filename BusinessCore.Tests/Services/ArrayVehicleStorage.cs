using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Models;
using CarManagement.Core.Models;
using CarManagement.Services;

namespace BusinessCore.Tests.Services
{
    public class ArrayVehicleStorage : AbstractVehicleStorage
    {
        public ArrayVehicleStorage() : base(initialVehicles())
        {
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
        }

        private static IDictionary<IEnrollment, IVehicle> initialVehicles()
        {
            return buildVehicles()
                .ToDictionary(v => v.Enrollment, new EnrollmentEqualityComparer());
        }

        private static IEnumerable<IVehicle> buildVehicles()
        {
            yield return new Vehicle
            {
                Color = CarColor.White,
                Enrollment = new Enrollment
                {
                    Serial = "XXX",
                    Number = 666,
                },
                Engine = new Engine
                {
                    HorsePower = 600,
                    IsStarted = true,
                },
                Doors = new IDoor[]
                {
                    new Door
                    {
                        IsOpen = true,
                    }
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 4},
                    new Wheel{Pressure = 4},
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 2},
                },
            };
            yield return new Vehicle
            {
                Color = CarColor.Black,
                Enrollment = new Enrollment
                {
                    Serial = "XXX",
                    Number = 666,
                },
                Engine = new Engine
                {
                    HorsePower = 600,
                    IsStarted = true,
                },
                Doors = new IDoor[]
                {
                    new Door
                    {
                        IsOpen = true,
                    }
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 6},
                },
            };
            yield return new Vehicle
            {
                Color = CarColor.Black,
                Enrollment = new Enrollment
                {
                    Serial = "XXX",
                    Number = 666,
                },
                Engine = new Engine
                {
                    HorsePower = 600,
                    IsStarted = true,
                },
                Doors = new IDoor[]
                {
                    new Door
                    {
                        IsOpen = true,
                    }
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 6},
                },
            };
            yield return new Vehicle
            {
                Color = CarColor.Black,
                Enrollment = new Enrollment
                {
                    Serial = "XXX",
                    Number = 666,
                },
                Engine = new Engine
                {
                    HorsePower = 600,
                    IsStarted = true,
                },
                Doors = new IDoor[]
                {
                    new Door
                    {
                        IsOpen = true,
                    }
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 6},
                },
            };
            yield return new Vehicle
            {
                Color = CarColor.Black,
                Enrollment = new Enrollment
                {
                    Serial = "XXX",
                    Number = 666,
                },
                Engine = new Engine
                {
                    HorsePower = 666,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door
                    {
                        IsOpen = true,
                    }
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 6},
                },
            };
            yield return new Vehicle
            {
                Color = CarColor.Black,
                Enrollment = new Enrollment
                {
                    Serial = "XXX",
                    Number = 666,
                },
                Engine = new Engine
                {
                    HorsePower = 600,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door
                    {
                        IsOpen = true,
                    }
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 6},
                },
            };
            yield return new Vehicle
            {
                Color = CarColor.Black,
                Enrollment = new Enrollment
                {
                    Serial = "XXX",
                    Number = 666,
                },
                Engine = new Engine
                {
                    HorsePower = 100,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door
                    {
                        IsOpen = true,
                    }
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 6},
                },
            };
            yield return new Vehicle
            {
                Color = CarColor.White,
                Enrollment = new Enrollment
                {
                    Serial = "XXX",
                    Number = 666,
                },
                Engine = new Engine
                {
                    HorsePower = 600,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door
                    {
                        IsOpen = true,
                    }
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 5},
                    new Wheel{Pressure = 5},
                },
            };
        }
    }
}
