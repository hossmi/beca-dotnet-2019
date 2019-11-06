﻿using System;
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

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private IDataParameter parameter;
        private readonly List<int> idList;
        private int id;
        private object query;
        private QueryBuilder queryBuilder;
        const string COUNT_VEHICLE = @"USE CarManagement
            SELECT count(enrollmentId) AS 'Count' FROM vehicle";

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
            this.idList = new List<int>();
            this.queryBuilder = new QueryBuilder();
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
                    this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "enrollment", values = new List<string>() { "id" } } }});
                    sentence.CommandText = $"{this.queryBuilder.newquerySelect}";
                    //this.queryBuilder = new QueryBuilder("id ", "enrollment");
                    //sentence.CommandText = $"{this.queryBuilder.querySelect}";
                    using (IDataReader reader = sentence.ExecuteReader())
                    {
                        while (reader.Read())
                            this.idList.Add((int)reader.GetValue(0));
                        reader.Close();
                    }
                    foreach (int id in this.idList)
                    {
                        this.parameter = setParameter(sentence, "@id", id);
                        sentence.Parameters.Add(this.parameter);
                        sentence.CommandText = deleteAllQuery().ToString();
                        sentence.ExecuteNonQuery();
                        sentence.Parameters.Remove(this.parameter);
                    }
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
                    sentence.Parameters.Add(setParameter(sentence, "@serial", enrollment.Serial));
                    sentence.Parameters.Add(setParameter(sentence, "@number", enrollment.Number));
                    selectIdEnrollment(sentence);
                    readEnrollmentId(sentence);
                    selectVehicle(sentence);
                    if (sentence.ExecuteScalar() != null)
                    {
                        sentence.CommandText = deleteAllQuery().ToString();
                        sentence.ExecuteNonQuery();
                    }
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
                    sentence.Parameters.Add(setParameter(sentence, "@serial", vehicle.Enrollment.Serial));
                    sentence.Parameters.Add(setParameter(sentence, "@number", vehicle.Enrollment.Number));
                    selectIdEnrollment(sentence);
                    if (sentence.ExecuteScalar() != null)
                    {
                        readEnrollmentId(sentence);
                        this.query = selectVehicle(sentence);
                        List<string> columnsValues = setVehicleParameters(vehicle, sentence);
                        if (this.query != null)
                            updateVehicle(sentence, vehicle, columnsValues);
                        else
                        {
                            this.queryBuilder = new QueryBuilder(new FieldValues() { field = "vehicle", values = columnsValues });
                            sentence.CommandText = $"{this.queryBuilder.queryInsert}";
                            insertwheelsdoors(vehicle, sentence);
                        }
                    }
                    else
                    {
                        insertEnrollment(sentence);
                        selectIdEnrollment(sentence);
                        readEnrollmentId(sentence);
                        insertVehicle(sentence, vehicle);
                    }
                }
            }
        }

        private static IDataParameter insertParams(IDbCommand sentence, List<string> columnsValues, object thing, string name)
        {
            columnsValues.Add(name);
            return setParameter(sentence, $"@{name}", thing);
        }
        private static IDataParameter setParameter(IDbCommand sentence, string name, object thing)
        {
            IDataParameter parameter = sentence.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = thing;
            return parameter;
        }
        private static List<string> setVehicleParameters(IVehicle vehicle, IDbCommand sentence)
        {
            List<string> columnsValues = new List<string>();
            sentence.Parameters.Add(insertParams(sentence, columnsValues, (int)vehicle.Color, "color"));
            sentence.Parameters.Add(insertParams(sentence, columnsValues, vehicle.Engine.IsStarted ? 1 : 0, "engineIsStarted"));
            sentence.Parameters.Add(insertParams(sentence, columnsValues, vehicle.Engine.HorsePower, "engineHorsePower"));
            return columnsValues;
        }
        private void insertEnrollment(IDbCommand sentence)
        {
            List<string> columnsValues = new List<string>
            {
                "serial",
                "number"
            };
            this.queryBuilder = new QueryBuilder(new FieldValues() { field = "enrollment", values = columnsValues });
            sentence.CommandText = $"{this.queryBuilder.queryInsert}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private void insertVehicle(IDbCommand sentence, IVehicle vehicle)
        {
            List<string> columnsValues = setVehicleParameters(vehicle, sentence);
            columnsValues.Add("id");
            this.queryBuilder = new QueryBuilder(new FieldValues() { field = "vehicle", values = columnsValues });
            sentence.CommandText = $"{this.queryBuilder.queryInsert}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            insertwheelsdoors(vehicle, sentence);
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private void insertwheelsdoors(IVehicle vehicle, IDbCommand sentence)
        {
            foreach (IWheel wheel in vehicle.Wheels)
                makeWheelDoor(sentence, wheel.Pressure, "pressure", "wheel");
            foreach (IDoor door in vehicle.Doors)
                makeWheelDoor(sentence, door.IsOpen, "isOpen", "door");
        }
        private void makeWheelDoor(IDbCommand sentence, object parameter, string column, string table)
        {
            sentence.Parameters.Clear();
            List<string> columnsValues = new List<string>();
            sentence.Parameters.Add(insertParams(sentence, columnsValues, this.id, "id"));
            sentence.Parameters.Add(insertParams(sentence, columnsValues, parameter, column));
            this.queryBuilder = new QueryBuilder(new FieldValues() { field = table, values = columnsValues });
            sentence.CommandText = $"{this.queryBuilder.queryInsert}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private object selectVehicle(IDbCommand sentence)
        {
            IList<whereFieldValues> whereValues = new List<whereFieldValues>();
            whereParam("=", "id", whereValues);

            //this.queryBuilder = new QueryBuilder("*", "vehicle", whereValues);
            //sentence.CommandText = $"{this.queryBuilder.querySelect}";
            this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "vehicle", values = new List<string>() { "*" } } }, whereValues = whereValues });
            sentence.CommandText = $"{this.queryBuilder.newquerySelect}";
            return sentence.ExecuteScalar();
        }
        private void selectIdEnrollment(IDbCommand sentence)
        {
            IList<whereFieldValues> whereValues = new List<whereFieldValues>();
            whereParam("=", "serial", whereValues);
            whereParam("=", "number", whereValues);
            this.queryBuilder = new QueryBuilder("id", "enrollment", whereValues);
            sentence.CommandText = $"{this.queryBuilder.querySelect}";
            //this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "enrollment", values = new List<string>() { "id" } } }, whereValues = whereValues });
            //sentence.CommandText = $"{this.queryBuilder.newquerySelect}";
        }
        private static void whereParam(string key, string param, IList<string> keys, IList<FieldValues> whereParams)
        {
            keys.Add(key);
            whereParams.Add(new FieldValues()
            {
                field = param,
                values = new
                List<string>
                {
                    $"@{param}"
                }
            });
        }
        private static void whereParam(string key, string param, IList<whereFieldValues> whereValues)
        {
            whereValues.Add(new whereFieldValues()
            {
                field = param,
                values = new
                List<string>
                {
                    $"@{param}"
                },
                key = key
            });
        }
        private void readEnrollmentId(IDbCommand sentence)
        {
            using (IDataReader reader = sentence.ExecuteReader())
            {
                reader.Read();
                this.id = (int)reader.GetValue(0);
                sentence.Parameters.Add(setParameter(sentence, $"@{reader.GetName(0)}", reader.GetValue(0)));
                reader.Close();
            }
        }
        private void updateVehicle(IDbCommand sentence, IVehicle vehicle, IList<string> columnsValues)
        {
            IList<whereFieldValues> whereValues = new List<whereFieldValues>();
            whereParam("=", "id", whereValues);
            this.queryBuilder = new QueryBuilder(new FieldValues() { field = "vehicle", values = columnsValues }, whereValues);
            sentence.CommandText = $"{this.queryBuilder.queryUpdate}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            deleteWheelsDoors(sentence);
            insertwheelsdoors(vehicle, sentence);
        }
        private StringBuilder deleteAllQuery()
        {
            IList<whereFieldValues> wherevalues = new List<whereFieldValues>();
            iQuery iQuery = new iQuery();
            iQuery.tablesColumns = new List<FieldValues>();
            whereParam("=", "id", wherevalues);
            StringBuilder query = new StringBuilder();
            this.queryBuilder = new QueryBuilder(new iQuery() {tablesColumns = new List<FieldValues>() { new FieldValues() { field = "wheel", values = new List<string>() }}, whereValues = wherevalues});
            query.Insert(query.Length, $"{this.queryBuilder.newqueryDelete};");
            this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "door", values = new List<string>() } }, whereValues = wherevalues });
            query.Insert(query.Length, $"{this.queryBuilder.newqueryDelete};");
            this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "vehicle", values = new List<string>() } }, whereValues = wherevalues });
            query.Insert(query.Length, $"{this.queryBuilder.newqueryDelete};");
            return query;
        }
        private void deleteWheelsDoors(IDbCommand sentence)
        {
            iQuery iquery = new iQuery();
            iquery.whereValues = new List<whereFieldValues>();
            whereParam("=", "id", iquery.whereValues);
            iquery.tablesColumns = new List<FieldValues>() { new FieldValues() { field = "wheel" } };
            this.queryBuilder = new QueryBuilder(iquery);
            sentence.CommandText = $"{this.queryBuilder.newqueryDelete}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            iquery.tablesColumns = new List<FieldValues>() { new FieldValues() { field = "door" } };
            this.queryBuilder = new QueryBuilder(iquery);
            sentence.CommandText = $"{this.queryBuilder.newqueryDelete}";
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private class PrvVehicleQuery : IVehicleQuery
        {
            private QueryBuilder queryBuilder = new QueryBuilder();
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IEnrollmentProvider enrollmentProvider;
            private int id;
            private readonly IDictionary<string, object> queryParameters;
            private readonly IList<FieldValues> tablesColumns;
            private readonly IList<whereFieldValues> whereValues;
            private readonly IDictionary<string, string> joinConditions;
            private readonly IList<string> columns;
            private IList<string> values;
            private IList<string> keys;
            private IList<WheelDto> wheelsDto;
            private IList<DoorDto> doorsDto;
            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.enrollmentProvider = new DefaultEnrollmentProvider();
                this.queryParameters = new Dictionary<string, object>();
                this.values = new List<string>();
                this.keys = new List<string>();
                this.whereValues = new List<whereFieldValues>();
                this.tablesColumns = new List<FieldValues>();
                this.columns = new List<string>();
                this.joinConditions = new Dictionary<string, string>();
            }
            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        this.queryBuilder = new QueryBuilder("serial, number ", "enrollment");
                        sentence.CommandText = $"{this.queryBuilder.querySelect}";
                        //this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "enrollment", values = new List<string>() { "serial, number " } } } });
                        //sentence.CommandText = $"{this.queryBuilder.newquerySelect}";
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
                whereParam("=", "color", this.whereValues);
                this.queryParameters.Add("@color", (int)color);
                return this;
            }
            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                whereParam("=", "engineIsStarted", this.whereValues);
                this.queryParameters.Add("@engineIsStarted", started);
                return this;
            }
            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                whereParam("=", "number", this.whereValues);
                this.queryParameters.Add("@number", enrollment.Number);
                whereParam("=", "serial", this.whereValues);
                this.queryParameters.Add("@serial", enrollment.Serial);
                return this;
            }
            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                whereParam("=", "serial", this.whereValues);
                this.queryParameters.Add("@serial", serial);
                return this;
            }
            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                whereParam("=", "engineHorsePower", this.whereValues);
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
                        sentence.CommandText = $@"
                            {
                                this.queryBuilder.complexSelect
                                (
                                    new List<FieldValues>
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
                                    new List<FieldValues>()
                                    {
                                        new FieldValues(){
                                            field = "id",
                                            values = new List<string>()
                                            {
                                                "enrollmentId"
                                            }
                                        }
                                    }
                                )
                            }
                            {
                                this.queryBuilder.where
                                (
                                    "enrollment",
                                    this.whereValues
                                )
                            }";
                        DBCommandExtensions.setParameters(sentence, this.queryParameters);
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.id = (int)reader.GetValue(2);
                                using (IDbCommand sentence2 = con.CreateCommand())
                                {
                                    copyParameters(sentence, sentence2);
                                    this.doorsDto = new List<DoorDto>();
                                    this.wheelsDto = new List<WheelDto>();
                                    IList<whereFieldValues> whereValues = new List<whereFieldValues>();
                                    sentence2.Parameters.Add(setParameter(sentence, "@id", this.id));

                                    whereParam("=", "id", this.whereValues);
                                    this.queryBuilder = new QueryBuilder("isOpen ", "door", whereValues);
                                    sentence2.CommandText = $"{this.queryBuilder.querySelect}";
                                    //this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "door", values = new List<string>() { "isOpen" } } }, whereValues = this.whereValues });
                                    //sentence.CommandText = $"{this.queryBuilder.newquerySelect}";
                                    elements(sentence2, "door");
                                    this.queryBuilder = new QueryBuilder("pressure ", "wheel", whereValues);
                                    sentence2.CommandText = $"{this.queryBuilder.querySelect}";
                                    //this.queryBuilder = new QueryBuilder(new iQuery() { tablesColumns = new List<FieldValues>() { new FieldValues() { field = "wheel", values = new List<string>() { "pressure" } } }, whereValues = whereValues });
                                    //sentence.CommandText = $"{this.queryBuilder.newquerySelect}";
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