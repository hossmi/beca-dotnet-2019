using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core.Models.DTOs;
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
            int affectedRows = command.ExecuteNonQuery();
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
                EnrollmentDto enrollment = new EnrollmentDto();
                enrollment.Serial = enrollmentResults.GetValue(0).ToString();
                enrollment.Number = Convert.ToInt32(enrollmentResults.GetValue(1));
                int enrollmentId = (int)enrollmentResults.GetValue(2);

                string getVehicle = "SELECT color, engineHorsePower, engineIsStarted FROM vehicle " +
                    "WHERE (enrollmentId=@id)";

                SqlCommand commandVehicle = new SqlCommand(getVehicle, this.connection);
                commandVehicle.Parameters.AddWithValue("@id", enrollmentId);
                SqlDataReader vehicleResults = commandVehicle.ExecuteReader();

                vehicleResults.Read();

                VehicleDto vehicle = new VehicleDto();
                vehicle.Enrollment = enrollment;
                CarColor color;
                Enum.TryParse<CarColor>(vehicleResults.GetValue(0).ToString(), out color);
                vehicle.Color = color;
                EngineDto engine = new EngineDto();
                engine.HorsePower = Convert.ToInt32(vehicleResults.GetValue(1));
                engine.IsStarted = Convert.ToBoolean(vehicleResults.GetValue(2));
                vehicle.Engine = engine;
                string getWheels = "SELECT pressure FROM wheel " +
                    "WHERE (vehicleId=@id)";
                SqlCommand commandWheels = new SqlCommand(getWheels, this.connection);
                commandWheels.Parameters.AddWithValue("@id", enrollmentId);
                SqlDataReader wheelsResults = commandWheels.ExecuteReader();

                List<WheelDto> wheels = new List<WheelDto>();
                while (wheelsResults.Read())
                {
                    WheelDto wheel = new WheelDto();
                    wheel.Pressure = Convert.ToDouble(wheelsResults.GetValue(0));
                    wheels.Add(wheel);
                }
                vehicle.Wheels = wheels.ToArray();

                string getDoors = "SELECT isOpen FROM door " +
                     "WHERE (vehicleId=@id)";
                SqlCommand commandDoors = new SqlCommand(getDoors, this.connection);
                commandDoors.Parameters.AddWithValue("@id", enrollmentId);
                SqlDataReader doorsResults = commandDoors.ExecuteReader();

                List<DoorDto> doors = new List<DoorDto>();
                while (doorsResults.Read())
                {
                    DoorDto door = new DoorDto();
                    door.IsOpen = Convert.ToBoolean(vehicleResults.GetValue(0));
                    doors.Add(door);
                }
                vehicle.Doors = doors.ToArray();

                vehicleCollection.Add(this.vehicleBuilder.import(vehicle));
            }

            return vehicleCollection;
        }

        public void set(IVehicle vehicle)
        {
            throw new NotImplementedException();
        }
    }
}
