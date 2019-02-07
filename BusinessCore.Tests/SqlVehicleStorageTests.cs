using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using CarManagement.Core;
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
            using (SqlConnection sqlDbConnection = new SqlConnection(connectionString))
            {
                string script = File.ReadAllText(@"Scripts\database-creation.sql");
                sqlDbConnection.Open();

                SqlCommand createTablesCommand = new SqlCommand(script, sqlDbConnection);
                createTablesCommand.ExecuteNonQuery();
                sqlDbConnection.Close();
            }
        }

        private static void drop(string connectionString)
        {
            using (SqlConnection sqlDbConnection = new SqlConnection(connectionString))
            {
                string script = File.ReadAllText(@"Scripts\database-drop.sql");
                sqlDbConnection.Open();

                SqlCommand dropTablesCommand = new SqlCommand(script, sqlDbConnection);
                dropTablesCommand.ExecuteNonQuery();
                sqlDbConnection.Close();
            }
        }

    }
}
