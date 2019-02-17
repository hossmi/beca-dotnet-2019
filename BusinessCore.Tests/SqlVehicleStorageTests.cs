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
using CarManagement.Core.Services;
using CarManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);

            IVehicle[] vehicles = databaseVehicleStorage
                .get()
                .ToArray();

            Assert.AreEqual(10, vehicles.Length);
        }

        [TestMethod]
        public void clear_erases_vehicles_from_DB()
        {
            IVehicleStorage databaseVehicleStorage = new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);

            databaseVehicleStorage.clear();

            IEnumerable<IVehicle> vehicles = databaseVehicleStorage.get();
            Assert.AreEqual(0, vehicles.Count());

            databaseVehicleStorage = new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);
            vehicles = databaseVehicleStorage.get();
            Assert.AreEqual(0, vehicles.Count());
        }

        [TestMethod]
        public void save_vehicle_to_DB_and_retrieve_it()
        {
            IVehicleStorage databaseVehicleStorage = new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);

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
                Doors = new Door[] { new Door{ IsOpen = false }, new Door { IsOpen = true }},
                Wheels = new Wheel[] { new Wheel { Pressure = 2.3 }, new Wheel { Pressure = 1.2 }},
            };

            databaseVehicleStorage.set(firstVehicle);

            databaseVehicleStorage = new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);
            IVehicle retrievedVehicle = databaseVehicleStorage.get().First();

            Assert.AreEqual(firstVehicle.Enrollment.Serial , retrievedVehicle.Enrollment.Serial);
            Assert.AreEqual(firstVehicle.Enrollment.Number , retrievedVehicle.Enrollment.Number);
            Assert.AreEqual(firstVehicle.Color, retrievedVehicle.Color);
            Assert.AreEqual(firstVehicle.Engine.HorsePower, retrievedVehicle.Engine.HorsePower);
            Assert.AreEqual(firstVehicle.Doors[0].IsOpen, retrievedVehicle.Doors[0].IsOpen);
            Assert.AreEqual(firstVehicle.Doors[1].IsOpen, retrievedVehicle.Doors[1].IsOpen);
            Assert.AreEqual(firstVehicle.Wheels[0].Pressure, retrievedVehicle.Wheels[0].Pressure);
            Assert.AreEqual(firstVehicle.Wheels[1].Pressure, retrievedVehicle.Wheels[1].Pressure);
        }

        private static void fullfillWithSampleData(string connectionString, IEnumerable<IVehicle> vehicles)
        {
            string sentencies = "";
            string queryEnrollmentId = "";

            foreach (IVehicle vehicle in vehicles)
            {
                sentencies = "INSERT INTO enrollment (serial, number) " +
                    "VALUES ('" + vehicle.Enrollment.Serial + "', " + vehicle.Enrollment.Number + ");";
                executeCommand(connectionString, sentencies);

                queryEnrollmentId = $@"
                                SELECT id 
                                FROM enrollment 
                                WHERE serial = '{vehicle.Enrollment.Serial}' 
                                AND number = {vehicle.Enrollment.Number};";

                int enrollmentId = executeScalarQuery(connectionString, queryEnrollmentId);

                sentencies = "INSERT INTO vehicle (enrollmentId, color, engineHorsePower, engineIsStarted) " +
                    "VALUES ('" + enrollmentId + "', " + (int)vehicle.Color + ", " + vehicle.Engine.HorsePower + ", " + (vehicle.Engine.IsStarted ? 1 : 0) + ");";
                executeCommand(connectionString, sentencies);

                foreach (IWheel wheel in vehicle.Wheels)
                {
                    sentencies = "INSERT INTO wheel (pressure, vehicleId) " +
                        "VALUES (" + wheel.Pressure + ", " + enrollmentId + ");";
                    executeCommand(connectionString, sentencies);
                }

                foreach (IDoor door in vehicle.Doors)
                {
                    sentencies = "INSERT INTO door (isOpen, vehicleId) " +
                        "VALUES (" + (door.IsOpen ? 1 : 0) + ", " + enrollmentId + ");";
                    executeCommand(connectionString, sentencies);
                }
            }
        }

        private static void create(string connectionString, string creationScript)
        {
            string filePath = creationScript;
            string sentencies = File.ReadAllText(filePath);
            executeCommand(connectionString, sentencies);
        }

        private static void drop(string connectionString, string destructionScript)
        {
            string filePath = destructionScript;
            string sentencies = File.ReadAllText(filePath);
            executeCommand(connectionString, sentencies);
        }

        private static void executeCommand(string connectionString, string sentencies)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            using (IDbCommand command = new SqlCommand())
            {
                command.CommandText = sentencies;
                command.Connection = connection;

                connection.Open();
                int afectedRows = command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static int executeScalarQuery(string connectionString, string query)
        {
            int result;

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);

            result = (int)command.ExecuteScalar();

            connection.Close();

            return result;
        }
    }
}
