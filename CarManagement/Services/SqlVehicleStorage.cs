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

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {

        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private const string BASE_QUERRY = "SELECT " +
                                                "serial, " +
                                                "number, " +
                                                "engineHorsePower, " +
                                                "engineIsStarted, " +
                                                "enrollmentid, " +
                                                "color " +
                                           "FROM " +
                                                "vehicle, " +
                                                "enrollment " +
                                           "WHERE " +
                                                "enrollmentid = id ";

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
                {
                    using (SqlCommand command = new SqlCommand("SELECT COUNT(engineHorsePower) FROM vehicle", connection))
                    {

                        connection.Open();
                        int numero = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                        return numero;

                    }
                }
            }
        }

        public void clear()
        {
            List<string> clearDbQuerry = new List<string>();
            clearDbQuerry.Add("DELETE FROM door");
            clearDbQuerry.Add("DELETE FROM wheel");
            clearDbQuerry.Add("DELETE FROM vehicle");
            clearDbQuerry.Add("DELETE FROM enrollment");
            //------------------------------------------------------------------------------------------
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                //------------------------------------------------------------------------------------------
                connection.Open();
                //------------------------------------------------------------------------------------------
                for (int i = 0; i < clearDbQuerry.Count; i++)
                {

                    SqlCommand command = new SqlCommand(clearDbQuerry[i], connection);
                    command.ExecuteNonQuery();

                }
                //------------------------------------------------------------------------------------------
                connection.Close();
                //------------------------------------------------------------------------------------------
            }

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void set(IVehicle vehicle)
        {
                       
            using (SqlConnection conection = new SqlConnection(this.connectionString))
            {
                conection.Open();
                string querry = "SELECT " +
                                    "id " +
                                "FROM " +
                                    "enrollment " +
                                "WHERE " +
                                    "serial = '" + vehicle.Enrollment.Serial + "' " +
                                "AND " +
                                    "number = " + vehicle.Enrollment.Number + " ";

                SqlCommand command = new SqlCommand(querry, conection);
                int id = Convert.ToInt32(command.ExecuteScalar());

                bool exists = getMeIfVehicleExist(command, id);

                if (exists == true)
                {
                    updateDb(command, id, conection, vehicle);

                }
                else
                {
                    insertDb(command, id, conection, vehicle);
                }
            }
        }

        private void insertDb(SqlCommand command, int id, SqlConnection conection, IVehicle vehicle)
        {
            const string pushToEnrollmentDb = "INSERT INTO enrollment" +
                                                      "(" +
                                                          "serial," +
                                                          "number" +
                                                      ") " +
                                                      "output INSERTED.ID VALUES" +
                                                      "(" +
                                                          "@serial," +
                                                          "@number" +
                                                      ")";

            const string pushToWheel = "INSERT INTO " +
                                       "wheel" +
                                       "(" +
                                            "vehicleid," +
                                            "pressure" +
                                       ")" +
                                       "VALUES" +
                                       "(" +
                                           "@vehicleid," +
                                           "@pressure" +
                                       ")";

            const string pushToDoor = "INSERT INTO " +
                                      "door" +
                                      "(" +
                                          "vehicleid," +
                                          "isopen" +
                                      ")" +
                                      "VALUES" +
                                      "(" +
                                          "@vehicleid," +
                                          "@isopen" +
                                      ")";

            const string pushToVehicle = "INSERT INTO " +
                                         "vehicle" +
                                         "(" +
                                             "enrollmentId," +
                                             "color," +
                                             "engineHorsePower," +
                                             "engineIsStarted" +
                                         ") " +
                                         "VALUES" +
                                         "(" +
                                             "@enrrollmentId," +
                                             "@color," +
                                             "@engineHorsePower," +
                                             "@engineIsStarted" +
                                         ")";


            command = new SqlCommand(pushToEnrollmentDb, conection);
            command.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
            command.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
            int enrollmentId = Convert.ToInt32(command.ExecuteScalar());

            command = new SqlCommand(pushToVehicle, conection);
            command.Parameters.AddWithValue("@enrrollmentId", enrollmentId);
            command.Parameters.AddWithValue("@color", vehicle.Color);
            command.Parameters.AddWithValue("@engineHorsePower", vehicle.Engine.HorsePower);
            command.Parameters.AddWithValue("@engineIsStarted", vehicle.Engine.IsStarted);
            command.ExecuteNonQuery();

            foreach (IWheel wheels in vehicle.Wheels)
            {
                command = new SqlCommand(pushToWheel, conection);
                command.Parameters.AddWithValue("@pressure", wheels.Pressure);
                command.Parameters.AddWithValue("@vehicleid", enrollmentId);
                command.ExecuteNonQuery();

            }
            foreach (IDoor doors in vehicle.Doors)
            {
                command = new SqlCommand(pushToDoor, conection);
                command.Parameters.AddWithValue("@isopen", doors.IsOpen);
                command.Parameters.AddWithValue("@vehicleid", enrollmentId);
                command.ExecuteNonQuery();
            }
            conection.Close();
        }
        
        private void updateDb(SqlCommand command, int id,SqlConnection conection, IVehicle vehicle)
        {
            const string updateWithVehicle = "UPDATE " +
                                                            "vehicle " +
                                                     "SET " +
                                                            "color = @color, " +
                                                            "engineHorsePower = @HorsePower, " +
                                                            "engineIsStarted = @isStarted " +
                                                     "WHERE " +
                                                            "enrollmentId = @enrollmentid ";
            const string updateWithEnrollment = "UPDATE" +
                                                    "enrollment" +
                                                "SET" +
                                                    "serial = @serial," +
                                                    "number = @number" +
                                                "WHERE" +
                                                    "id = @id";


            const string updateWithWheels = "UPDATE " +
                                                "wheel " +
                                            "SET " +
                                                "pressure = @pressure" +
                                            "WHERE " +
                                                "vehicleId = @vehicleid ";


            const string updateWithDoors = "UPDATE " +
                                                "door " +
                                           "SET " +
                                                "isOpen = @isOpen " +
                                           "WHERE " +
                                                "vehicleId = @vehicleid ";

            command = new SqlCommand(updateWithVehicle, conection);
            command.Parameters.AddWithValue("@color", vehicle.Color);
            command.Parameters.AddWithValue("@HorsePower", vehicle.Engine.HorsePower);
            command.Parameters.AddWithValue("@isStarted", vehicle.Engine.IsStarted);
            command.Parameters.AddWithValue("@enrollmentid", id);
            command.ExecuteNonQuery();

            command = new SqlCommand(updateWithEnrollment, conection);
            command.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
            command.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            foreach (IWheel wheels in vehicle.Wheels)
            {
                command = new SqlCommand(updateWithWheels, conection);
                command.Parameters.AddWithValue("@pressure", wheels.Pressure);
                command.Parameters.AddWithValue("@vehicleid", id);
                command.ExecuteNonQuery();

            }

            foreach (IDoor doors in vehicle.Doors)
            {
                command = new SqlCommand(updateWithDoors, conection);
                command.Parameters.AddWithValue("@isOpen", doors.IsOpen);
                command.Parameters.AddWithValue("@vehicleid", id);
                command.ExecuteNonQuery();

            }
            conection.Close();
        }

        private bool getMeIfVehicleExist(SqlCommand command, int id)
        {
            string querry = "SELECT COUNT" +
                            "(" +
                                "enrollmentId" +
                            ") " +
                            "FROM " +
                                "vehicle " +
                            "WHERE " +
                                "enrollmentId = @id";

            command = new SqlCommand(querry, command.Connection);
            command.Parameters.AddWithValue("@id", id);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count == 0) return false;
            else return true;
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

                //------------------------------------------------------------------------------------------
                String getFromWheel = "SELECT  pressure FROM wheel WHERE id =" + Convert.ToInt32(reader["id"]);
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
                //------------------------------------------------------------------------------------------
                String getFromDoor = "SELECT  isOpen FROM door WHERE id =" + Convert.ToInt32(reader["id"]);
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

                //------------------------------------------------------------------------------------------
                connection.Close();
            }

            //------------------------------------------------------------------------------------------
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

            private string getMeQuerry(IEnumerable<string> filters)
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


        }

    }
}
