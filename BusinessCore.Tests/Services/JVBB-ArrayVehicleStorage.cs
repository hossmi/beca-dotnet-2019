using System.Collections.Generic;
using System.Linq;
using BusinessCore.Tests.Models;
using CarManagement.Core.Models;
using CarManagement.Services;

namespace BusinessCore.Tests.Services
{
    public class JVBBArrayVehicleStorage : AbstractVehicleStorage
    {
        public JVBBArrayVehicleStorage() : base(initialVehicles())
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
            //METAL BAWKSES GO 'ERE!

            yield return new Vehicle
            {
                Color = CarColor.Purple,
                Enrollment = new Enrollment
                {
                    Serial = "CSM",
                    Number = 1000,
                },
                Engine = new Engine
                {
                    HorsePower = 666,
                    IsStarted = false,
                },
                Doors = new IDoor[]
    {
                    new Door { IsOpen = true },
                    new Door { IsOpen = true },
                    new Door { IsOpen = true },
                    new Door { IsOpen = true },
    },
                Wheels = new IWheel[]
    {
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 4.5},
                    new Wheel{Pressure = 4.5},
    },
            };

            yield return new Vehicle
            {
                Color = CarColor.Purple,
                Enrollment = new Enrollment
                {
                    Serial = "CSM",
                    Number = 1001,
                },
                Engine = new Engine
                {
                    HorsePower = 666,
                    IsStarted = false,
                },
                Doors = new IDoor[]
{
                    new Door { IsOpen = false },
                    new Door { IsOpen = true },
                    new Door { IsOpen = true },
                    new Door { IsOpen = false },
},
                Wheels = new IWheel[]
{
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 4.5},
                    new Wheel{Pressure = 4.5},
},
            };

            yield return new Vehicle
            {
                Color = CarColor.Purple,
                Enrollment = new Enrollment
                {
                    Serial = "CSM",
                    Number = 1002,
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
                    new Door { IsOpen = true },
                    new Door { IsOpen = false },
},
                Wheels = new IWheel[]
{
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 1},
                    new Wheel{Pressure = 4.5},
                    new Wheel{Pressure = 4.5},
},
            };

            yield return new Vehicle
            {
                Color = CarColor.Purple,
                Enrollment = new Enrollment
                {
                    Serial = "CSM",
                    Number = 1003,
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
                    new Door { IsOpen = true },
                    new Door { IsOpen = true },
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
