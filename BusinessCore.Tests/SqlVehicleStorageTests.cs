using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using CarManagement.Core;
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
            #region "skeletal inserts"
            string insertEnrollmentSkeleton = "INSERT INTO enrollment (serial, number) output INSERTED.ID VALUES (@serial, @number)";
            string insertVehicleSkeleton = "INSERT INTO vehicle (enrollmentId, color, engineHorsePower, engineIsStarted) VALUES (@enrollmentId, @color, @engineHorsePower, @engineIsStarted)";
            string insertDoorSkeleton = "INSERT INTO door (vehicleId, isOpen) VALUES (@vehicleId, @isOpen)";
            string insertWheelSkeleton = "INSERT INTO wheel (vehicleId, pressure) VALUES (@vehicleId, @pressure)";
            #endregion

            using (SqlConnection sqlDbConnection = new SqlConnection(connectionString))
            {
                sqlDbConnection.Open();
                foreach (IVehicle vehicle in vehicles)
                {
                    #region "Enrollment"
                    SqlCommand inserter = new SqlCommand(insertEnrollmentSkeleton, sqlDbConnection);
                    inserter.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                    inserter.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                    int databaseEnrollmentId = (int)inserter.ExecuteScalar();
                    #endregion
                    #region "Vehicle"
                    inserter = new SqlCommand(insertVehicleSkeleton, sqlDbConnection);
                    inserter.Parameters.AddWithValue("@enrollmentId", databaseEnrollmentId);
                    inserter.Parameters.AddWithValue("@color", vehicle.Color);
                    inserter.Parameters.AddWithValue("@engineHorsePower", vehicle.Engine.HorsePower);
                    inserter.Parameters.AddWithValue("@engineIsStarted", vehicle.Engine.IsStarted);
                    inserter.ExecuteNonQuery();
                    #endregion
                    #region "Doors"
                    foreach (IDoor door in vehicle.Doors)
                    {
                        inserter = new SqlCommand(insertDoorSkeleton, sqlDbConnection);
                        inserter.Parameters.AddWithValue("@vehicleId", databaseEnrollmentId);
                        inserter.Parameters.AddWithValue("@isOpen", door.IsOpen);
                        inserter.ExecuteNonQuery();
                    }
                    #endregion
                    #region "Wheels"
                    foreach (IWheel wheel in vehicle.Wheels)
                    {
                        inserter = new SqlCommand(insertWheelSkeleton, sqlDbConnection);
                        inserter.Parameters.AddWithValue("@vehicleId", databaseEnrollmentId);
                        inserter.Parameters.AddWithValue("@pressure", wheel.Pressure);
                        inserter.ExecuteNonQuery();
                    }
                    #endregion
                }
                sqlDbConnection.Close();
            }
        }

        private static void create(string connectionString, string creationScript)
        {
            executeDbCommand(connectionString, File.ReadAllText(creationScript));
        }

        private static void drop(string connectionString, string destructionScript)
        {
            executeDbCommand(connectionString, File.ReadAllText(destructionScript));
        }

        private static void executeDbCommand(string connectionString, string command)
        {
            using (SqlConnection sqlDbConnection = new SqlConnection(connectionString))
            {
                sqlDbConnection.Open();

                SqlCommand actualCommand = new SqlCommand(command, sqlDbConnection);
                actualCommand.ExecuteNonQuery();
                sqlDbConnection.Close();
            }
        }
    }
}
