using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Models;
using CarManagement.Core.Models;
using CarManagement.Services;

namespace BusinessCore.Tests.Services
{
    public class ArrayVehicleStorage_amunoz : AbstractVehicleStorage
    {
        public ArrayVehicleStorage_amunoz() : base(initialVehicles())
        {
        }

        protected override void save(IEnumerable<IVehicle> vehicles)
        {
        }

        private static IDictionary<IEnrollment, IVehicle> initialVehicles()
        {
            return buildVehicles()
                .ToDictionary(v => v.Enrollment, new PrvEnrollmentComparer());
        }

        private static IEnumerable<IVehicle> buildVehicles()
        {
            yield return new Vehicle
            {
                Color = CarColor.Yellow,
                Enrollment = new Enrollment
                {
                    Serial = "PNG",
                    Number = 0100,
                },
                Engine = new Engine
                {
                    HorsePower = 666,
                    IsStarted = true,
                },
                Doors = new IDoor[]
                {
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 4},
                    new Wheel{Pressure = 2},
                },
            };

            yield return new Vehicle
            {
                Color = CarColor.Yellow,
                Enrollment = new Enrollment
                {
                    Serial = "PNG",
                    Number = 0200,
                },
                Engine = new Engine
                {
                    HorsePower = 600,
                    IsStarted = true,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = true },
                    new Door { IsOpen = false },
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 6},
                },
            };

            yield return new Vehicle
            {
                Color = CarColor.Yellow,
                Enrollment = new Enrollment
                {
                    Serial = "JVC",
                    Number = 0300,
                },
                Engine = new Engine
                {
                    HorsePower = 600,
                    IsStarted = true,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 6},
                    new Wheel{Pressure = 6},
                    new Wheel{Pressure = 6},
                },
            };

            yield return new Vehicle
            {
                Color = CarColor.Black,
                Enrollment = new Enrollment
                {
                    Serial = "JVC",
                    Number = 400,
                },
                Engine = new Engine
                {
                    HorsePower = 600,
                    IsStarted = true,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
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
                    Serial = "JVC",
                    Number = 1000,
                },
                Engine = new Engine
                {
                    HorsePower = 666,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
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
                    Serial = "ZZZ",
                    Number = 2000,
                },
                Engine = new Engine
                {
                    HorsePower = 600,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 6},
                },
            };

            yield return new Vehicle
            {
                Color = CarColor.Yellow,
                Enrollment = new Enrollment
                {
                    Serial = "ZZZ",
                    Number = 2100,
                },
                Engine = new Engine
                {
                    HorsePower = 100,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 6},
                },
            };

            yield return new Vehicle
            {
                Color = CarColor.Red,
                Enrollment = new Enrollment
                {
                    Serial = "ZZZ",
                    Number = 3000,
                },
                Engine = new Engine
                {
                    HorsePower = 666,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = true },
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 5},
                    new Wheel{Pressure = 5},
                },
            };

            yield return new Vehicle
            {
                Color = CarColor.White,
                Enrollment = new Enrollment
                {
                    Serial = "ZZZ",
                    Number = 3100,
                },
                Engine = new Engine
                {
                    HorsePower = 666,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = true },
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 5},
                    new Wheel{Pressure = 5},
                },
            };

            yield return new Vehicle
            {
                Color = CarColor.White,
                Enrollment = new Enrollment
                {
                    Serial = "ZZZ",
                    Number = 3300,
                },
                Engine = new Engine
                {
                    HorsePower = 666,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
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

        private class PrvEnrollmentComparer : IEqualityComparer<IEnrollment>
        {
            public bool Equals(IEnrollment x, IEnrollment y)
            {
                return x.Serial == x.Serial
                    && x.Number == x.Number;
            }

            public int GetHashCode(IEnrollment x)
            {
                return $"{x.Serial}{x.Number}".GetHashCode();
            }
        }
    }
}
