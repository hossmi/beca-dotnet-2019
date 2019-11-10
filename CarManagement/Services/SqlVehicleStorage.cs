using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using ToolBox.Extensions.DbCommands;
using ToolBox.Services;
using static ToolBox.Services.QueryBuilder;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private int id;
        private QueryBuilder queryBuilder;
        private List<whereFieldValues> whereValues;
        const string COUNT_VEHICLE = @"USE CarManagement
            SELECT count(enrollmentId) AS 'Count' FROM vehicle";

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count {
            get
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = COUNT_VEHICLE;
                        return (int)sentence.ExecuteScalar();
                    }

                }

            }
        }
        public void clear()
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                using (IDbCommand sentence = con.CreateCommand())
                {
                    con.Open();
                    sentence.CommandText = $"{removeQuery()}";
                    sentence.ExecuteNonQuery();
                    con.Close();
                }
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
                using (IDbCommand sentence = con.CreateCommand())
                {
                    con.Open();
                    setParameters2(sentence, new List<Param>(){new Param("serial", enrollment.Serial), new Param("number", enrollment.Number)});
                    this.queryBuilder = new QueryBuilder(new iQuery(){tablesColumns = new List<FieldValues>(){new FieldValues(){field = "enrollment", values = new List<string>(){"serial", "number"}}}, whereValues = new List<whereFieldValues>(){WhereParam("=", "id")}});
                    sentence.CommandText = $"{this.queryBuilder.select()}";
                    if (sentence.ExecuteScalar() != null)
                    {
                        sentence.CommandText = $"{removeQuery(new List<whereFieldValues>() { WhereParam("=", "id") })}";
                        sentence.ExecuteNonQuery();
                    }
                    con.Close();
                }
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
                        this.id = readEnrollmentId(sentence);
                        setParameters2(sentence, new List<Param>(){new Param("color", (int)vehicle.Color), new Param("engineIsStarted", vehicle.Engine.IsStarted ? 1 : 0), new Param("engineHorsePower", vehicle.Engine.HorsePower)});
                        if (vehicleExist(sentence) != null)
                        {
                            updateVehicle(sentence, vehicle);
                        }
                        else
                        {
                            insertVehicle(sentence, vehicle);
                        }
                    }
                    else
                    {
                        insertEnrollment(sentence);
                        insertVehicle(sentence, vehicle);
                    }
                }
            }
        }

        private string enrollmentExist(IDbCommand sentence, IVehicle vehicle)
        {
            string response = "OK";
            setParameters2(sentence, new List<Param>(){new Param("serial", vehicle.Enrollment.Serial), new Param("number", vehicle.Enrollment.Number)});
            this.queryBuilder = new QueryBuilder(new iQuery(){tablesColumns = new List<FieldValues>(){new FieldValues(){field = "enrollment", values = new List<string>(){"id"}}}, whereValues = new List<whereFieldValues>(){WhereParam("=", "serial"), WhereParam("=", "number")}});
            sentence.CommandText = $"{this.queryBuilder.select()}";
            if (sentence.ExecuteScalar() == null)
            {
                response = null;
            }
            return response;
        }

        private static IDataParameter insertParams(IDbCommand sentence, Param param, IList<string> columnsValues = null)
        {
            if (columnsValues != null)
            {
                columnsValues.Add(param.Name);
            }
            return SetParameter(sentence, param);
        }
        private static IDataParameter SetParameter(IDbCommand sentence, Param param)
        {
            IDataParameter parameter = sentence.CreateParameter();
            parameter.ParameterName = param.Name;
            parameter.Value = param.Value;
            return parameter;
        }
        private static IList<string> setParameters(IDbCommand sentence, IList<Param> parameters)
        {
            IList<string> columnsValues = new List<string>();
            foreach (Param parameter in parameters)
            {
                sentence.Parameters.Add(insertParams(sentence, parameter, columnsValues));
            }
            return columnsValues;
        }
        private static void setParameters2(IDbCommand sentence, IList<Param> parameters)
        {
            foreach (Param parameter in parameters)
            {
                sentence.Parameters.Add(insertParams(sentence, parameter));
            }
        }

        private void insertEnrollment(IDbCommand sentence)
        {
            this.queryBuilder = new QueryBuilder(new iQuery(){tablesColumns = new List<FieldValues>(){new FieldValues(){field = "enrollment", values = new List<string>(){"serial", "number"}, output = "OUTPUT INSERTED.ID"}}});
            sentence.CommandText = $"{this.queryBuilder.insert()}";
            this.id = (int)sentence.ExecuteScalar();
            sentence.Parameters.Add(SetParameter(sentence, new Param("id", this.id)));
        }
        private void insertVehicle(IDbCommand sentence, IVehicle vehicle)
        {
            sentence.Parameters.Clear();
            this.queryBuilder = new QueryBuilder(new iQuery(){tablesColumns = new List<FieldValues>(){new FieldValues(){field = "vehicle", values = setParameters(sentence, new List<Param>(){new Param("id", this.id), new Param("color", (int)vehicle.Color), new Param("engineIsStarted", vehicle.Engine.IsStarted ? 1 : 0), new Param("engineHorsePower", vehicle.Engine.HorsePower)})}}});
            sentence.CommandText = $"{this.queryBuilder.insert()}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            insertwheelsdoors(vehicle, sentence);
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private void insertwheelsdoors(IVehicle vehicle, IDbCommand sentence)
        {
            foreach (IWheel wheel in vehicle.Wheels)
                makeWheelDoor(sentence, new List<Param>(){new Param("id", this.id), new Param("pressure", wheel.Pressure)}, "wheel");
            foreach (IDoor door in vehicle.Doors)
                makeWheelDoor(sentence, new List<Param>(){new Param("id", this.id), new Param("isOpen", door.IsOpen)}, "door");
        }
        private void makeWheelDoor(IDbCommand sentence, IList<Param> parameters, string table)
        {
            sentence.Parameters.Clear();
            this.queryBuilder = new QueryBuilder(new iQuery(){tablesColumns = new List<FieldValues>(){new FieldValues(){field = table, values = setParameters(sentence, parameters)}}});
            sentence.CommandText = $"{this.queryBuilder.insert()}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }

        private void selectVehicle(IDbCommand sentence)
        {
            this.queryBuilder = new QueryBuilder(new iQuery(){tablesColumns = new List<FieldValues>(){new FieldValues(){field = "vehicle", values = new List<string>(){"*"}}}, whereValues = new List<whereFieldValues>{WhereParam("=", "id")}});
            sentence.CommandText = $"{this.queryBuilder.select()}";
        }
        private string vehicleExist(IDbCommand sentence)
        {
            string response = "OK";
            this.queryBuilder = new QueryBuilder(new iQuery(){tablesColumns = new List<FieldValues>(){new FieldValues(){field = "vehicle", values = new List<string>(){"*"}}}, whereValues = new List<whereFieldValues>{WhereParam("=", "id")}});
            sentence.CommandText = $"{this.queryBuilder.select()}";
            if (sentence.ExecuteScalar() == null)
            {
                response = null;
            }
            return response;
        }
        private static whereFieldValues WhereParam(string key, string param)
        {
            return new whereFieldValues() { field = param, values = new List<string> { $"@{param}" }, key = key };
        }
        private static int readEnrollmentId(IDbCommand sentence)
        {
            int id;
            using (IDataReader reader = sentence.ExecuteReader())
            {
                reader.Read();
                id = (int)reader.GetValue(0);
                sentence.Parameters.Add(SetParameter(sentence, new Param(reader.GetName(0), reader.GetValue(0))));
                reader.Close();
            }
            return id;
        }
        private void updateVehicle(IDbCommand sentence, IVehicle vehicle)
        {
            this.queryBuilder = new QueryBuilder(new iQuery(){tablesColumns = new List<FieldValues>(){new FieldValues(){field = "vehicle", values = new List<string>(){"color", "engineIsStarted", "engineHorsePower"}}}, whereValues = new List<whereFieldValues>(){WhereParam("=", "id")}});
            sentence.CommandText = $"{this.queryBuilder.update()}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            deleteWheelsDoors(sentence);
            insertwheelsdoors(vehicle, sentence);
        }
        private StringBuilder removeQuery(IList<whereFieldValues> wherevalues = null)
        {
            StringBuilder query = new StringBuilder();
            this.queryBuilder = new QueryBuilder(new iQuery(){tablesColumns = new List<FieldValues>(){new FieldValues(){field = "wheel"}}, whereValues = wherevalues});
            query.Insert(query.Length, $"{this.queryBuilder.delete()};");
            this.queryBuilder = new QueryBuilder(new iQuery(){tablesColumns = new List<FieldValues>(){new FieldValues(){field = "door"}}, whereValues = wherevalues});
            query.Insert(query.Length, $"{this.queryBuilder.delete()};");
            this.queryBuilder = new QueryBuilder(new iQuery(){tablesColumns = new List<FieldValues>(){new FieldValues(){field = "vehicle"}}, whereValues = wherevalues});
            query.Insert(query.Length, $"{this.queryBuilder.delete()};");
            return query;
        }
        private void deleteWheelsDoors(IDbCommand sentence)
        {
            this.whereValues = new List<whereFieldValues>(){WhereParam("=", "id")};
            this.queryBuilder = new QueryBuilder(new iQuery{whereValues = this.whereValues, tablesColumns = new List<FieldValues>(){new FieldValues(){field = "wheel"}}});
            sentence.CommandText = $"{this.queryBuilder.delete()}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            this.queryBuilder = new QueryBuilder(new iQuery{whereValues = this.whereValues, tablesColumns = new List<FieldValues>(){new FieldValues(){field = "door"}}});
            sentence.CommandText = $"{this.queryBuilder.delete()}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
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
            private QueryBuilder queryBuilder;
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IEnrollmentProvider enrollmentProvider;
            private readonly IDictionary<string, object> queryParameters;
            private readonly IList<whereFieldValues> whereValues;
            private IList<WheelDto> wheelsDto;
            private IList<DoorDto> doorsDto;
            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.enrollmentProvider = new DefaultEnrollmentProvider();
                this.queryParameters = new Dictionary<string, object>();
                this.whereValues = new List<whereFieldValues>();
            }
            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "enrollment", values = new List<string>() { "serial, number " } } } });
                        sentence.CommandText = $"{this.queryBuilder.select()}";
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
                this.whereValues.Add(WhereParam("=", "color"));
                this.queryParameters.Add("@color", (int)color);
                return this;
            }
            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                this.whereValues.Add(WhereParam("=", "engineIsStarted"));
                this.queryParameters.Add("@engineIsStarted", started);
                return this;
            }
            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                this.whereValues.Add(WhereParam("=", "number"));
                this.queryParameters.Add("@number", enrollment.Number);
                this.whereValues.Add(WhereParam("=", "serial"));
                this.queryParameters.Add("@serial", enrollment.Serial);
                return this;
            }
            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.whereValues.Add(WhereParam("=", "serial"));
                this.queryParameters.Add("@serial", serial);
                return this;
            }
            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                this.whereValues.Add(WhereParam("=", "engineHorsePower"));
                this.queryParameters.Add("@engineHorsePower", horsePower);
                return this;
            }
            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min > 0);
                Asserts.isTrue(max > 0);
                this.whereValues.Add
                (
                    new whereFieldValues()
                    {
                        field = "engineHorsePower",
                        values =
                        {
                            "@min",
                            "@max"
                        },
                        key = "BETWEEN"
                    }

                );
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
                        this.queryBuilder = new QueryBuilder
                        (
                            new iQuery()
                            {
                                tablesColumns = new List<FieldValues>()
                                {
                                    new FieldValues()
                                    {
                                        field = "enrollment",
                                        values = new List<string>
                                        {
                                            "serial",
                                            "number",
                                            "id"
                                        }
                                    },
                                    new FieldValues()
                                    {
                                        field = "vehicle",
                                        values = new List<string>
                                        {
                                            "color",
                                            "engineIsStarted",
                                            "engineHorsePower"
                                        }
                                    }
                                },
                                innerValues = new List<FieldValues>()
                                {
                                    new FieldValues(){
                                        field = "id",
                                        values = new List<string>()
                                        {
                                            "enrollmentId"
                                        }
                                    }
                                },
                                whereValues = this.whereValues
                            }
                        );
                        sentence.CommandText = $"{this.queryBuilder.complexSelect()}";
                        DBCommandExtensions.setParameters(sentence, this.queryParameters);
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = (int)reader.GetValue(2);
                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    copyParameters(sentence, sentence2);
                                    this.doorsDto = new List<DoorDto>();
                                    this.wheelsDto = new List<WheelDto>();
                                    IList<whereFieldValues> whereValues = new List<whereFieldValues>()
                                    {
                                        WhereParam("=", "id")
                                    };
                                    sentence2.Parameters.Add(SetParameter(sentence, new Param("id", id)));
                                    this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "door", values = new List<string>() { "isOpen" } } }, whereValues = whereValues });
                                    sentence2.CommandText = $"{this.queryBuilder.select()}";
                                    elements(sentence2, "door");
                                    this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "wheel", values = new List<string>() { "pressure" } } }, whereValues = whereValues });
                                    sentence2.CommandText = $"{this.queryBuilder.select()}";
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
                            this.doorsDto.Add(new DoorDto(Convert.ToBoolean(reader2.GetValue(0))));
                        else
                            this.wheelsDto.Add(new WheelDto(Convert.ToDouble(reader2.GetValue(0))));
                    }
                }
            }
            private static void copyParameters(IDbCommand sentence, IDbCommand sentence2)
            {
                if (sentence2.Parameters == null)
                {
                    foreach (IDbDataParameter parameter in sentence.Parameters)
                        sentence2.Parameters.Add(parameter);
                }
            }
        }
    }
}