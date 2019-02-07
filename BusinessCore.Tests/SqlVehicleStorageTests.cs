using System;
using System.Configuration;
using System.Data;
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
            string filePath = Path.Combine(Environment.CurrentDirectory, "Scripts", "database-creation.sql");
            string script = File.ReadAllText(filePath);
            executeNonQueryCommand(connectionString, script);
        }

        private static void drop(string connectionString)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Scripts", "database-drop.sql");
            string script = File.ReadAllText(filePath);
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
