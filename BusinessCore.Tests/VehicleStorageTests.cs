using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using CarManagement.Extensions.Filters;
using CarManagement.Services;
using CarManagement.Services.CarManagement.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestClass]
    public class VehicleStorageTests
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
        public void fileVehicleStorage_must_persists_vehicles()
        {
            SingleEnrollmentProvider enrollmentProvider = new SingleEnrollmentProvider();
            IEqualityComparer<IEnrollment> equalityComparer = new EnrollmentEqualityComparer();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);


            using (IVehicleStorage vehicleStorage =
                new FileVehicleStorage(this.VehiclesFilePath, vehicleBuilder))
            {
                vehicleStorage.clear();
                Assert.AreEqual(0, vehicleStorage.Count);
            }

            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);
            IVehicle motoVehicle = vehicleBuilder.build();

            using (IVehicleStorage vehicleStorage =
                new FileVehicleStorage(this.VehiclesFilePath, vehicleBuilder))
            {
                vehicleStorage.set(motoVehicle);
                Assert.AreEqual(1, vehicleStorage.Count);
            }

            using (IVehicleStorage vehicleStorage =
                new FileVehicleStorage(this.VehiclesFilePath, vehicleBuilder))
            {
                Assert.AreEqual(1, vehicleStorage.Count);

                IVehicle vehicle = vehicleStorage.get(enrollmentProvider.DefaultEnrollment);
                Assert.IsNotNull(vehicle);
                Assert.IsTrue(equalityComparer.Equals(enrollmentProvider.DefaultEnrollment, vehicle.Enrollment));
            }
        }

        [TestMethod]
        public void inMemoryVehicleStorage_must_be_empty_on_each_instance()
        {
            IVehicle vehicle, motoVehicle;

            SingleEnrollmentProvider enrollmentProvider = new SingleEnrollmentProvider();
            IEqualityComparer<IEnrollment> equalityComparer = new EnrollmentEqualityComparer();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IVehicleStorage vehicleStorage = new InMemoryVehicleStorage();

            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);
            motoVehicle = vehicleBuilder.build();

            Assert.AreEqual(0, vehicleStorage.Count);
            vehicleStorage.set(motoVehicle);
            Assert.AreEqual(1, vehicleStorage.Count);

            vehicle = vehicleStorage.get(enrollmentProvider.DefaultEnrollment);
            Assert.IsNotNull(vehicle);
            Assert.IsTrue(equalityComparer.Equals(enrollmentProvider.DefaultEnrollment, vehicle.Enrollment));

            vehicleStorage = new InMemoryVehicleStorage();
            Assert.AreEqual(0, vehicleStorage.Count);

            Negassert.mustFail(() =>
            {
                vehicle = vehicleStorage.get(enrollmentProvider.DefaultEnrollment);
            });
        }

        [TestMethod]
        public void vehicleStoarge_implementations_must_return_6_items()
        {
            ArrayEnrollmentProvider enrollmentProvider = new ArrayEnrollmentProvider();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);

            IVehicleStorage[] vehicleStorages = new IVehicleStorage[]
            {
                new InMemoryVehicleStorage(),
                new FileVehicleStorage(this.VehiclesFilePath, vehicleBuilder),
            };

            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setColor(CarColor.Blue);
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);

            for (int i = 0; i < enrollmentProvider.Count; i++)
            {
                IVehicle vehicle = vehicleBuilder.build();
                foreach (IVehicleStorage vehicleStorage in vehicleStorages)
                    vehicleStorage.set(vehicle);
            }

            foreach (IVehicleStorage vehicleStorage in vehicleStorages)
            {
                IEnumerable<IVehicle> vehicles = vehicleStorage.get();
                Assert.AreEqual(enrollmentProvider.Count, vehicles.Count());
            }

            foreach (IVehicleStorage vehicleStorage in vehicleStorages)
                vehicleStorage.Dispose();

        }

        [TestMethod]
        public void filtering_vehicle_items()
        {
            ArrayEnrollmentProvider enrollmentProvider = new ArrayEnrollmentProvider();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);

            vehicleBuilder.addWheel();
            vehicleBuilder.addWheel();
            vehicleBuilder.setColor(CarColor.Blue);
            vehicleBuilder.setDoors(0);
            vehicleBuilder.setEngine(40);

            using (IVehicleStorage vehicleStorage = new InMemoryVehicleStorage())
            {
                for (int i = 0; i < enrollmentProvider.Count; i++)
                {
                    IVehicle vehicle = vehicleBuilder.build();

                    if (i % 3 == 0)
                        vehicle.Engine.start();

                    vehicleStorage.set(vehicle);
                }

                Func<IVehicle, bool> byOddEnrollment = vehicle => vehicle.Enrollment.Number % 2 == 0;

                IEnumerable<IEngine> selectedEngines = vehicleStorage
                    .get()
                    .filter(byOddEnrollment)          //4
                    .filter(vehicle => vehicle.Enrollment.Serial == "BBC")   //2
                    .select(vehicle => vehicle.Engine)                    //2
                    .filter(engine => engine.IsStarted);         //1

                Assert.AreEqual(1, selectedEngines.Count());
            }
        }
    }
}
