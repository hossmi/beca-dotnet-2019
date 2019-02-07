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
            throw new NotImplementedException();
        }

        private static void create(string connectionString, string creationScript)
        {
            using (IDbConnection conection = new SqlConnection(connectionString))
            {
                using (IDbCommand command = new SqlCommand())
                {

                    string ruta = Path.Combine(Environment.CurrentDirectory, "Scripts", "database-creation.sql");
                    string sentences = File.ReadAllText(ruta);

                    executeCommand(conection, command, sentences);
                }
            }

        }

        private static void drop(string connectionString, string destructionScript)
        {
            using (IDbConnection conection = new SqlConnection(connectionString))
            {

                using (IDbCommand command = new SqlCommand())
                {

                    string ruta = Path.Combine(Environment.CurrentDirectory, "Scripts", "database-drop.sql");
                    string sentences = File.ReadAllText(ruta);

                    executeCommand(conection, command, sentences);
                }
            }

        }

        private static void executeCommand(IDbConnection conection, IDbCommand command, string sentences)
        {
            
            command.CommandText = sentences;
            command.Connection = conection;

            conection.Open();
            command.ExecuteNonQuery();
            conection.Close();
        }

    }
}
