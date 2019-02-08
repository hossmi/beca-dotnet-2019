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
            String pushToEnrollment = "INSERT INTO enrollment(serial,number)"+ 
               " output INSERTED.ID VALUES (@serial, @number)";
            String pushToVehicle = "INSERT INTO vehicle(enrollmentid,color,engineHorsePower,engineIsStarted)"+"VALUES(@enrollmentid,@color,@engineHorsePower,@engineIsStarted)";
            String pushToWheel = "INSERT INTO wheel(vehicleid,pressure)"+"VALUES(@vehicleid,pressure)";
            String pushToDoor = "INSERT INTO door(vehicleid,isopen)"+"VALUES(@vehicleid,@isopen)";

            SqlConnection conection = new SqlConnection(connectionString);
            SqlCommand pusher;
            conection.Open();
            foreach (IVehicle vehicle in vehicles)
            {
                pusher = new SqlCommand(pushToEnrollment, conection);
                pusher.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                pusher.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                int enrollmentId = (int)pusher.ExecuteScalar();
               

                pusher = new SqlCommand(pushToVehicle, conection);
                pusher.Parameters.AddWithValue("@enrollmentid", enrollmentId);
                pusher.Parameters.AddWithValue("@color", vehicle.Color);
                pusher.Parameters.AddWithValue("@engineHorsePower", vehicle.Engine.HorsePower);
                pusher.Parameters.AddWithValue("@engineIsStarted", vehicle.Engine.IsStarted);
                pusher.ExecuteNonQuery();


                foreach (IWheel wheels in vehicles)
                {
                    pusher = new SqlCommand(pushToWheel, conection);
                    pusher.Parameters.AddWithValue("@pressure", wheels.Pressure);
                    pusher.Parameters.AddWithValue("@vehiculoid", enrollmentId);
                    pusher.ExecuteNonQuery();

                }
                foreach (IDoor doors in vehicles)
                {
                    pusher = new SqlCommand(pushToDoor, conection);
                    pusher.Parameters.AddWithValue("@pressure", doors.IsOpen);
                    pusher.Parameters.AddWithValue("@vehiculoid", enrollmentId);
                    pusher.ExecuteNonQuery();
                }
               

            }
            conection.Close();
            

        }

        private static void create(string connectionString, string creationScript)
        {
           executeCommand(connectionString, File.ReadAllText(creationScript));

        }

        private static void drop(string connectionString, string destructionScript)
        {
          executeCommand(connectionString, File.ReadAllText(destructionScript));

        }

        private static void executeCommand(string connectionString, string command)
        {

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand sqlCommand = new SqlCommand(command, sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }

        }

    }
}
