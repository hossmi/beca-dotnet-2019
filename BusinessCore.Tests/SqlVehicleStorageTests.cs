using System;
using System.Collections.Generic;
using System.Configuration;
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
            foreach (IVehicle vehicle in vehicles)
            {
                InsertVehicle(connectionString, vehicle);
            }
        }

        public class Insert
        {

        }
        public static void InsertVehicle(string connectionString, IVehicle vehicle)
        {
            string serial = vehicle.Enrollment.Serial.ToString();
            int number = vehicle.Enrollment.Number;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            String query;
            query = "USE [CarManagement]" +
                "INSERT INTO [enrollment] (serial, number) " +
                "VALUES ('" + serial + "', " + number + ")";
            SqlCommand sentence = new SqlCommand(query, con);
            sentence.ExecuteNonQuery();

            query = "SELECT id " +
                "FROM [enrollment]" +
                "WHERE (serial = '" + serial + "' AND number = " + number + ")";
            sentence = new SqlCommand(query, con);
            SqlDataReader reader = sentence.ExecuteReader();
            reader.Read();
            int ressult = (int)reader["id"];
            reader.Close();

            int color = (int)vehicle.Color;
            int engineIsStarted = vehicle.Engine.IsStarted ? 1 : 0;

            int engineHorsePower = vehicle.Engine.HorsePower;
            query = "INSERT INTO [vehicle] (enrollmentId, color, engineHorsePower, engineIsStarted) " +
                "VALUES (" + ressult + ", " + color + ", " + engineHorsePower + ", " + engineIsStarted + ")";
            sentence = new SqlCommand(query, con);
            sentence.ExecuteNonQuery();

            IWheel[] wheels = vehicle.Wheels;
            foreach (IWheel wheel in wheels)
            {
                double pressure = wheel.Pressure;
                query = "INSERT INTO wheel (vehicleId, pressure) " +
                    "VALUES (" + ressult + ", " + pressure + ")";
                sentence = new SqlCommand(query, con);
                sentence.ExecuteNonQuery();
            }

            IDoor[] doors = vehicle.Doors;
            foreach (IDoor door in doors)
            {
                string isOpen = (door.IsOpen ? 1 : 0).ToString();
                query = "INSERT INTO door (vehicleId, isOpen) " +
                    "VALUES (" + ressult + ", " + isOpen + ")";
                sentence = new SqlCommand(query, con);
                sentence.ExecuteNonQuery();

            }
            con.Close();
        }

        private static void create(string connectionString, string creationScript)
        {
            string FilePath = Path.Combine(Environment.CurrentDirectory, "Scripts", "database-creation.sql");
            string file = File.ReadAllText(FilePath);
            SqlConnection con = new SqlConnection(connectionString);
            ConMethod(file, con);

        }

        private static void drop(string connectionString, string destructionScript)
        {
            SqlConnection con = new SqlConnection(connectionString);
            string FilePath = Path.Combine(Environment.CurrentDirectory, "Scripts", "database-drop.sql");
            string file = File.ReadAllText(FilePath);
            SqlCommand sentence = new SqlCommand(file, con);
            ConMethod(file, con);
        }
        public string cosa = Environment.MachineName;
        private static void ConMethod(string file, SqlConnection con)
        {
            SqlCommand sentence = new SqlCommand(file, con);
            con.Open();
            int affectedRows = sentence.ExecuteNonQuery();
            con.Close();
        }
    }
}
