using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestCategory("JVBB SQL Tests")]
    [TestClass]
    public class JVBBSqlVehicleStorageTests
    {
        private const string CONNECTION_STRING_KEY = "CarManagerConnectionString";
        private const string CREATION_SCRIPT_FILE_KEY = "CreationScriptFile";
        private const string DESTRUCTION_SCRIPT_FILE_KEY = "DestructionScriptFile";
        private const string SCRIPTS_FOLDER_KEY = "scriptsFolder";

        private readonly string connectionString;
        private readonly string creationScript;
        private readonly string destructionScript;
        private readonly IVehicleStorage fakeStorage;
        private readonly IVehicleBuilder vehicleBuilder;
        private readonly IEnrollmentProvider enrollmentProvider;

        public JVBBSqlVehicleStorageTests()
        {
            this.connectionString = ConfigurationManager.AppSettings[CONNECTION_STRING_KEY];

            string scripsFolder = ConfigurationManager.AppSettings[SCRIPTS_FOLDER_KEY];

            this.creationScript = ConfigurationManager.AppSettings[CREATION_SCRIPT_FILE_KEY];
            this.creationScript = Path.Combine(Environment.CurrentDirectory, scripsFolder, this.creationScript);

            this.destructionScript = ConfigurationManager.AppSettings[DESTRUCTION_SCRIPT_FILE_KEY];
            this.destructionScript = Path.Combine(Environment.CurrentDirectory, scripsFolder, this.destructionScript);

            this.fakeStorage = new ArrayVehicleStorage();
            this.enrollmentProvider = new DefaultEnrollmentProvider();
            this.vehicleBuilder = new VehicleBuilder(this.enrollmentProvider);
        }

        [TestInitialize]
        public void recreate()
        {
            drop(this.connectionString, this.destructionScript);
            create(this.connectionString, this.creationScript);
            fullfillWithSampleData(this.connectionString, this.fakeStorage.getAll());
        }

        [TestMethod]
        public void insert_a_new_vehicle_and_update_it()
        {
            IVehicleStorage databaseVehicleStorage =
                new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);

            this.vehicleBuilder.addWheel();
            this.vehicleBuilder.addWheel();
            this.vehicleBuilder.addWheel();
            this.vehicleBuilder.addWheel();

            this.vehicleBuilder.setColor(CarColor.Yellow);
            this.vehicleBuilder.setDoors(5);
            this.vehicleBuilder.setEngine(700);

            IVehicle vehicle = this.vehicleBuilder.build();
            IEnrollment enrollment = vehicle.Enrollment;

            foreach (IWheel wheel in vehicle.Wheels)
            {
                wheel.Pressure = 3.0;
            }

            Assert.IsNotNull(vehicle);

            databaseVehicleStorage.set(vehicle);

            Assert.AreEqual(11, databaseVehicleStorage.Count);

            vehicle.Doors[1].open();
            vehicle.Doors[3].open();
            vehicle.Engine.start();
            vehicle.Wheels[3].Pressure = 2.0;

            databaseVehicleStorage.set(vehicle);

            vehicle = databaseVehicleStorage.get(enrollment);

            Assert.IsNotNull(vehicle);

            Assert.IsTrue(vehicle.Doors[1].IsOpen);
            Assert.IsTrue(vehicle.Doors[3].IsOpen);
            Assert.IsTrue(vehicle.Engine.IsStarted);
            Assert.AreEqual(2.0, vehicle.Wheels[3].Pressure);

        }

        [TestMethod]
        public void there_are_ten_vehicles_stored_at_database_using_Count()
        {
            IVehicleStorage databaseVehicleStorage = 
                new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);

            Assert.AreEqual(10, databaseVehicleStorage.Count);
        }

        [TestMethod]
        public void there_exists_a_vehicle_with_ZZZ_serial_and_2010_number_as_enrollment()
        {
            IVehicleStorage databaseVehicleStorage =
                new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);
            IEnrollment enrollment = this.enrollmentProvider.import("ZZZ", 2100);
            IVehicle vehicle = databaseVehicleStorage.get(enrollment);

            Assert.IsNotNull(vehicle);
        }

        private static void fullfillWithSampleData(string connectionString, IEnumerable<IVehicle> vehicles)
        {
            throw new NotImplementedException();
        }

        private static void create(string connectionString, string creationScript)
        {
            throw new NotImplementedException();
        }

        private static void drop(string connectionString, string destructionScript)
        {
            throw new NotImplementedException();
        }

    }
}
