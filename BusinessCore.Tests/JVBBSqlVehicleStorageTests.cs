using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using BusinessCore.Tests.Services;
using CarManagement.Core.Models;
using CarManagement.Core.Services;
using CarManagement.Extensions.Filters;
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

        private const string INSERT_ENROLLMENT = "INSERT INTO [enrollment] (serial,number) " +
            "OUTPUT INSERTED.ID " +
            "VALUES (@serial, @number)";
        private const string INSERT_VEHICLE = "INSERT INTO [vehicle] (enrollmentId, color, engineHorsePower, engineIsStarted) " +
            "VALUES (@enrollmentKEY, @color, @horsepower, @started)";
        private const string INSERT_WHEEL = "INSERT INTO [wheel] (pressure,vehicleId) " +
            "VALUES (@pressure, @enrollmentKEY)";
        private const string INSERT_DOOR = "INSERT INTO [door] (isOpen, vehicleId) " +
            "VALUES (@open, @enrollmentKEY)";

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
            fullfillWithSampleData(this.connectionString, this.fakeStorage.get());
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
