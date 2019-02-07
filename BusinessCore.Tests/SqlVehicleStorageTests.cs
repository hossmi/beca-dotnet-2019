using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    [TestCategory("Requires SQL Server connection")]
    [TestClass]
    public class SqlVehicleStorageTests
    {
        private const string ConnectionStringKey = "Data Source = ALC-45W9LQ1\SQLEXPRESS; " +
                               "Initial Catalog=DataBaseName;" +
                               "User id=UserName;" +
                               "Password=Secret;";

        private const string ConnectionStringAlt = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


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
            throw new NotImplementedException();
            dbConect(connectionString);
        }

        private static void drop(string connectionString)
        {
            throw new NotImplementedException();
            dbConect(connectionString);
        }

        private static void dbConect(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.ConnectionString = connectionString;

                connection.Open();
                // Use the connection
            }
        }

    }
}
