using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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

        private const string INSERT_ENROLLMENT = "INSERT INTO [enrollment] (serial,number) " +
                    "OUTPUT INSERTED.ID " +
                    "VALUES ('@serial', @number)";
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

        public SqlVehicleStorageTests()
        {
            this.connectionString = ConfigurationManager.AppSettings[CONNECTION_STRING_KEY].ToString();

            string baseFolder = ConfigurationManager.AppSettings[SCRIPTS_FOLDER_KEY].ToString();
            baseFolder = Path.Combine(Environment.CurrentDirectory, baseFolder);

            this.creationScript = ConfigurationManager.AppSettings[CREATION_SCRIPT_FILE_KEY].ToString();
            this.creationScript = Path.Combine(baseFolder, this.creationScript);

            this.destructionScript = ConfigurationManager.AppSettings[DESTRUCTION_SCRIPT_FILE_KEY].ToString();
            this.destructionScript = Path.Combine(baseFolder, this.destructionScript);

            this.fakeStorage = new ArrayVehicleStorage();
        }

        [TestInitialize]
        public void recreate()
        {
            drop(this.connectionString, this.destructionScript);
            create(this.connectionString, this.creationScript);
            fullfillWithSampleData(this.connectionString, this.fakeStorage.getAll());
        }

        [TestCleanup]
        public void cleanUp()
        {
            drop(this.connectionString, this.destructionScript);
        }

        [TestMethod]
        public void create_and_drop_works()
        {
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
                sqlCommand.Parameters.AddWithValue("@serial", "'"+vehicle.Enrollment.Serial+"'");
                sqlCommand.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                string enrollmentKEY = sqlCommand.ExecuteScalar().ToString();

                sqlCommand = new SqlCommand(INSERT_VEHICLE, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@enrollmentKEY", enrollmentKEY);
                sqlCommand.Parameters.AddWithValue("@color", ((int)vehicle.Color).ToString());
                sqlCommand.Parameters.AddWithValue("@horsepower", vehicle.Engine.HorsePower.ToString());
                sqlCommand.Parameters.AddWithValue("@started", Convert.ToInt32(vehicle.Engine.IsStarted).ToString());
                insertedVehicles = sqlCommand.ExecuteNonQuery();
                
                foreach (IWheel wheel in vehicle.Wheels)
                {
                    sqlCommand = new SqlCommand(INSERT_WHEEL, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@pressure", wheel.Pressure.ToString());
                    sqlCommand.Parameters.AddWithValue("@enrollmentKEY", enrollmentKEY);
                    insertedWheels = insertedWheels + sqlCommand.ExecuteNonQuery();
                }

                foreach (IDoor door in vehicle.Doors)
                {
                    sqlCommand = new SqlCommand(INSERT_DOOR, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@open", Convert.ToInt32(door.IsOpen).ToString());
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
