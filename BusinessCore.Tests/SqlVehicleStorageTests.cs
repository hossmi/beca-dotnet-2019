using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
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
            recreate();
        }

        private static void create(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            string FilePath = Path.Combine(Environment.CurrentDirectory, "Scripts", "database-creation.sql");
            string file = File.ReadAllText(FilePath);
            SqlCommand sentence = new SqlCommand(file, con);
            con.Open();
            int affectedRows = sentence.ExecuteNonQuery();
            con.Close();

        }

        private static void drop(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            string FilePath = Path.Combine(Environment.CurrentDirectory, "Scripts", "database-drop.sql");
            string file = File.ReadAllText(FilePath);
            SqlCommand sentence = new SqlCommand(file, con);
            con.Open();
            int affectedRows = sentence.ExecuteNonQuery();
            con.Close();
        }

    }
}
