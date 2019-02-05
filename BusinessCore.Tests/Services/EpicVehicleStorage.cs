using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Models;
using CarManagement.Core.Models;
using CarManagement.Services;

namespace BusinessCore.Tests.Services
{
    public class EpicVehicleStorage : AbstractVehicleStorage
    {
        public EpicVehicleStorage() : base(initialVehicles())
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
            #region "VehiclesArray"
            yield return new Vehicle
            {
                Color = CarColor.White,
                Enrollment = new Enrollment
                {
                    Serial = "AAA",
                    Number = 0000,
                },
                Engine = new Engine
                {
                    HorsePower = 200,
                    IsStarted = false,
                },
                Doors = new IDoor[]
     {
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
     },
                Wheels = new IWheel[]
     {
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 2},
     },
            };

            yield return new Vehicle
            {
                Color = CarColor.Green,
                Enrollment = new Enrollment
                {
                    Serial = "BBB",
                    Number = 0010,
                },
                Engine = new Engine
                {
                    HorsePower = 200,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 2},
                },
            };

            yield return new Vehicle
            {
                Color = CarColor.White,
                Enrollment = new Enrollment
                {
                    Serial = "AAA",
                    Number = 1000,
                },
                Engine = new Engine
                {
                    HorsePower = 200,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = false },
                    new Door { IsOpen = false },
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 4},
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 2},
                },
            };

            yield return new Vehicle
            {
                Color = CarColor.Yellow,
                Enrollment = new Enrollment
                {
                    Serial = "CCC",
                    Number = 0000,
                },
                Engine = new Engine
                {
                    HorsePower = 300,
                    IsStarted = true,
                },
                Doors = new IDoor[]
                {
                    new Door { IsOpen = true },
                    new Door { IsOpen = true },
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 3},
                    new Wheel{Pressure = 3},
                    new Wheel{Pressure = 3},
                },
            };

            yield return new Vehicle
            {
                Color = CarColor.Red,
                Enrollment = new Enrollment
                {
                    Serial = "ABC",
                    Number = 0000,
                },
                Engine = new Engine
                {
                    HorsePower = 200,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 2},
                },
            };

            yield return new Vehicle
            {
                Color = CarColor.Black,
                Enrollment = new Enrollment
                {
                    Serial = "ABC",
                    Number = 0001,
                },
                Engine = new Engine
                {
                    HorsePower = 250,
                    IsStarted = false,
                },
                Doors = new IDoor[]
                {
                },
                Wheels = new IWheel[]
                {
                    new Wheel{Pressure = 2},
                    new Wheel{Pressure = 2},
                },
            };
            #endregion
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
