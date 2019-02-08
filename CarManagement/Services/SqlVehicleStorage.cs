using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core.Models;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private const string clearCommand = "DELETE FROM door; " +
            //"DBCC CHECKIDENT(door, RESEED, 0); " +
            "DELETE FROM wheel; " +
            //"DBCC CHECKIDENT (wheel, RESEED, 0); " +
            "DELETE FROM vehicle; " +
            "DELETE FROM enrollment;";
            //"DBCC CHECKIDENT (enrollment, RESEED, 0);";
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private readonly SqlConnection connection;

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;

            this.connection = new SqlConnection(this.connectionString);
            this.connection.Open();
        }

        public int Count { get; }

        public void clear()
        {
            SqlCommand command = new SqlCommand(clearCommand, this.connection);
            int affectedRows=command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            this.connection.Close();
        }

        public IVehicle get(IEnrollment enrollment)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IVehicle> getAll()
        {
            List<IVehicle> vehicleCollection = new List<IVehicle>();
            string getEnrollments = "SELECT serial, number, id FROM enrollment";
            SqlDataReader enrollmentResults;

            using (SqlCommand command = new SqlCommand(getEnrollments, this.connection))
            {
                enrollmentResults = command.ExecuteReader();
            }

            while (enrollmentResults.Read())
            {
                string serial = enrollmentResults.GetValue(0).ToString();
                int number = Convert.ToInt32(enrollmentResults.GetValue(1));
                int enrollmentId = (int)enrollmentResults.GetValue(2);

                string getVehicle = "SELECT color, engineHorsePower, engineIsStarted FROM vehicle " +
                    "WHERE (enrollmentId=@id)";

                SqlCommand commandVehicle = new SqlCommand(getVehicle, this.connection);
                commandVehicle.Parameters.AddWithValue("@id", enrollmentId);
                SqlDataReader vehicleResults = commandVehicle.ExecuteReader();

                vehicleResults.Read();

                CarColor color = (CarColor)vehicleResults.GetValue(0);
                int horseposer = (int)vehicleResults.GetValue(1);
                bool isStarted = (bool)vehicleResults.GetValue(2);
                                                                                           
            }

            return vehicleCollection;
        }

        public void set(IVehicle vehicle)
        {
            throw new NotImplementedException();
        }
    }
}
