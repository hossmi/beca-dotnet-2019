using System;
using System.Collections.Generic;
using System.IO;
using CarManagement.Services;
using CarManagement.Extensions.Vehicles;
using CarManagement.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarManagement.Core.Services;

namespace BusinessCore.Tests
{
    [TestClass]
    [TestCategory("JVBB Tests")]
    public class JVBBTests
    {
        public string VehiclesFilePath
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, "Vehicles.xml");
            }
        }

        [TestInitialize]
        [TestCleanup]
        public void initialize()
        {
            if (File.Exists(this.VehiclesFilePath))
                File.Delete(this.VehiclesFilePath);
        }

        [TestMethod]
        public void WrongBuildRequests_01()
        {
            SingleEnrollmentProvider enrollmentProvider = new SingleEnrollmentProvider();
            IVehicleBuilder builder = new VehicleBuilder(enrollmentProvider);

            Negassert.mustFail(() => builder.removeWheel());
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();
            builder.addWheel();

            Negassert.mustFail(() => builder.setDoors(-27));
            builder.setDoors(5);
            Negassert.mustFail(() => builder.setEngine(-40));
            Negassert.mustFail(() => builder.setEngine(0));
            builder.setEngine(100);
            Negassert.mustFail(() => builder.setColor((CarColor)27));

            IVehicle vehicle = builder.build();

            Negassert.mustFail(() => vehicle.Engine.stop());

            vehicle.Engine.start();
            Negassert.mustFail(() => vehicle.Engine.start());

            Negassert.mustFail(() => vehicle.setWheelsPressure(-1));
            Negassert.mustFail(() => vehicle.setWheelsPressure(0));
            Negassert.mustFail(() => vehicle.setWheelsPressure(0.99));
            Negassert.mustFail(() => vehicle.setWheelsPressure(5.01));
            vehicle.setWheelsPressure(2);

        }

        [TestMethod]
        public void StorageComparison()
        {
            IVehicle motoVehicle;

            SingleEnrollmentProvider enrollmentProvider = new SingleEnrollmentProvider();
            IEqualityComparer<IEnrollment> equalityComparer = new EnrollmentEqualityComparer();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            //IDtoConverter dtoConverter = new DefaultDtoConverter(enrollmentProvider);
            IVehicleStorage vehicleFileStorage = new FileVehicleStorage(this.VehiclesFilePath, vehicleBuilder);
            IVehicleStorage vehicleMemoryStorage = new InMemoryVehicleStorage();


            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);
            motoVehicle = vehicleBuilder.build();

            vehicleFileStorage.set(motoVehicle);
            vehicleMemoryStorage.set(motoVehicle);

            vehicleFileStorage = new FileVehicleStorage(this.VehiclesFilePath, vehicleBuilder);

            IVehicle memoryVehicleA = vehicleMemoryStorage.get(enrollmentProvider.DefaultEnrollment);
            IVehicle fileVehicle = vehicleFileStorage.get(enrollmentProvider.DefaultEnrollment);

            Assert.IsNotNull(memoryVehicleA);
            Assert.IsNotNull(vehicleFileStorage);

            Assert.IsTrue(SameVehicle(memoryVehicleA, fileVehicle, equalityComparer));
        }

        private static bool SameVehicle(IVehicle v1, IVehicle v2, IEqualityComparer<IEnrollment> enrollmentComparer)
        {

            if (v1.Color != v2.Color)
                return false;

            if (v1.Wheels.Length != v2.Wheels.Length)
                return false;

            for (int i = 0; i < v1.Wheels.Length; i++)
                if (v1.Wheels[i].Pressure != v2.Wheels[i].Pressure)
                    return false;

            if (v1.Doors.Length != v2.Doors.Length)
                return false;

            for (int i = 0; i < v1.Doors.Length; i++)
                if (v1.Doors[i].IsOpen != v2.Doors[i].IsOpen)
                    return false;

            if (v1.Engine.HorsePower != v2.Engine.HorsePower)
                return false;

            if (v1.Engine.IsStarted != v2.Engine.IsStarted)
                return false;

            if (enrollmentComparer.Equals(v1.Enrollment, v2.Enrollment) == false)
                return false;

            return true;
        }
    }
}
