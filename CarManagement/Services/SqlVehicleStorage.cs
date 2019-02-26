using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using ToolBox.Services;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {

        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;

        #region --------------------------SQL REGION---------------------------------------
        private const string SELECT_ALL_ENROLLMENTS = @"SELECT serial, number FROM enrollment 
                                WHERE(EXISTS(select enrollmentId from vehicle v where v.enrollmentId = enrollment.id))";
        private const string PUSH_TO_ENROLLMENTDB = @"
             INSERT INTO enrollment(serial, number) 
             output INSERTED.ID VALUES(@serial,@number) ";

        private const string PUSH_TO_VEHICLE = @"
             INSERT INTO vehicle(enrollmentId,color,engineHorsePower,engineIsStarted)
             VALUES(@enrrollmentId,@color,@engineHorsePower,@engineIsStarted) ";

        private const string PUSH_TO_WHEEL = @"
             INSERT INTO wheel(vehicleId,pressure)
             VALUES(@vehicleId,@pressure) ";

        private const string PUSH_TO_DOOR = @"
             INSERT INTO door(vehicleId,isOpen)
             VALUES(@vehicleId,@isOpen) ";

        private const string UPDATE_WITH_VEHICLE = @"
             UPDATE vehicle
             SET color = @color,engineHorsePower = @engineHorsePower, engineIsStarted = @engineIsStarted 
             WHERE enrollmentId = @enrollmentId ";

        private const string UPDATE_WITH_ENROLLMENT = @"
             UPDATE enrollment 
             SET serial = @serial, number = @number
             WHERE id = @id ";


        private const string UPDATE_WITH_WHEELS = @"
             UPDATE wheel 
             SET pressure = @pressure 
             WHERE vehicleId = @vehicleId ";


        private const string UPDATE_WITH_DOORS = @"
             UPDATE door 
             SET isOpen = @isOpen 
             WHERE vehicleId = @vehicleId ";

        private const string SELECT_ID_FROM_ENROLLMENT = @"
             SELECT id 
             FROM enrollment
             WHERE serial = @serial
             AND number = @number ";
        private const string DELETE_DOORS = @"
              DELETE 
              FROM Door 
              WHERE vehicleId = @id ";
        private const string DELETE_WHEELS = @"
              DELETE 
              FROM wheel
              WHERE vehicleId = @id ";

        private const string BASE_QUERRY = @"
            SELECT e.serial, 
                   e.number, 
                   v.engineHorsePower, 
                   v.engineIsStarted, 
                   v.enrollmentId, 
                   v.color 
            FROM vehicle v
            INNER JOIN enrollment e ON v.enrollmentId = e.id 
            WHERE enrollmentId = id ";

        private const string SELECT_COUNT_VEHICLES = @"
             SELECT COUNT(engineHorsePower) 
             FROM vehicle";

        #endregion

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count
        {
            get
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                using (SqlCommand command = new SqlCommand(SELECT_COUNT_VEHICLES, connection))
                {

                    connection.Open();
                    int numero = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                    return numero;

                }

            }
        }

        public void clear()
        {
            List<string> clearDbQuery = new List<string>();
            clearDbQuery.Add("DELETE FROM door");
            clearDbQuery.Add("DELETE FROM wheel");
            clearDbQuery.Add("DELETE FROM vehicle");
            clearDbQuery.Add("DELETE FROM enrollment");
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                for (int i = 0; i < clearDbQuery.Count; i++)
                {

                    SqlCommand command = new SqlCommand(clearDbQuery[i], connection);
                    command.ExecuteNonQuery();

                }
                connection.Close();
            }

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void set(IVehicle vehicle)
        {

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                int id;

                connection.Open();

                using (SqlCommand command = new SqlCommand(SELECT_ID_FROM_ENROLLMENT, connection))
                {
                    command.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                    command.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                    id = Convert.ToInt32(command.ExecuteScalar());
                }


                if (vehicleExist(connection, id))
                    updateDb(id, connection, vehicle);
                else
                    insertDb(id, connection, vehicle);
            }

        }

        private static void insertDb(int id, SqlConnection connection, IVehicle vehicle)
        {

            SqlCommand command = new SqlCommand(PUSH_TO_ENROLLMENTDB, connection);
            command.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
            command.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
            int enrollmentId = Convert.ToInt32(command.ExecuteScalar());

            command = new SqlCommand(PUSH_TO_VEHICLE, connection);
            command.Parameters.AddWithValue("@enrrollmentId", enrollmentId);
            command.Parameters.AddWithValue("@color", vehicle.Color);
            command.Parameters.AddWithValue("@engineHorsePower", vehicle.Engine.HorsePower);
            command.Parameters.AddWithValue("@engineIsStarted", vehicle.Engine.IsStarted);
            command.ExecuteNonQuery();

            foreach (IWheel wheels in vehicle.Wheels)
            {
                command = new SqlCommand(PUSH_TO_WHEEL, connection);
                command.Parameters.AddWithValue("@pressure", wheels.Pressure);
                command.Parameters.AddWithValue("@vehicleId", enrollmentId);
                command.ExecuteNonQuery();

            }
            foreach (IDoor doors in vehicle.Doors)
            {
                command = new SqlCommand(PUSH_TO_DOOR, connection);
                command.Parameters.AddWithValue("@isOpen", doors.IsOpen);
                command.Parameters.AddWithValue("@vehicleId", enrollmentId);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        private static void updateDb(int id, SqlConnection conection, IVehicle vehicle)
        {

            SqlCommand command = new SqlCommand(UPDATE_WITH_VEHICLE, conection);
            command.Parameters.AddWithValue("@color", vehicle.Color);
            command.Parameters.AddWithValue("@engineHorsePower", vehicle.Engine.HorsePower);
            command.Parameters.AddWithValue("@engineIsStarted", vehicle.Engine.IsStarted);
            command.Parameters.AddWithValue("@enrollmentId", id);
            command.ExecuteNonQuery();

            command = new SqlCommand(UPDATE_WITH_ENROLLMENT, conection);
            command.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
            command.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            command = new SqlCommand(DELETE_DOORS, conection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            command = new SqlCommand(DELETE_WHEELS, conection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            foreach (IWheel wheels in vehicle.Wheels)
            {
                command = new SqlCommand(PUSH_TO_WHEEL, conection);
                command.Parameters.AddWithValue("@pressure", wheels.Pressure);
                command.Parameters.AddWithValue("@vehicleId", id);
                command.ExecuteNonQuery();

            }

            foreach (IDoor doors in vehicle.Doors)
            {
                command = new SqlCommand(PUSH_TO_DOOR, conection);
                command.Parameters.AddWithValue("@isOpen", doors.IsOpen);
                command.Parameters.AddWithValue("@vehicleId", id);
                command.ExecuteNonQuery();

            }
            conection.Close();
        }

        private static bool vehicleExist(SqlConnection connection, int id)
        {
            string query = "SELECT COUNT" +
                            "(" +
                                "enrollmentId" +
                            ") " +
                            "FROM " +
                                "vehicle " +
                            "WHERE " +
                                "enrollmentId = @id";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                int count = Convert.ToInt32(command.ExecuteScalar());

                return count > 0;
            }
        }

        public IVehicle get(IEnrollment enrollment)
        {

            VehicleDto vehicleDto = new VehicleDto();
            vehicleDto.Enrollment.Number = enrollment.Number;
            vehicleDto.Enrollment.Serial = enrollment.Serial;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                String getFromEnrollment = "SELECT  id,serial,enrollment FROM enrollment WHERE serial = " + enrollment.Serial + ", number = " + enrollment.Number;
                SqlCommand command = new SqlCommand(getFromEnrollment, connection);
                connection.Open();
                IDataReader reader = command.ExecuteReader();
                String getFromVehicle = "SELECT  color,engineHorsePower,engineIsStarted FROM wheel WHERE id =" + Convert.ToInt32(reader["id"]);
                command = new SqlCommand(getFromVehicle, connection);
                reader = command.ExecuteReader();
                vehicleDto.Color = (CarColor)Convert.ToInt32(reader["color"]);
                vehicleDto.Engine.HorsePower = Convert.ToInt32(reader["engineHorsePower"]);
                vehicleDto.Engine.IsStarted = Convert.ToBoolean(reader["engineIsStarted"]);

                string getFromWheel = "SELECT  pressure FROM wheel WHERE id =" + Convert.ToInt32(reader["id"]);
                command = new SqlCommand(getFromWheel, connection);
                reader = command.ExecuteReader();
                reader.Read();
                float[] vehiclePressure = null;
                for (int i = 0; reader.Read(); i++)
                {
                    vehiclePressure[i] = (float)(reader["pressure"]);
                    reader.Read();
                }
                int counter = 0;
                foreach (WheelDto wheels in vehicleDto.Wheels)
                {
                    wheels.Pressure = vehiclePressure[counter];
                    counter++;
                }
                string getFromDoor = "SELECT  isOpen FROM door WHERE id =" + Convert.ToInt32(reader["id"]);
                command = new SqlCommand(getFromDoor, connection);
                reader = command.ExecuteReader();
                reader.Read();
                bool[] vehicleIsOpen = null;
                for (int i = 0; reader.Read(); i++)
                {
                    vehicleIsOpen[i] = (bool)reader["isOpen"];
                    reader.Read();
                }
                counter = 0;
                foreach (DoorDto doors in vehicleDto.Doors)
                {
                    doors.IsOpen = vehicleIsOpen[counter];
                    counter++;
                }

                connection.Close();
            }

            return this.vehicleBuilder.import(vehicleDto);


        }

        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IDictionary<string, string> filters;
            private string indexEngineHorsePower = "whereHorsePower";

            public IEnumerable<IEnrollment> Keys
            {
                get
                {
                    return enumerateEnrollments();
                }
            }

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.filters = new Dictionary<string, string>();
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereColorIs)));
                this.filters[nameof(whereColorIs)] = " color = " + Convert.ToInt32(color) + " ";
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEngineIsStarted)));
                this.filters[nameof(whereEngineIsStarted)] = " isStarted = " + started + " ";
                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEnrollmentIs)));
                this.filters[nameof(whereEnrollmentIs)] = "serial = '" + enrollment.Serial + "' AND number = " + enrollment.Number + " ";
                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEnrollmentSerialIs)));
                this.filters[nameof(whereEnrollmentSerialIs)] = " serial = " + serial + " ";
                return this;
            }
            public IVehicleQuery whereEnrollmentNumberIs(string number)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEnrollmentNumberIs)));
                this.filters[nameof(whereEnrollmentNumberIs)] = " number = " + number.ToString() + " ";
                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                Asserts.isFalse(this.filters.ContainsKey(this.indexEngineHorsePower));
                this.filters[nameof(this.indexEngineHorsePower)] = " engineHorsePower = " + horsePower.ToString() + " ";
                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(this.indexEngineHorsePower)));
                this.filters[nameof(this.indexEngineHorsePower)] = " engineHorsePower  >= " + min + " AND engineHorsePower <= " + max + " ";
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }

            private IEnumerator<IVehicle> enumerate()
            {
                SqlConnection connection = new SqlConnection(this.connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(getMeQuerry(this.filters.Values), connection);
                IEnumerator<IVehicle> vehicle = giveMeValues(command);
                connection.Close();
                return vehicle;
            }

            private static string getMeQuerry(IEnumerable<string> filters)
            {
                string querry = "";

                if (querry != "")
                {
                    querry = " WHERE ";
                }
                foreach (string filter in filters)
                {
                    querry += " AND " + filter;
                }

                return querry = BASE_QUERRY + querry;
            }

            private IEnumerator<IVehicle> giveMeValues(SqlCommand command)
            {

                command.Connection.Open();
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        VehicleDto vehicleDto = new VehicleDto()
                        {
                            Color = (CarColor)(Convert.ToInt32(reader["color"])),
                            Engine = new EngineDto()
                            {
                                HorsePower = Convert.ToInt32(reader["engineHorsePower"]),
                                IsStarted = Convert.ToBoolean(reader["engineIsStarted"]),
                            },
                            Enrollment = new EnrollmentDto()
                            {
                                Serial = Convert.ToString(reader["serial"]),
                                Number = Convert.ToInt32(reader["number"]),
                            },
                        };

                        int enrollmentID = Convert.ToInt32(reader["enrollmentid"]);
                        string getVehicleWheel = $@"SELECT pressure FROM wheel WHERE vehicleId =   {enrollmentID};";
                        command = new SqlCommand(getVehicleWheel, command.Connection);
                        vehicleDto.Wheels = giveMeWheels(command);

                        string getVehicleDoor = $@"SELECT isOpen FROM door WHERE vehicleId =   {enrollmentID};"; ;
                        command = new SqlCommand(getVehicleDoor, command.Connection);
                        vehicleDto.Doors = giveMeDoors(command);
                        yield return this.vehicleBuilder.import(vehicleDto);

                    }
                }

            }

            private static DoorDto[] giveMeDoors(SqlCommand command)
            {
                using (IDataReader readerDoor = command.ExecuteReader())
                {
                    IList<DoorDto> doors = new List<DoorDto>();

                    while (readerDoor.Read())
                    {
                        DoorDto door = new DoorDto()
                        {
                            IsOpen = Convert.ToBoolean(readerDoor["isOpen"]),
                        };
                        doors.Add(door);
                    }
                    return doors.ToArray();
                }
            }

            private static WheelDto[] giveMeWheels(SqlCommand command)
            {
                IList<WheelDto> wheels = new List<WheelDto>();
                using (IDataReader readerWheel = command.ExecuteReader())
                {
                    while (readerWheel.Read())
                    {
                        WheelDto wheel = new WheelDto()
                        {
                            Pressure = Convert.ToDouble(readerWheel["pressure"]),
                        };
                        wheels.Add(wheel);
                    }
                    return wheels.ToArray();
                }
            }

            private IEnumerable<IEnrollment> enumerateEnrollments()
            {

                using (IDbConnection connection = new SqlConnection(this.connectionString))
                {
                    IDbCommand command = new SqlCommand(SELECT_ALL_ENROLLMENTS);
                    command.Connection = connection;
                    connection.Open();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string serial = reader["serial"].ToString();
                            int number = Convert.ToInt32(reader["number"]);
                            yield return this.vehicleBuilder.import(serial, number);
                        }
                    }
                    connection.Close();
                }
            }

        }

    }
}
