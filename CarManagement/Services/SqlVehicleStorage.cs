using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using ToolBox;

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
                using (IDbConnection con = conOpen())
                {
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = "SELECT count(*) AS 'Count' FROM vehicle";
                        IDataReader reader = sentence.ExecuteReader();
                        reader.Read();
                        int count = Convert.ToInt32(reader["Count"]);
                        return count;
                    }

                }

            }
        }

        public void clear()
        {
            using (IDbConnection con = conOpen())
            {
                using (IDbCommand sentence = con.CreateCommand())
                {
                    sentence.CommandText = "USE Carmanagement;" +
                        "DELETE FROM door;" +
                        "DELETE FROM wheel;" +
                        "DELETE FROM vehicle;" +
                        "DELETE FROM enrollment;";
                    sentence.ExecuteNonQuery();
                }

            }

        }

        public void Dispose()
        {
            IDbConnection con = conOpen();
                con.Dispose();
        }

        private IDbConnection conOpen()
        {
            IDbConnection con = new SqlConnection(this.connectionString);
            //IDbConnection con = new SqlConnection(this.connectionString);
            con.Open();
            return con;
        }
        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }

        public void set(IVehicle vehicle)
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                con.Open();
                using (IDbCommand sentence = con.CreateCommand())
                {
                    IDbDataParameter parameter = sentence.CreateParameter();
                    parameter.ParameterName = "@serial";
                    parameter.Value = vehicle.Enrollment.Serial;
                    sentence.Parameters.Add(parameter);
                    parameter = sentence.CreateParameter();
                    parameter.ParameterName = "@number";
                    parameter.Value = vehicle.Enrollment.Number;
                    sentence.Parameters.Add(parameter);
                    sentence.CommandText = @"USE CarManagement; 
                    SELECT id FROM enrollment 
                    WHERE serial = @serial AND number = @number";
                    if (sentence.ExecuteNonQuery() > 0)
                    {
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            reader.Read();
                            parameter = sentence.CreateParameter();
                            parameter.ParameterName = "@id";
                            parameter.Value = (int)reader["id"];
                            sentence.Parameters.Add(parameter);
                            reader.Close();
                            sentence.CommandText = @"SELECT * FROM vehicle
                            WHERE enrollmentId = @id";
                            int query = sentence.ExecuteNonQuery();
                            using (IDataReader reader2 = sentence.ExecuteReader())
                            {
                                reader2.Read();
                                parameter = sentence.CreateParameter();
                                parameter.ParameterName = "@color";
                                parameter.Value = (int)vehicle.Color;
                                sentence.Parameters.Add(parameter);

                                parameter = sentence.CreateParameter();
                                parameter.ParameterName = "@engineIsStarted";
                                parameter.Value = vehicle.Engine.IsStarted ? 1 : 0;
                                sentence.Parameters.Add(parameter);

                                parameter = sentence.CreateParameter();
                                parameter.ParameterName = "@engineHorsePower";
                                parameter.Value = vehicle.Engine.HorsePower;
                                sentence.Parameters.Add(parameter);
                                reader.Close();

                                if (query > 0)
                                {
                                    sentence.CommandText = @"UPDATE vehicle
                                    SET color = @color, 
                                        engineHorsePower = @engineHorsePower, 
                                        engineIsStarted = @engineIsStarted
                                        WHERE enrollmentId = @id";
                                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    sentence.CommandText = @"DELETE FROM wheel
                                    WHERE enrollmentId = @id";
                                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    IWheel[] wheels = vehicle.Wheels;
                                    foreach (IWheel wheel in wheels)
                                    {
                                        if (sentence.Parameters.Contains("@pressure"))
                                        {
                                            sentence.Parameters.Remove("@pressure");
                                        }
                                        parameter = sentence.CreateParameter();
                                        parameter.ParameterName = "@pressure";
                                        parameter.Value = wheel.Pressure;
                                        sentence.Parameters.Add(parameter);
                                        sentence.CommandText = @"INSERT INTO wheel(enrollmentId, pressure)
                                        VALUES (@id, @pressure)";
                                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    }
                                    sentence.CommandText = @"DELETE FROM door
                                    WHERE enrollmentId = @id";
                                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    IDoor[] doors = vehicle.Doors;
                                    foreach (IDoor door in doors)
                                    {
                                        if (sentence.Parameters.Contains("@isOpen"))
                                        {
                                            sentence.Parameters.Remove("@isOpen");
                                        }
                                        parameter = sentence.CreateParameter();
                                        parameter.ParameterName = "@isOpen";
                                        parameter.Value = door.IsOpen;
                                        sentence.Parameters.Add(parameter);
                                        sentence.CommandText = @"INSERT INTO door(enrollmentId, isOpen)
                                        VALUES (@id, @isOpen)";
                                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    }
                                }
                                else
                                {
                                    sentence.CommandText = @"INSERT INTO vehicle(enrollmwntId, 
                                    color, engineHorsePower, engineIsStarted)
                                    VALUES (@id, @color, @engineHorsePower, @engineIsStarted)";

                                    Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    IWheel[] wheels = vehicle.Wheels;
                                    foreach (IWheel wheel in wheels)
                                    {
                                        if (sentence.Parameters.Contains("@pressure"))
                                        {
                                            sentence.Parameters.Remove("@pressure");
                                        }
                                        parameter = sentence.CreateParameter();
                                        parameter.ParameterName = "@pressure";
                                        parameter.Value = wheel.Pressure;
                                        sentence.Parameters.Add(parameter);
                                        sentence.CommandText = @"INSERT INTO wheel(enrollmentId, pressure)
                                        VALUES (@id, @pressure)";
                                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    }
                                    IDoor[] doors = vehicle.Doors;
                                    foreach (IDoor door in doors)
                                    {
                                        if (sentence.Parameters.Contains("@isOpen"))
                                        {
                                            sentence.Parameters.Remove("@isOpen");
                                        }
                                        parameter = sentence.CreateParameter();
                                        parameter.ParameterName = "@isOpen";
                                        parameter.Value = door.IsOpen;
                                        sentence.Parameters.Add(parameter);
                                        sentence.CommandText = @"INSERT INTO door(enrollmentId, isOpen)
                                        VALUES (@id, @isOpen)";
                                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        sentence.CommandText = @"USE CarManagement;
                        INSERT INTO enrollment(serial, number)
                        VALUES (@serial, @number)";
                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                        sentence.CommandText = @"SELECT * FROM enrollment
                        WHERE serial = @serial AND number = @number";
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            reader.Read();
                            parameter = sentence.CreateParameter();
                            parameter.ParameterName = "@id";
                            parameter.Value = (int)reader["id"];
                            sentence.Parameters.Add(parameter);
                            reader.Close();

                            parameter = sentence.CreateParameter();
                            parameter.ParameterName = "@color";
                            parameter.Value = (int)vehicle.Color;
                            sentence.Parameters.Add(parameter);

                            parameter = sentence.CreateParameter();
                            parameter.ParameterName = "@engineHorsePower";
                            parameter.Value = vehicle.Engine.HorsePower;
                            sentence.Parameters.Add(parameter);

                            parameter = sentence.CreateParameter();
                            parameter.ParameterName = "@engineIsStarted";
                            parameter.Value = Convert.ToInt16(vehicle.Engine.IsStarted);
                            sentence.Parameters.Add(parameter);

                            sentence.CommandText = @"INSERT INTO vehicle(enrollmentId, 
                            color, engineHorsePower, engineIsStarted)
                            VALUES (@id, @color, @engineHorsePower, @engineIsStarted)";
                            sentence.ExecuteNonQuery();
                            IWheel[] wheels = vehicle.Wheels;
                            foreach (IWheel wheel in wheels)
                            {
                                if (sentence.Parameters.Contains(parameter))
                                {
                                    sentence.Parameters.Remove(parameter);
                                }
                                parameter = sentence.CreateParameter();
                                parameter.ParameterName = "@pressure";
                                parameter.Value = wheel.Pressure;
                                sentence.Parameters.Add(parameter);
                                sentence.CommandText = @"INSERT INTO wheel(vehicleId, pressure)
                                        VALUES (@id, @pressure)";
                                Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                            }
                            IDoor[] doors = vehicle.Doors;
                            foreach (IDoor door in doors)
                            {
                                if (sentence.Parameters.Contains(parameter))
                                {
                                    sentence.Parameters.Remove(parameter);
                                }
                                parameter = sentence.CreateParameter();
                                parameter.ParameterName = "@isOpen";
                                parameter.Value = door.IsOpen;
                                sentence.Parameters.Add(parameter);
                                sentence.CommandText = @"INSERT INTO door(vehicleId, isOpen)
                                        VALUES (@id, @isOpen)";
                                Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                            }
                        }
                    }
                }
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

                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = SELECT_VEHICLE_HEAD;
                        foreach (var item in this.queryParameters.Keys)
                        {
                            object type;
                            string data = this.queryParameters.ContainsKey(nameof(item)).ToString();
                            string dataType = this.Type.ContainsKey(nameof(item)).ToString();
                            if (dataType == "int")
                            {
                                type = SqlDbType.Int;
                            }
                            else
                            {
                                type = SqlDbType.VarChar;
                            }
                            IDbDataParameter parameter = sentence.CreateParameter();
                            parameter.ParameterName = $"{item}";
                            parameter.Value = data;
                            sentence.Parameters.Add(parameter);
                        }

                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IDbDataParameter parameter = sentence.CreateParameter();
                                parameter.ParameterName = "@ID";
                                parameter.Value = (int)reader["id"];
                                sentence.Parameters.Add(parameter);
                                int id = (int)reader["id"];
                                enrollmentDto.Serial = reader["serial"].ToString();
                                enrollmentDto.Number = Convert.ToInt32(reader["number"]);
                                CarColor color = (CarColor)Enum.Parse(typeof(CarColor), reader["color"].ToString());
                                engineDto.IsStarted = Convert.ToBoolean(reader["engineIsStarted"]);
                                engineDto.HorsePower = Convert.ToInt32(reader["engineHorsePower"]);
                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    sentence2.CommandText = SELECT_WHEELS;
                                    parameter = sentence2.CreateParameter();
                                    parameter.ParameterName = "@ID";
                                    parameter.Value = id;
                                    sentence2.Parameters.Add(parameter);

                                    using (IDataReader reader2 = sentence2.ExecuteReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            WheelDto wheelDto = new WheelDto();
                                            wheelDto.Pressure = Convert.ToDouble(reader2["pressure"]);
                                            wheelsDto.Add(wheelDto);
                                        }
                                    }
                                }
                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    sentence2.CommandText = SELECT_DOORS;
                                    parameter = sentence2.CreateParameter();
                                    parameter.ParameterName = "@ID";
                                    parameter.Value = (int)reader["id"];
                                    sentence2.Parameters.Add(parameter);

                                    using (IDataReader reader2 = sentence2.ExecuteReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            DoorDto doorDto = new DoorDto();
                                            doorDto.IsOpen = Convert.ToBoolean(reader2["isOpen"]);
                                            doorsDto.Add(doorDto);
                                        }
                                    }
                                }
                                vehicleDto.Color = color;
                                vehicleDto.Doors = doorsDto.ToArray();
                                vehicleDto.Wheels = wheelsDto.ToArray();
                                vehicleDto.Engine = engineDto;
                                vehicleDto.Enrollment = enrollmentDto;
                                IVehicle vehicle_get = this.vehicleBuilder.import(vehicleDto);

                                yield return vehicle_get;
                            }
                        }
                    }
                }
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
            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                throw new NotImplementedException();
            }

        }
    }
}
