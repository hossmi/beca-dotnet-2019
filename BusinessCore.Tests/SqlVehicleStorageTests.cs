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
