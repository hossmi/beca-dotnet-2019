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
        private const string UPDATE_VEHICLE = @"UPDATE [vehicle] SET [color] = @color, [engineHorsePower] = @engineHorsePower, [engineIsStarted] = @engineIsStarted
                                                WHERE [enrollmentId] = @enrollmentId";
        private const string UPDATE_DOOR = @"UPDATE [door] SET [isOpen] = @isOpen WHERE [vehicleId] = @vehicleId";
        private const string UPDATE_WHEEL = @"UPDATE [wheel] SET [pressure] = @pressure WHERE [vehicleId] = @vehicleId";

        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count { get; }

        public void clear()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IVehicle get(IEnrollment enrollment)
        {
            VehicleDto vehicle = new VehicleDto();
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            EngineDto engine = new EngineDto();
            List<WheelDto> wheels = new List<WheelDto>();
            List<DoorDto> doors = new List<DoorDto>();

            string enrollmentId;

            SqlConnection conn = new SqlConnection(this.connectionString);
            conn.Open();

            string selectEnrollmentId = @"SELECT [id] FROM [enrollment] WHERE [serial] = @serial AND [number] = @number";
            using (SqlCommand command = new SqlCommand(selectEnrollmentId, conn))
            {
                command.Parameters.AddWithValue("@serial", enrollment.Serial);
                command.Parameters.AddWithValue("@number", enrollment.Number);
                enrollmentId = command.ExecuteScalar().ToString();
            }

            enrollmentDto.Number = enrollment.Number;
            enrollmentDto.Serial = enrollment.Serial;

            string selectVehicle = @"SELECT * FROM [vehicle] WHERE [enrollmentId] = @enrollmentId";
            using (SqlCommand command = new SqlCommand(selectVehicle, conn))
            {
                command.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    engine.HorsePower = Convert.ToInt32(dataReader["engineHorsePower"]);
                    engine.IsStarted = Convert.ToBoolean(dataReader["engineIsStarted"]);
                    vehicle.Color = (CarColor)Convert.ToInt32(dataReader["color"]);
                    vehicle.Enrollment = enrollmentDto;
                    vehicle.Engine = engine;
                }
            }

            string selectWheels = @"SELECT * FROM [wheel] WHERE [vehicleId] = @enrollmentId";
            using (SqlCommand command = new SqlCommand(selectWheels, conn))
            {
                command.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                SqlDataReader dataReader = command.ExecuteReader();
                                
                while (dataReader.Read())
                {
                    WheelDto wheel = new WheelDto();
                    wheel.Pressure = Convert.ToDouble(dataReader["pressure"]);
                    wheels.Add(wheel);
                }
            }

            string selectDoors = @"SELECT * FROM [door] WHERE [vehicleId] = @enrollmentId";
            using (SqlCommand command = new SqlCommand(selectDoors, conn))
            {
                command.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                SqlDataReader dataReader = command.ExecuteReader();
                
                while (dataReader.Read())
                {
                    DoorDto door = new DoorDto();
                    door.IsOpen = Convert.ToBoolean(dataReader["isOpen"]);
                    doors.Add(door);
                }
            }

            conn.Close();

            vehicle.Wheels = wheels.ToArray();
            vehicle.Doors = doors.ToArray();

            return this.vehicleBuilder.import(vehicle); 
        }

        public IEnumerable<IVehicle> getAll()
        {
            List<IVehicle> vehicles = new List<IVehicle>();
            
            SqlConnection conn = new SqlConnection(this.connectionString);
            conn.Open();

            string selectVehicles = @"SELECT * FROM [vehicle]";
            using (SqlCommand command = new SqlCommand(selectVehicles, conn))
            {
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    VehicleDto vehicle = new VehicleDto();
                    //vehicles.Add(dataReader[""]);
                }
            }

            conn.Close();

            return vehicles;
        }

        public void set(IVehicle vehicle)
        {
            SqlConnection conn = new SqlConnection(this.connectionString);
            conn.Open();

            string enrollmentId;
            using (SqlCommand command = new SqlCommand(UPDATE_ENROLLMENT, conn))
            {
                command.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                command.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                command.ExecuteNonQuery();
            }

            string selectEnrollmentId = @"SELECT [id] FROM [enrollment] WHERE [serial] = @serial AND [number] = @number";
            using (SqlCommand command = new SqlCommand(selectEnrollmentId, conn))
            {
                command.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                command.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                enrollmentId = command.ExecuteScalar().ToString();
            }

            using (SqlCommand command = new SqlCommand(UPDATE_VEHICLE, conn))
            {
                command.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                command.Parameters.AddWithValue("@color", vehicle.Color);
                command.Parameters.AddWithValue("@engineHorsePower", vehicle.Engine.HorsePower);
                command.Parameters.AddWithValue("@engineIsStarted", vehicle.Engine.IsStarted);
                command.ExecuteNonQuery();
            }

            foreach (IWheel wheel in vehicle.Wheels)
            {
                using (SqlCommand command = new SqlCommand(UPDATE_WHEEL, conn))
                {
                    command.Parameters.AddWithValue("@vehicleId", enrollmentId);
                    command.Parameters.AddWithValue("@pressure", wheel.Pressure);
                    command.ExecuteNonQuery();
                }
            }

            foreach (IDoor door in vehicle.Doors)
            {
                using (SqlCommand command = new SqlCommand(UPDATE_DOOR, conn))
                {
                    command.Parameters.AddWithValue("@vehicleId", enrollmentId);
                    command.Parameters.AddWithValue("@isOpen", door.IsOpen);
                    command.ExecuteNonQuery();
                }
            }

            conn.Close();
        }
    }
}
