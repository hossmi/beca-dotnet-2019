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

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count {
            get
            {
                SqlConnection con = conOpen();
                String query;
                query = "SELECT count(*) AS 'Count' FROM vehicle";
                SqlCommand sentence = new SqlCommand(query, con);
                SqlDataReader reader = sentence.ExecuteReader();
                reader.Read();
                int count = Convert.ToInt32(reader["Count"]);
                return count;
            }
        }

        public void clear()
        {
            SqlConnection con = conOpen();
            String query;
            query = "USE Carmanagement;" +
                "DELETE FROM door;" +
                "DELETE FROM wheel;" + 
                "DELETE FROM vehicle;" + 
                "DELETE FROM enrollment;";
            SqlCommand sentence = new SqlCommand(query, con);
            sentence.ExecuteNonQuery();
        }

        public void Dispose()
        {
            SqlConnection con;
            con = new SqlConnection(this.connectionString);
            con.Dispose();
        }

        private SqlConnection conOpen()
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();
            return con;
        }
        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }

        public void set(IVehicle vehicle)
        {
            string serial = vehicle.Enrollment.Serial.ToString();
            int number = vehicle.Enrollment.Number;
            SqlConnection con;
            con = new SqlConnection(this.connectionString);
            con.Open();
            string query;
            query = "USE CarManagement;" +
                "SELECT count(*) AS 'count' FROM enrollment " +
                "WHERE serial = '" + serial + "' AND number = " + number;
            SqlCommand sentence = new SqlCommand(query, con);
            SqlDataReader reader = sentence.ExecuteReader();
            reader.Read();
            int count = Convert.ToInt32(reader["count"]);
            if (count > 0)
            {
                query = "SELECT id " +
                    "FROM [enrollment]" +
                    "WHERE (serial = '" + serial + "' AND number = " + number + ")";
                sentence = new SqlCommand(query, con);
                reader = sentence.ExecuteReader();
                reader.Read();
                int ressult = (int)reader["id"];
                reader.Close();

                int color_int = (int)vehicle.Color;
                int engineIsStarted = vehicle.Engine.IsStarted ? 1 : 0;

                int engineHorsePower = vehicle.Engine.HorsePower;
                query = "UPDATE vehicle" +
                    "SET enrollmentId = " + ressult + ", color = " + color_int + ", engineHorsePower = " + engineHorsePower + ", engineIsStarted = " + engineIsStarted;
                sentence = new SqlCommand(query, con);
                sentence.ExecuteNonQuery();

                IWheel[] wheels = vehicle.Wheels;
                foreach (IWheel wheel in wheels)
                {
                    double pressure = wheel.Pressure;
                    query = "UPDATE wheel" +
                        "SET pressure = " + pressure;
                    sentence = new SqlCommand(query, con);
                    sentence.ExecuteNonQuery();
                }

                IDoor[] doors = vehicle.Doors;
                foreach (IDoor door in doors)
                {
                    string isOpen = (door.IsOpen ? 1 : 0).ToString();
                    query = "UPDATE door" +
                        "SET vehicleId = " + ressult + ", isOpen = '" + isOpen + "'";
                    sentence = new SqlCommand(query, con);
                    sentence.ExecuteNonQuery();

                }
                con.Close();
            }
            else
            {
                query = "USE [CarManagement]" +
                "INSERT INTO [enrollment] (serial, number) " +
                "VALUES ('" + serial + "', " + number + ")";
                sentence = new SqlCommand(query, con);
                sentence.ExecuteNonQuery();

                query = "SELECT id " +
                    "FROM [enrollment]" +
                    "WHERE (serial = '" + serial + "' AND number = " + number + ")";
                sentence = new SqlCommand(query, con);
                reader = sentence.ExecuteReader();
                reader.Read();
                int ressult = (int)reader["id"];
                reader.Close();

                int color_int = (int)vehicle.Color;
                int engineIsStarted = vehicle.Engine.IsStarted ? 1 : 0;

                int engineHorsePower = vehicle.Engine.HorsePower;
                query = "INSERT INTO [vehicle] (enrollmentId, color, engineHorsePower, engineIsStarted) " +
                    "VALUES (" + ressult + ", " + color_int + ", " + engineHorsePower + ", " + engineIsStarted + ")";
                sentence = new SqlCommand(query, con);
                sentence.ExecuteNonQuery();

                IWheel[] wheels = vehicle.Wheels;
                foreach (IWheel wheel in wheels)
                {
                    string pressure = wheel.Pressure.ToString().Replace(",", ".");
                    query = "INSERT INTO wheel (vehicleId, pressure) " +
                        "VALUES (" + ressult + ", " + pressure + ")";
                    sentence = new SqlCommand(query, con);
                    sentence.ExecuteNonQuery();
                }

                IDoor[] doors = vehicle.Doors;
                foreach (IDoor door in doors)
                {
                    string isOpen = (door.IsOpen ? 1 : 0).ToString();
                    query = "INSERT INTO door (vehicleId, isOpen) " +
                        "VALUES (" + ressult + ", " + isOpen + ")";
                    sentence = new SqlCommand(query, con);
                    sentence.ExecuteNonQuery();

                }
                con.Close();
            }
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private CarColor color;
            private bool engineIsStarted;
            private IEnrollment enrollment;
            private string enrollmentSerial;
            private int horsePower;
            private int min;
            private int max;
            private IDictionary<string, object> queryParameters;
            private IDictionary<string, string> queryParts;
            private IDictionary<string, string> Type;
            private const string SELECT_VEHICLE_HEAD = @"
                USE CarManagement; 
                SELECT e.serial, 
                    e.number, 
                    e.id, 
                    v.color, 
                    v.engineIsStarted, 
                    v.engineHorsePower 
                FROM enrollment e 
                INNER JOIN vehicle v ON e.id = v.enrollmentId ";
            private const string SELECT_WHEELS = @"
                    USE CarManagement;
                    SELECT pressure 
                    FROM wheel 
                    WHERE vehicleId = @ID";
            private const string SELECT_DOORS = @"
                    USE CarManagement; 
                    SELECT isOpen 
                    FROM door 
                    WHERE vehicleId = @ID";

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.queryParts = new Dictionary<string, string>();
                this.queryParameters = new Dictionary<string, object>();
                this.Type = new Dictionary<string, string>();
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                Asserts.isEnumDefined(color);
                this.queryParameters.Add("@color", (int)color);
                this.queryParts.Add("color"," color = " + (int)color);
                this.color = color;
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                this.queryParameters.Add("@engineIsStarted", started);
                //this.queryParts.Add("engineIsStarted", " engineIsStarted = " + Convert.ToUInt16(started));
                this.queryParts.Add("@engineIsStarted", " engineIsStarted = @engineIsStarted");
                this.Type.Add("@engineIsStarted", "int");
                this.engineIsStarted = started;
                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                this.queryParameters.Add("@number", enrollment.Number);
                //this.queryParts.Add("number", "number = " + enrollment.Number);
                //this.queryParts.Add("serial", "serial = " + enrollment.Serial);
                this.queryParts.Add("@number", "number = @number");
                this.queryParts.Add("@serial", "serial = @serial");
                this.Type.Add("@engineIsStarted", "int");
                this.enrollment = enrollment;
                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.enrollmentSerial = serial;
                this.queryParameters.Add("@serial", serial);
                //this.queryParts.Add("serial", "serial = " + serial);
                this.queryParts.Add("@serial", "serial = @serial");
                this.Type.Add("@serial", "string");
                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                this.queryParameters.Add("@engineHorsePower", horsePower);
                //this.queryParts.Add("engineHorsePower", "engineHorsePower = " + horsePower);
                this.queryParts.Add("@engineHorsePower", "engineHorsePower = @engineHorsePower");
                this.horsePower = horsePower;
                this.Type.Add("@engineHorsePower", "int");
                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min > 0);
                Asserts.isTrue(max > 0);
                this.queryParameters.Add("@min", min);
                this.queryParameters.Add("@max", max);
                //this.queryParts.Add("engineHorsePower", "engineHorsePower BETWEEN " + min + " AND " + max);
                this.queryParts.Add("@engineHorsePower", "engineHorsePower BETWEEN @min AND @max");
                this.Type.Add("@min", "int");
                this.Type.Add("@max", "int");
                this.max = max;
                this.min = min;
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }

            private IEnumerator<IVehicle> enumerate()
            {
                string query = createQuery(SELECT_VEHICLE_HEAD, this.queryParts);

                VehicleDto vehicleDto = new VehicleDto();
                EnrollmentDto enrollmentDto = new EnrollmentDto();
                EngineDto engineDto = new EngineDto();
                List<WheelDto> wheelsDto = new List<WheelDto>();
                List<DoorDto> doorsDto = new List<DoorDto>();
                SqlCommand sentence = genericSentence(query, this.connectionString);
                SqlDataReader reader = sentence.ExecuteReader();

                while (reader.Read())
                {
                    sentence.Parameters.Add("@ID", SqlDbType.Int);
                    sentence.Parameters["@ID"].Value = (int)reader["id"];
                    int id = (int)reader["id"];
                    enrollmentDto.Serial = reader["serial"].ToString();
                    enrollmentDto.Number = Convert.ToInt32(reader["number"]);
                    CarColor color = (CarColor)Enum.Parse(typeof(CarColor), reader["color"].ToString());
                    engineDto.IsStarted = Convert.ToBoolean(reader["engineIsStarted"]);
                    engineDto.HorsePower = Convert.ToInt32(reader["engineHorsePower"]);

                    SqlCommand sentence2 = genericSentence(SELECT_WHEELS, this.connectionString);
                    sentence2.Parameters.Add("@ID", SqlDbType.Int);
                    sentence2.Parameters["@ID"].Value = id;
                    SqlDataReader reader2 = sentence2.ExecuteReader();

                    while (reader2.Read())
                    {
                        WheelDto wheelDto = new WheelDto();
                        wheelDto.Pressure = Convert.ToDouble(reader2["pressure"]);
                        wheelsDto.Add(wheelDto);
                    }

                    reader2.Close();

                    sentence2 = genericSentence(SELECT_DOORS, this.connectionString);
                    sentence2.Parameters.Add("@ID", SqlDbType.Int);
                    sentence2.Parameters["@ID"].Value = sentence.Parameters["@ID"].Value;
                    reader2 = sentence2.ExecuteReader();

                    while (reader2.Read())
                    {
                        DoorDto doorDto = new DoorDto();
                        doorDto.IsOpen = Convert.ToBoolean(reader2["isOpen"]);
                        doorsDto.Add(doorDto);
                    }

                    reader2.Close();

                    vehicleDto.Color = color;
                    vehicleDto.Doors = doorsDto.ToArray();
                    vehicleDto.Wheels = wheelsDto.ToArray();
                    vehicleDto.Engine = engineDto;
                    vehicleDto.Enrollment = enrollmentDto;
                    IVehicle vehicle_get = this.vehicleBuilder.import(vehicleDto);

                    yield return vehicle_get;
                }
                reader.Close();
            }

            private SqlCommand genericSentence(string query, string con)
            {
                SqlCommand sentence = new SqlCommand(query, connection(con));
                //sentence.setParameters()
                foreach (var item in this.queryParameters.Keys)
                {
                    object var;
                    string data = this.queryParameters.ContainsKey(nameof(item)).ToString();
                    string dataType = this.Type.ContainsKey(nameof(item)).ToString();
                    if(dataType == "int")
                    {
                        var = SqlDbType.Int;
                    }
                    else 
                    {
                        var = SqlDbType.VarChar;
                    }
                    sentence.Parameters.AddWithValue($"{data}", var);
                }
                return sentence;
            }

            private SqlConnection connection(string connection)
            {
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                return con;
            }
            private static string createQuery(string query, IDictionary<string, string> conditionParts)
            {
                int counter = 0;

                foreach (string conditionPart in conditionParts.Values)
                {
                    if (counter == 0)
                    {
                        query = " WHERE " + conditionPart;
                    }
                    else
                    {
                        query += " AND " + conditionPart;
                    }
                    counter++;
                }

                return query;
            }
        }
    }
}
