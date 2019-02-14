using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using BusinessCore.Tests.Models;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolBox.Models;
using ToolBox.Services;

namespace BusinessCore.Tests
{
    [TestCategory("Requires SQL Server connection")]
    [TestClass]
    public class SqlVehicleStorageTests
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

        public SqlVehicleStorageTests()
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
            fullfillWithSampleData(this.connectionString, this.fakeStorage.get());
        }

        [TestMethod]
        public void create_and_drop_works()
        {
        }

        [TestMethod]
        public void there_are_ten_vehicles_stored_at_database()
        {
            IVehicleStorage databaseVehicleStorage =
                new SqlVehicleStorage(this.connectionString, this.vehicleBuilder, this.enrollmentProvider);

            IVehicle[] vehicles = databaseVehicleStorage
                .get()
                .ToArray();

            Assert.AreEqual(10, vehicles.Length);
        }

        [TestMethod]
        public void clear_erases_vehicles_from_DB()
        {
            IVehicleStorage databaseVehicleStorage = 
                new SqlVehicleStorage(this.connectionString, this.vehicleBuilder, this.enrollmentProvider);

            databaseVehicleStorage.clear();

            IEnumerable<IVehicle> vehicles = databaseVehicleStorage.get();
            Assert.AreEqual(0, vehicles.Count());

            databaseVehicleStorage = 
                new SqlVehicleStorage(this.connectionString, this.vehicleBuilder, this.enrollmentProvider);
            vehicles = databaseVehicleStorage.get();
            Assert.AreEqual(0, vehicles.Count());
        }

        [TestMethod]
        public void save_vehicle_to_DB_and_retrieve_it()
        {
            IVehicleStorage databaseVehicleStorage = 
                new SqlVehicleStorage(this.connectionString, this.vehicleBuilder, this.enrollmentProvider);

            databaseVehicleStorage.clear();
            IEnumerable<IVehicle> vehicles = databaseVehicleStorage.get();
            Assert.AreEqual(0, vehicles.Count());

            IVehicle firstVehicle = new Vehicle
            {
                Enrollment = new Enrollment
                {
                    Serial = "AZD",
                    Number = 4444
                },
                Engine = new Engine
                {
                    IsStarted = true,
                    HorsePower = 1000,
                },
                Color = CarColor.Purple,
                Doors = new Door[] { new Door { IsOpen = false }, new Door { IsOpen = true } },
                Wheels = new Wheel[] { new Wheel { Pressure = 2.3 }, new Wheel { Pressure = 1.2 } },
            };

            databaseVehicleStorage.set(firstVehicle);

            databaseVehicleStorage = 
                new SqlVehicleStorage(this.connectionString, this.vehicleBuilder, this.enrollmentProvider);
            IVehicle retrievedVehicle = databaseVehicleStorage.get().First();

            Assert.AreEqual(firstVehicle.Enrollment.Serial, retrievedVehicle.Enrollment.Serial);
            Assert.AreEqual(firstVehicle.Enrollment.Number, retrievedVehicle.Enrollment.Number);
            Assert.AreEqual(firstVehicle.Color, retrievedVehicle.Color);
            Assert.AreEqual(firstVehicle.Engine.HorsePower, retrievedVehicle.Engine.HorsePower);
            Assert.AreEqual(firstVehicle.Doors[0].IsOpen, retrievedVehicle.Doors[0].IsOpen);
            Assert.AreEqual(firstVehicle.Doors[1].IsOpen, retrievedVehicle.Doors[1].IsOpen);
            Assert.AreEqual(firstVehicle.Wheels[0].Pressure, retrievedVehicle.Wheels[0].Pressure);
            Assert.AreEqual(firstVehicle.Wheels[1].Pressure, retrievedVehicle.Wheels[1].Pressure);
        }

        private static void fullfillWithSampleData(string connectionString, IEnumerable<IVehicle> vehicles)
        {
            IEnumerable<VehicleDto> dtoVehicles = vehicles.Select(exportVehicle);
            DB<SqlConnection> db = new DB<SqlConnection>(connectionString);

            foreach (VehicleDto vehicle in dtoVehicles)
            {
                db.transact(commandBuilder =>
                {
                    int affectedRows, enrollmentId;

                    affectedRows = commandBuilder
                        .setQuery("INSERT enrollment ( serial, number) VALUES (@serial, @number);")
                        .setParameter("@serial", vehicle.Enrollment.Serial)
                        .setParameter("@number", vehicle.Enrollment.Number)
                        .build()
                        .ExecuteNonQuery();

                    Assert.AreEqual(1, affectedRows);

                    enrollmentId = (int)commandBuilder
                        .setQuery("SELECT id FROM enrollment WHERE serial = @serial AND number = @number;")
                        .setParameter("@serial", vehicle.Enrollment.Serial)
                        .setParameter("@number", vehicle.Enrollment.Number)
                        .build()
                        .ExecuteScalar();

                    affectedRows = commandBuilder
                        .setQuery(@"INSERT INTO vehicle (enrollmentId, color, engineHorsePower, engineIsStarted)
                                    VALUES (@enrollmentId, @color, @engineHorsePower, @engineIsStarted);")
                        .setParameter("@enrollmentId", enrollmentId)
                        .setParameter("@color", vehicle.Color)
                        .setParameter("@engineHorsePower", vehicle.Engine.HorsePower)
                        .setParameter("@engineIsStarted", vehicle.Engine.IsStarted)
                        .build()
                        .ExecuteNonQuery();

                    Assert.AreEqual(1, affectedRows);

                    foreach (WheelDto wheel in vehicle.Wheels)
                    {
                        affectedRows = commandBuilder
                            .setQuery(@"INSERT INTO wheel (vehicleId, pressure) VALUES (@vehicleId, @pressure);")
                            .setParameter("@vehicleId", enrollmentId)
                            .setParameter("@pressure", wheel.Pressure)
                            .build()
                            .ExecuteNonQuery();

                        Assert.AreEqual(1, affectedRows);
                    }

                    foreach (DoorDto door in vehicle.Doors)
                    {
                        affectedRows = commandBuilder
                            .setQuery(@"INSERT INTO door (vehicleId, isOpen) VALUES (@vehicleId, @isOpen);")
                            .setParameter("@vehicleId", enrollmentId)
                            .setParameter("@isOpen", door.IsOpen)
                            .build()
                            .ExecuteNonQuery();

                        Assert.AreEqual(1, affectedRows);
                    }

                    return TransactionAction.Commit;
                });
            }
        }


        private static VehicleDto exportVehicle(IVehicle vehicle)
        {
            return new VehicleDto
            {
                Color = vehicle.Color,
                Enrollment = new EnrollmentDto
                {
                    Serial = vehicle.Enrollment.Serial,
                    Number = vehicle.Enrollment.Number,
                },
                Engine = new EngineDto
                {
                    HorsePower = vehicle.Engine.HorsePower,
                    IsStarted = vehicle.Engine.IsStarted,
                },
                Doors = vehicle.Doors.Select(door => new DoorDto
                {
                    IsOpen = door.IsOpen,
                }).ToArray(),
                Wheels = vehicle.Wheels.Select(w => new WheelDto
                {
                    Pressure = w.Pressure
                }).ToArray(),
            };
        }

        private static void create(string connectionString, string creationScript)
        {
            DB<SqlConnection> db = new DB<SqlConnection>(connectionString);
            string statements = File.ReadAllText(creationScript);
            db.write(statements);
        }

        private static void drop(string connectionString, string destructionScript)
        {
            DB<SqlConnection> db = new DB<SqlConnection>(connectionString);
            string statements = File.ReadAllText(destructionScript);
            db.write(statements);
        }

    }
}
