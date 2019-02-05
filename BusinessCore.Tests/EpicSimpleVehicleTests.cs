using CarManagement.Extensions.Vehicles;
using CarManagement.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarManagement.Services;
using BusinessCore.Tests.Services;
using BusinessCore.Tests.Models;

namespace BusinessCore.Tests
{
    [TestClass]
    public class EpicSimpleVehicleTests
    {
        private readonly EpicVehicleStorage vehicleStorage;

        public EpicSimpleVehicleTests()
        {
            this.vehicleStorage = new EpicVehicleStorage();
        }

        [TestMethod]
        public void Vehicle_entities_shall_not_share_wheels()
        {
            DefaultEnrollmentProvider provider = new DefaultEnrollmentProvider();
            VehicleBuilder builder = new VehicleBuilder(provider);

            builder.addWheel();
            builder.setDoors(3);
            builder.setEngine(12);

            IVehicle vehicle0 = builder.build();
            IVehicle vehicle1 = builder.build();

            vehicle0.setWheelsPressure(3.2);
            vehicle1.setWheelsPressure(1.1);

            Assert.IsFalse(vehicle0.Wheels[0].Pressure == vehicle1.Wheels[0].Pressure);
        }

        [TestMethod]
        public void Find_vehicle_enrollmment_with_most_powerful_engine_and_2_wheels()
        {
            IEnrollment querriedEnrollment = this.vehicleStorage
                .getAll()
                /**/;

            Assert.AreEqual(new Enrollment() { Serial = "ABC", Number = 0001 }, querriedEnrollment);
        }

        [TestMethod]
        public void Find_vehicle_enrollment_with_one_wheel_with_more_pressure_than_the_others()
        {
            IEnrollment querriedEnrollment = this.vehicleStorage
                .getAll()
                /**/;

            Assert.AreEqual(new Enrollment() { Serial = "AAA", Number = 1000 }, querriedEnrollment);
        }
    }
}
