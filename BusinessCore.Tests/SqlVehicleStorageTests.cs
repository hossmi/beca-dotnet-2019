using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace BusinessCore.Tests
{
    [TestCategory("Requires SQL Server connection")]
    [TestClass]
    public class SqlVehicleStorageTests
    {

        private const string ConnectionStringKey = "CarManagerConnectionString";

        private readonly string connectionString;

        public SqlVehicleStorageTests()
        {
            Assert.IsTrue(ConfigurationManager.AppSettings.AllKeys.Contains(ConnectionStringKey));
            this.connectionString = ConfigurationManager.AppSettings[ConnectionStringKey].ToString();
        }

        [TestInitialize]
        public void recreate()
        {
            drop(this.connectionString);
            create(this.connectionString);
        }

        [TestCleanup]
        public void cleanUp()
        {
            drop(this.connectionString);
        }

        [TestMethod]
        public void create_and_drop_works()
        {

        }

        private static void create(string connectionString)        {
           
            FileInfo file = new FileInfo(@"C:\Repositorio_Pablo\BusinessCore.Tests\Scripts\database-creation.sql");
            string script = file.OpenText().ReadToEnd();

            SqlCommand comand = new SqlCommand(script);
            SqlConnection connection = new SqlConnection();

            connection.ConnectionString = connectionString;
            connection.Open();
            comand.ExecuteNonQuery();
            connection.Close();
        }

        private static void drop(string connectionString)
        {

            FileInfo file = new FileInfo(@"C:\Repositorio_Pablo\BusinessCore.Tests\Scripts\database-drop.sql");
            string script = file.OpenText().ReadToEnd();

            SqlCommand comand = new SqlCommand(script);
            SqlConnection connection = new SqlConnection();

            connection.ConnectionString = connectionString;
            connection.Open();
            comand.ExecuteNonQuery();
            connection.Close();
        }
    }
}
