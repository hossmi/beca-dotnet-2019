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

        private const string INSERT_ENROLLMENT =    "INSERT INTO [enrollment] (serial,number) " +
            "OUTPUT INSERTED.ID " +
            "VALUES (@serial, @number)";
        private const string INSERT_VEHICLE =   "INSERT INTO [vehicle] (enrollmentId, color, engineHorsePower, engineIsStarted) " +
            "VALUES (@enrollmentKEY, @color, @horsepower, @started)";
        private const string INSERT_WHEEL = "INSERT INTO [wheel] (pressure,vehicleId) " +
            "VALUES (@pressure, @enrollmentKEY)";
        private const string INSERT_DOOR =  "INSERT INTO [door] (isOpen, vehicleId) " +
            "VALUES (@open, @enrollmentKEY)";

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
            fullfillWithSampleData(this.connectionString, this.fakeStorage.getAll());
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
                .getAll()
                .ToArray();

            Assert.AreEqual(10, vehicles.Length);
        }

        [TestMethod]
        public void clear_erases_vehicles_from_DB()
        {
            IVehicleStorage databaseVehicleStorage = new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);

            databaseVehicleStorage.clear();

            IEnumerable<IVehicle> vehicles = databaseVehicleStorage.getAll();
            Assert.AreEqual(0, vehicles.Count());

            databaseVehicleStorage = new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);
            vehicles = databaseVehicleStorage.getAll();
            Assert.AreEqual(0, vehicles.Count());
        }

        [TestMethod]
        public void save_vehicle_to_DB_and_retrieve_it()
        {
            IVehicleStorage databaseVehicleStorage = new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);

            databaseVehicleStorage.clear();
            IEnumerable<IVehicle> vehicles = databaseVehicleStorage.getAll();
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
                },
                Color = CarColor.Purple,
                Doors = new Door[] { new Door{ IsOpen = false }, new Door { IsOpen = true }},
                Wheels = new Wheel[] { new Wheel { Pressure = 2.3 }, new Wheel { Pressure = 1.2 }},
            };

            databaseVehicleStorage.set(firstVehicle);

            databaseVehicleStorage = new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);
            IVehicle retrievedVehicle = databaseVehicleStorage.getAll().First();

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
            int insertedVehicles = 0;
            int insertedWheels = 0;
            int insertedDoors = 0;

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            foreach (IVehicle vehicle in vehicles)
            {
                SqlCommand sqlCommand = new SqlCommand(INSERT_ENROLLMENT, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                sqlCommand.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                string enrollmentKEY = sqlCommand.ExecuteScalar().ToString();

                sqlCommand = new SqlCommand(INSERT_VEHICLE, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@enrollmentKEY", enrollmentKEY);
                sqlCommand.Parameters.AddWithValue("@color", ((int)vehicle.Color));
                sqlCommand.Parameters.AddWithValue("@horsepower", vehicle.Engine.HorsePower);
                sqlCommand.Parameters.AddWithValue("@started", Convert.ToInt32(vehicle.Engine.IsStarted));
                insertedVehicles = insertedVehicles + sqlCommand.ExecuteNonQuery();
                
                foreach (IWheel wheel in vehicle.Wheels)
                {
                    sqlCommand = new SqlCommand(INSERT_WHEEL, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@pressure", wheel.Pressure);
                    sqlCommand.Parameters.AddWithValue("@enrollmentKEY", enrollmentKEY);
                    insertedWheels = insertedWheels + sqlCommand.ExecuteNonQuery();
                }

                foreach (IDoor door in vehicle.Doors)
                {
                    sqlCommand = new SqlCommand(INSERT_DOOR, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@open", Convert.ToInt32(door.IsOpen));
                    sqlCommand.Parameters.AddWithValue("@enrollmentKEY", enrollmentKEY);
                    insertedDoors = insertedDoors + sqlCommand.ExecuteNonQuery();
                }
            }
            sqlConnection.Close();
        }

        private static void create(string connectionString, string creationScript)
        {
            string script = File.ReadAllText(creationScript);
            executeNonQueryCommand(connectionString, script);
        }

        private static void drop(string connectionString, string destructionScript)
        {
            string script = File.ReadAllText(destructionScript);
            executeNonQueryCommand(connectionString, script);
        }

        private static void executeNonQueryCommand(string connectionString, string script)
        {
            using (IDbConnection sqlConnection = new SqlConnection(connectionString))
            using (IDbCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = script;
                sqlCommand.Connection = sqlConnection;

                sqlConnection.Open();
                int affectedRows = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
