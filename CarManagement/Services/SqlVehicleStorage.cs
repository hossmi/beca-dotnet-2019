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
            "DELETE FROM wheel; " +
            "DELETE FROM vehicle; " +
            "DELETE FROM enrollment;";

        private const string COUNT_VEHICLES = "SELECT COUNT(enrollmentId) FROM vehicle";

        private const string INSERT_ENROLLMENT = "INSERT INTO [enrollment] (serial,number) " +
            "OUTPUT INSERTED.ID " +
            "VALUES (@serial, @number)";
        private const string INSERT_VEHICLE = "INSERT INTO [vehicle] (enrollmentId, color, engineHorsePower, engineIsStarted) " +
            "VALUES (@enrollmentKEY, @color, @horsepower, @started)";
        private const string INSERT_WHEEL = "INSERT INTO [wheel] (pressure,vehicleId) " +
            "VALUES (@pressure, @enrollmentKEY)";
        private const string INSERT_DOOR = "INSERT INTO [door] (isOpen, vehicleId) " +
            "VALUES (@open, @enrollmentKEY)";

        private const string SELECT_ENROLLMENT_WITH_PARAMS = "SELECT id FROM enrollment " +
            "WHERE (serial = @serial AND number = @number)";
        private const string SELECT_ENROLLMENTS = "SELECT serial, number, id FROM enrollment";
        private const string SELECT_VEHICLE = "SELECT enrollmentId, color, engineHorsePower, engineIsStarted FROM vehicle " +
            "WHERE (enrollmentId=@id)";
        private const string SELECT_WHEEL = "SELECT id, pressure FROM wheel " +
            "WHERE (vehicleId=@vehicleId)";
        private const string SELECT_DOOR = "SELECT id, isOpen from door " +
            "WHERE (vehicleId=@vehicleId)";


        private const string UPDATE_VEHICLE = "UPDATE [vehicle] " +
            "SET color = @color, engineHorsePower = @horsepower, engineIsStarted = @started " +
            "WHERE enrollmentId = @enrollmentKEY";
        private const string UPDATE_WHEEL = "UPDATE [wheel] " +
            "SET pressure = @pressure " +
            "WHERE id = @id ";
        private const string UPDATE_DOOR = "UPDATE [door] " +
            "SET isOpen = @open " +
            "WHERE id = @id";



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

        public int Count
        {
            get
            {
                SqlCommand commandVehicle = new SqlCommand(COUNT_VEHICLES, this.connection);
                int vehicleCount = Convert.ToInt32(commandVehicle.ExecuteScalar());

                return vehicleCount;
            }
        }

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
            int enrollmentId;

            using (SqlCommand command = new SqlCommand(SELECT_ENROLLMENT_WITH_PARAMS, this.connection))
            {
                command.Parameters.AddWithValue("@serial", enrollment.Serial);
                command.Parameters.AddWithValue("@number", enrollment.Number);
                enrollmentId = Convert.ToInt32(command.ExecuteScalar());
            }

            if (enrollmentId > 0)
            {
                EnrollmentDto enrollmentdto = new EnrollmentDto();
                enrollmentdto.Number = enrollment.Number;
                enrollmentdto.Serial = enrollment.Serial;

                SqlCommand commandVehicle = new SqlCommand(SELECT_VEHICLE, this.connection);
                commandVehicle.Parameters.AddWithValue("@id", enrollmentId);
                SqlDataReader vehicleResults = commandVehicle.ExecuteReader();

                vehicleResults.Read();

                if (vehicleResults.HasRows)
                {
                    VehicleDto vehicle = new VehicleDto();
                    vehicle.Enrollment = enrollmentdto;
                    vehicle.Color = (CarColor)Convert.ToInt32(vehicleResults["color"]);
                    EngineDto engine = new EngineDto();
                    engine.HorsePower = Convert.ToInt32(vehicleResults["engineHorsePower"]);
                    engine.IsStarted = Convert.ToBoolean(vehicleResults["engineIsStarted"]);
                    vehicle.Engine = engine;

                    SqlCommand commandWheels = new SqlCommand(SELECT_WHEEL, this.connection);
                    commandWheels.Parameters.AddWithValue("@vehicleId", enrollmentId);
                    SqlDataReader wheelsResults = commandWheels.ExecuteReader();

                    if (wheelsResults.HasRows)
                    {
                        List<WheelDto> wheels = new List<WheelDto>();
                        while (wheelsResults.Read())
                        {
                            WheelDto wheel = new WheelDto();
                            wheel.Pressure = Convert.ToDouble(wheelsResults["pressure"]);
                            wheels.Add(wheel);
                        }
                        vehicle.Wheels = wheels.ToArray();
                    }

                    SqlCommand commandDoors = new SqlCommand(SELECT_DOOR, this.connection);
                    commandDoors.Parameters.AddWithValue("@vehicleId", enrollmentId);
                    SqlDataReader doorsResults = commandDoors.ExecuteReader();

                    if (doorsResults.HasRows)
                    {
                        List<DoorDto> doors = new List<DoorDto>();
                        while (doorsResults.Read())
                        {
                            DoorDto door = new DoorDto();
                            door.IsOpen = Convert.ToBoolean(doorsResults["isOpen"]);
                            doors.Add(door);
                        }
                        vehicle.Doors = doors.ToArray();
                    }
                    return this.vehicleBuilder.import(vehicle);
                }
            }

            return null;
        }

        public IEnumerable<IVehicle> getAll()
        {
            List<IVehicle> vehicleCollection = new List<IVehicle>();
            SqlDataReader enrollmentResults;

            using (SqlCommand command = new SqlCommand(SELECT_ENROLLMENTS, this.connection))
            {
                enrollmentResults = command.ExecuteReader();
            }

            while (enrollmentResults.Read())
            {
                EnrollmentDto enrollment = new EnrollmentDto();
                enrollment.Serial = enrollmentResults["serial"].ToString();
                enrollment.Number = Convert.ToInt32(enrollmentResults["number"]);
                int enrollmentId = (int)enrollmentResults["id"];



                SqlCommand commandVehicle = new SqlCommand(SELECT_VEHICLE, this.connection);
                commandVehicle.Parameters.AddWithValue("@id", enrollmentId);
                SqlDataReader vehicleResults = commandVehicle.ExecuteReader();

                vehicleResults.Read();

                VehicleDto vehicle = new VehicleDto();
                vehicle.Enrollment = enrollment;
                vehicle.Color = (CarColor)Convert.ToInt32(vehicleResults["color"]);
                EngineDto engine = new EngineDto
                {
                    HorsePower = Convert.ToInt32(vehicleResults["engineHorsePower"]),
                    IsStarted = Convert.ToBoolean(vehicleResults["engineIsStarted"]),

                };
                vehicle.Engine = engine;

                SqlCommand commandWheels = new SqlCommand(SELECT_WHEEL, this.connection);
                commandWheels.Parameters.AddWithValue("@vehicleId", enrollmentId);
                SqlDataReader wheelsResults = commandWheels.ExecuteReader();

                List<WheelDto> wheels = new List<WheelDto>();
                while (wheelsResults.Read())
                {
                    WheelDto wheel = new WheelDto();
                    wheel.Pressure = Convert.ToDouble(wheelsResults["pressure"]);
                    wheels.Add(wheel);
                }
                vehicle.Wheels = wheels.ToArray();

                SqlCommand commandDoors = new SqlCommand(SELECT_DOOR, this.connection);
                commandDoors.Parameters.AddWithValue("@vehicleId", enrollmentId);
                SqlDataReader doorsResults = commandDoors.ExecuteReader();

                List<DoorDto> doors = new List<DoorDto>();
                while (doorsResults.Read())
                {
                    DoorDto door = new DoorDto();
                    door.IsOpen = Convert.ToBoolean(doorsResults["isOpen"]);
                    doors.Add(door);
                }
                vehicle.Doors = doors.ToArray();

                vehicleCollection.Add(this.vehicleBuilder.import(vehicle));
            }

            return vehicleCollection;
        }

        public void set(IVehicle vehicle)
        {
            int enrollmentId;
            int vehicleId;

            using (SqlCommand command = new SqlCommand(SELECT_ENROLLMENT_WITH_PARAMS, this.connection))
            {
                command.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                command.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                enrollmentId = Convert.ToInt32(command.ExecuteScalar());
            }

            using (SqlCommand command = new SqlCommand(SELECT_VEHICLE, this.connection))
            {
                command.Parameters.AddWithValue("@id", enrollmentId);
                vehicleId = Convert.ToInt32(command.ExecuteScalar());
            }

            if (enrollmentId >0 && vehicleId>0)
            {
                int updatedVehicles = 0;
                int updatedWheels = 0;
                int updatedDoors = 0;

                SqlCommand sqlCommand = new SqlCommand(UPDATE_VEHICLE, this.connection);
                sqlCommand.Parameters.AddWithValue("@enrollmentKEY", enrollmentId);
                sqlCommand.Parameters.AddWithValue("@color", ((int)vehicle.Color));
                sqlCommand.Parameters.AddWithValue("@horsepower", vehicle.Engine.HorsePower);
                sqlCommand.Parameters.AddWithValue("@started", Convert.ToInt32(vehicle.Engine.IsStarted));
                updatedVehicles = updatedVehicles + sqlCommand.ExecuteNonQuery();

                SqlCommand commandWheels = new SqlCommand(SELECT_WHEEL, this.connection);
                commandWheels.Parameters.AddWithValue("@vehicleId", enrollmentId);
                SqlDataReader wheelsResults = commandWheels.ExecuteReader();

                int i = 0;
                while (wheelsResults.Read())
                {
                    int wheelId = Convert.ToInt32(wheelsResults["id"]);

                    sqlCommand = new SqlCommand(UPDATE_WHEEL, this.connection);
                    sqlCommand.Parameters.AddWithValue("@pressure", vehicle.Wheels[i].Pressure);
                    sqlCommand.Parameters.AddWithValue("@id", wheelId);
                    updatedWheels = updatedWheels + sqlCommand.ExecuteNonQuery();
                    i++;
                }

                SqlCommand commandDoors = new SqlCommand(SELECT_DOOR, this.connection);
                commandDoors.Parameters.AddWithValue("@vehicleId", enrollmentId);
                SqlDataReader doorsResults = commandDoors.ExecuteReader();

                i = 0;
                while (doorsResults.Read())
                {
                    int doorId = Convert.ToInt32(doorsResults["id"]);

                    sqlCommand = new SqlCommand(UPDATE_DOOR, this.connection);
                    sqlCommand.Parameters.AddWithValue("@open", vehicle.Doors[i].IsOpen);
                    sqlCommand.Parameters.AddWithValue("@id", doorId);
                    updatedDoors = updatedDoors + sqlCommand.ExecuteNonQuery();
                    i++;
                }
            }
            else
            {
                int insertedVehicles = 0;
                int insertedWheels = 0;
                int insertedDoors = 0;

                SqlCommand sqlCommand = new SqlCommand(INSERT_ENROLLMENT, this.connection);
                sqlCommand.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                sqlCommand.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                string enrollmentKEY = sqlCommand.ExecuteScalar().ToString();

                sqlCommand = new SqlCommand(INSERT_VEHICLE, this.connection);
                sqlCommand.Parameters.AddWithValue("@enrollmentKEY", enrollmentKEY);
                sqlCommand.Parameters.AddWithValue("@color", ((int)vehicle.Color));
                sqlCommand.Parameters.AddWithValue("@horsepower", vehicle.Engine.HorsePower);
                sqlCommand.Parameters.AddWithValue("@started", Convert.ToInt32(vehicle.Engine.IsStarted));
                insertedVehicles = insertedVehicles + sqlCommand.ExecuteNonQuery();

                foreach (IWheel wheel in vehicle.Wheels)
                {
                    sqlCommand = new SqlCommand(INSERT_WHEEL, this.connection);
                    sqlCommand.Parameters.AddWithValue("@pressure", wheel.Pressure);
                    sqlCommand.Parameters.AddWithValue("@enrollmentKEY", enrollmentKEY);
                    insertedWheels = insertedWheels + sqlCommand.ExecuteNonQuery();
                }

                foreach (IDoor door in vehicle.Doors)
                {
                    sqlCommand = new SqlCommand(INSERT_DOOR, this.connection);
                    sqlCommand.Parameters.AddWithValue("@open", Convert.ToInt32(door.IsOpen));
                    sqlCommand.Parameters.AddWithValue("@enrollmentKEY", enrollmentKEY);
                    insertedDoors = insertedDoors + sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}

