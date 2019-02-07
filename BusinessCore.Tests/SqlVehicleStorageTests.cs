using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        private static void create(string connectionString)
        {
        }

        private static void drop(string connectionString)
        {            
            string filePath = Path.Combine(Environment.CurrentDirectory, "Scripts", "database-drop.sql");
            string sentencies = File.ReadAllText(filePath);

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            command.CommandText = sentencies;
            command.Connection = connection;

            connection.Open();
            int afectedRows = command.ExecuteNonQuery();
            connection.Close();
        }

    }
}
