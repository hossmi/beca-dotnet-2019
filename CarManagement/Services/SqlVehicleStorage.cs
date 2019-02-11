using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private const string sentenceCountVehicle = "SELECT count(*) FROM vehicle;";

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count
        {
            get
            {
                return executeScalarQuery(this.connectionString, sentenceCountVehicle);
            }
        }

        public void clear()
        {
            string sentenceClearEnrollment = "DELETE FROM enrollment";
            string sentenceClearVehicle = "DELETE FROM vehicle";
            string sentenceClearWherl = "DELETE FROM wheel";
            string sentenceClearDoor = "DELETE FORM door";
            List<String> deleteTables = new List<String>();
            deleteTables.Add(sentenceClearDoor);
            deleteTables.Add(sentenceClearWherl);
            deleteTables.Add(sentenceClearVehicle);
            deleteTables.Add(sentenceClearEnrollment);

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                foreach (String sentenceDeleteTable in deleteTables)
                {
                    SqlCommand deleteComand = new SqlCommand(sentenceDeleteTable, connection);
                    deleteComand.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IVehicle get(IEnrollment enrollment)
        {
            int enrollmentId = getEnrollmentId(this.connectionString, enrollment);//0-enrollmentId-NULL

            string queryGetVehicle = "SELECT * FROM vehicle WHERE enrollmentId=" + enrollmentId + "";

            VehicleDto vehicleDto = new VehicleDto();

            EnrollmentDto enrollmentDto = new EnrollmentDto();
            enrollmentDto.Serial = enrollment.Serial;
            enrollmentDto.Number = enrollment.Number;
            SqlConnection connection = new SqlConnection(this.connectionString);
            connection.Open();
            SqlCommand sentenceGetVehicle = new SqlCommand(queryGetVehicle, connection);
            SqlDataReader reader = sentenceGetVehicle.ExecuteReader();
            reader.Read();
            vehicleDto.Enrollment = enrollmentDto;
            vehicleDto.Color = (CarColor)reader["color"];
            vehicleDto.Engine = new EngineDto();
            vehicleDto.Engine.HorsePower = (int)reader["engineHorsePower"];
            vehicleDto.Engine.IsStarted = (bool)reader["engineIsStarted"];
            connection.Close();

            //falta wheels y doors
            //DoorDto[] = new DoorDto();

            connection.Close();

            
            
            IVehicle car =  vehicleBuilder.import(vehicleDto);

            throw new NotImplementedException();
        }


        public IEnumerable<IVehicle> getAll()
        {
            throw new NotImplementedException();
        }

        public void set(IVehicle vehicle)
        {
            throw new NotImplementedException();
        }

        private static int executeScalarQuery(string connectionString, string query)
        {
            int result;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            result = (int)command.ExecuteScalar();
            connection.Close();

            return result;
        }

        private bool existsEntollment(String connectionString, IEnrollment enrollment)
        {
            string serial = enrollment.Serial;
            int number = enrollment.Number;
            bool existeEnrollment = false;

            string sentenceExistsEnrollment = "SELECT count(*) FROM enrollment " +
                "WHERE serial=" + serial + " AND number=" + number + ";";
            int enrollmentId = 0;
            enrollmentId = executeScalarQuery(connectionString, sentenceExistsEnrollment);

            if (enrollmentId != 0)
            {
                existeEnrollment = true;
            }

            return existeEnrollment;
        }

        private int getEnrollmentId(String connectionString, IEnrollment enrollment) 
        {
            string serial = enrollment.Serial;
            int number = enrollment.Number;
            int enrollmentId = 0;

            string sentenceGetEnrollmentId = "SELECT id FROM enrollment " +
                "WHERE serial=" + serial + " AND number=" + number + ";";
            enrollmentId = executeScalarQuery(connectionString, sentenceGetEnrollmentId);


            return enrollmentId;
        }
    }
}
