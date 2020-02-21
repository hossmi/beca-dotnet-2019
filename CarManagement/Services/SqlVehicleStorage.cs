using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using ToolBox.Extensions.DbCommands;
using static ToolBox.Services.QueryBuilder;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private int id;
        private const string COUNT_VEHICLE = @"USE CarManagement
            SELECT count(enrollmentId) AS 'Count' FROM vehicle";

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count
        {
            get
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = COUNT_VEHICLE;
                        this.id = (int)sentence.ExecuteScalar();
                    }
                    con.Close();
                    return this.id;
                }

            }
        }
        public void clear()
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                con.Open();
                using (IDbCommand sentence = con.CreateCommand())
                {
                    remover(sentence);
                }
                con.Close();
            }
        }
        public void Dispose()
        {
            SqlConnection.ClearAllPools();
        }
        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }
        public void remove(IEnrollment enrollment)
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                con.Open();
                using (IDbCommand sentence = con.CreateCommand())
                {
                    setParameters(sentence, new List<Param>() { new Param("serial", enrollment.Serial), new Param("number", enrollment.Number) });
                    string id = check_tag_name("id", "enrollment");
                    sentence.CommandText = $"{select()} {from("enrollment")} {equal(id, $"@id")}";
                    if (sentence.ExecuteScalar() != null)
                    {
                        remover(sentence, id, "@id");
                    }
                }
                con.Close();
            }
        }
        public void set(IVehicle vehicle)
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                con.Open();
                using (IDbCommand sentence = con.CreateCommand())
                {
                    if (enrollmentExist(sentence, vehicle) != null)
                    {
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            reader.Read();
                            this.id = (int)reader.GetValue(0);
                            sentence.Parameters.Add(SetParameter(sentence, new Param(reader.GetName(0), reader.GetValue(0))));
                            reader.Close();
                        }

                        setParameters(sentence, new List<Param>() { new Param("color", (int)vehicle.Color), new Param("engineIsStarted", vehicle.Engine.IsStarted ? 1 : 0), new Param("engineHorsePower", vehicle.Engine.HorsePower) });
                        if (vehicleExist(sentence) != null)
                        {
                            string id = check_tag_name("id", "vehicle");
                            sentence.CommandText = $@"
                            {update("vehicle")} 
                            {setData(new List<FieldValue>() { new FieldValue() { field = "color", value = "@color" }, new FieldValue() { field = "engineIsStarted", value = "@engineIsStarted" }, new FieldValue() { field = "engineHorsePower", value = "@engineHorsePower" }})}
                            {where()} {equal(id, "@id")}";

                            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                            id = check_tag_name("id", "wheel");
                            delete(sentence, $"{delete("wheel", id, "@id")} {delete("door", id, "@id")}");
                            insertwheelsdoors(vehicle, sentence, this.id);
                        }
                        else
                        {
                            insertVehicle(sentence, vehicle, this.id);
                        }
                    }
                    else
                    {
                        sentence.CommandText = $"{insert("enrollment")} {setData(string.Join(", ", new List<string>() { "serial", "number" }), "OUTPUT INSERTED.ID", string.Join(", ", new List<string>() { "@serial", "@number" }))}";
                        this.id = (int)sentence.ExecuteScalar();
                        sentence.Parameters.Add(SetParameter(sentence, new Param("id", this.id)));
                        insertVehicle(sentence, vehicle, this.id);
                    }
                }
            }
        }

        private static IDataParameter SetParameter(IDbCommand sentence, Param param)
        {
            IDataParameter parameter = sentence.CreateParameter();
            parameter.ParameterName = param.Name;
            parameter.Value = param.Value;
            return parameter;
        }
        private IList<string> parameterNames(IList<Param> parameters)
        {
            IList<string> columnsValues = new List<string>();
            foreach (Param parameter in parameters)
            {
                columnsValues.Add(parameter.Name);
            }
            return columnsValues;
        }
        private void setParameters(IDbCommand sentence, IList<Param> parameters)
        {
            foreach (Param parameter in parameters)
            {
                sentence.Parameters.Add(SetParameter(sentence, parameter));
            }
        }
        private string enrollmentExist(IDbCommand sentence, IVehicle vehicle)
        {
            string response = "OK";
            setParameters(sentence, new List<Param>() { new Param("serial", vehicle.Enrollment.Serial), new Param("number", vehicle.Enrollment.Number) });
            sentence.CommandText = $"{select("id")} {from("enrollment")} {where()} {and_or(equal("serial", "@serial"), "AND", equal("number", "@number"))}";
            if (sentence.ExecuteScalar() == null)
            {
                response = null;
            }
            return response;
        }
        private void insertVehicle(IDbCommand sentence, IVehicle vehicle, int id)
        {
            sentence.Parameters.Clear();
            IList<Param> parameters = new List<Param>()
            {
                new Param("id", id),
                new Param("color", (int)vehicle.Color),
                new Param("engineIsStarted", vehicle.Engine.IsStarted ? 1 : 0),
                new Param("engineHorsePower", vehicle.Engine.HorsePower)
            };
            setParameters(sentence, parameters);
            sentence.CommandText = $"{insert("vehicle")} {setData(fields_values(new List<string>() { "@id", "@color", "@engineHorsePower", "@engineIsStarted" }))}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            insertwheelsdoors(vehicle, sentence, id);
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private void insertwheelsdoors(IVehicle vehicle, IDbCommand sentence, int id)
        {
            foreach (IWheel wheel in vehicle.Wheels)
            {
                makeWheelDoor(sentence, new List<Param>() { new Param("id", id), new Param("pressure", wheel.Pressure) }, "wheel");
            }
            foreach (IDoor door in vehicle.Doors)
            {
                makeWheelDoor(sentence, new List<Param>() { new Param("id", id), new Param("isOpen", door.IsOpen) }, "door");
            }
        }
        private void makeWheelDoor(IDbCommand sentence, IList<Param> parameters, string table)
        {
            sentence.Parameters.Clear();
            setParameters(sentence, parameters);
            IList<string> values = new List<string>();
            IList<string> fields = new List<string>();

            foreach (string value in parameterNames(parameters))
            {
                values.Add($"{element(value, table, "value")}");
                fields.Add($"{element(value, table, "condition")}");
            }
            sentence.CommandText = $"{insert(table)} {setData(fields_values(fields), fields_values(values))}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private string vehicleExist(IDbCommand sentence)
        {
            string response = "OK";
            string id = check_tag_name("enrollment", "id");
            sentence.CommandText = $"{select()} {from("enrollment")} {where()} {equal("id", "@id")}";
            if (sentence.ExecuteScalar() == null)
            {
                response = null;
            }
            return response;
        }
        private static void remover(IDbCommand sentence, string field, string value)
        {
            sentence.CommandText = $"{delete("wheel", field, value)} {delete("door", field, value)} {delete("vehicle", field, value)}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private static void remover(IDbCommand sentence)
        {
            sentence.CommandText = $"{delete("wheel")} {delete("door")} {delete("vehicle")}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private static void delete(IDbCommand sentence, string order)
        {
            sentence.CommandText = $"{order}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private static string delete(string table_name)
        {
            return $"DELETE {from(table_name)};";
        }
        private static string delete(string table_name, string field, string value)
        {
            return $"DELETE {from(table_name)} {where()} {equal(field, value)};";
        }

        private class Param
        {
            public Param(string Param_name, object Param_value)
            {
                this.Name = Param_name;
                this.Var = $"@{Param_name}";
                this.Value = Param_value;
            }
            public string Name { get; }
            public string Var { get; }
            public object Value { get; }
        }
        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IEnrollmentProvider enrollmentProvider;
            private readonly IDictionary<string, object> queryParameters;
            private readonly IList<string> where;
            private IList<WheelDto> wheelsDto;
            private IList<DoorDto> doorsDto;
            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.enrollmentProvider = new DefaultEnrollmentProvider();
                this.queryParameters = new Dictionary<string, object>();
                this.where = new List<string>();
            }
            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = $"{select()} {from("enrollment")}";
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                yield return this.enrollmentProvider
                                    .import(
                                    reader.GetValue(0).ToString(),
                                    Convert.ToInt16(reader.GetValue(1)));
                            }
                        }
                    }
                }

            }
            public IEnumerable<IEnrollment> Keys
            {
                get
                {
                    return enumerateEnrollments();
                }
            }
            public IVehicleQuery whereColorIs(CarColor color)
            {
                Asserts.isEnumDefined(color);

                this.queryParameters.Add("@color", (int)color);
                this.where.Add(equal("color", "@color"));
                return this;
            }
            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                this.queryParameters.Add("@engineIsStarted", started);
                this.where.Add(equal("engineIsStarted", "@engineIsStarted"));
                return this;
            }
            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                this.where.Add(equal("number", "@number"));
                this.queryParameters.Add("@number", enrollment.Number);
                this.queryParameters.Add("@serial", enrollment.Serial);
                this.where.Add(equal("serial", "@serial"));
                return this;
            }
            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.queryParameters.Add("@serial", serial);
                this.where.Add(equal("serial", "@serial"));
                return this;
            }
            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                this.queryParameters.Add("@engineHorsePower", horsePower);
                this.where.Add(equal("engineHorsePower", "@engineHorsePower"));
                return this;
            }
            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min > 0);
                Asserts.isTrue(max > 0);
                this.where.Add(equal("engineHorsePower", between("@min", "@max")));
                this.queryParameters.Add("@min", min);
                this.queryParameters.Add("@max", max);
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }
            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }
            private IEnumerator<IVehicle> enumerate()
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = $@"
                            {select(new List<string>() { "e.serial", "e.number", "e.id", "v.color", "v.engineIsStarted", "v.engineHorsePower" })}
                            {from("enrollment e")} {inner_join("vehicle v")} {on()} {condition_value("id", "enrollmentId")}";

                        if (this.where.Count > 0)
                        {
                            sentence.CommandText += $" {where()} {string.Join(" AND ", this.where)}";
                        }
                        DBCommandExtensions.setParameters(sentence, this.queryParameters);
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = (int)reader.GetValue(2);
                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    if (sentence2.Parameters == null)
                                    {
                                        foreach (IDbDataParameter parameter in sentence.Parameters)
                                        {
                                            sentence2.Parameters.Add(parameter);
                                        }
                                    }
                                    this.doorsDto = new List<DoorDto>();
                                    this.wheelsDto = new List<WheelDto>();
                                    sentence2.Parameters.Add(SetParameter(sentence, new Param("id", id)));
                                    string key = check_tag_name("id", "door");
                                    sentence2.CommandText = $"{select("isOpen")} {from("door")} {where()} {equal(key, "@id")}";
                                    elements(sentence2, "door");
                                    sentence2.CommandText = $"{select("pressure")} {from("wheel")} {where()} {equal(key, "@id")}";
                                    elements(sentence2, "wheel");
                                }
                                yield return this.vehicleBuilder.import(
                                    new VehicleDto(
                                        (CarColor)Enum.Parse(typeof(CarColor),
                                        reader.GetValue(3).ToString()),
                                        new EngineDto(
                                            Convert.ToInt32(reader.GetValue(5)),
                                            Convert.ToBoolean(reader.GetValue(4))),
                                        new EnrollmentDto(
                                            reader.GetValue(0).ToString(),
                                            Convert.ToInt32(reader.GetValue(1))),
                                        this.wheelsDto.ToArray(),
                                        this.doorsDto.ToArray()));
                            }
                            reader.Close();
                        }
                    }
                }
            }
            void elements(IDbCommand sentence2, string type)
            {
                using (IDataReader reader2 = sentence2.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        if (type == "door")
                        {
                            this.doorsDto.Add(new DoorDto(Convert.ToBoolean(reader2.GetValue(0))));
                        }
                        else
                        {
                            this.wheelsDto.Add(new WheelDto(Convert.ToDouble(reader2.GetValue(0))));
                        }
                    }
                }
            }
        }
    }
}