using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        private List<int> idList;
        private object query;
        private QueryBuilder queryBuilder = new QueryBuilder();
        const string COUNT_VEHICLE = @"USE CarManagement
            SELECT count(enrollmentId) AS 'Count' FROM vehicle";

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
            this.idList = new List<int>();
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
                    sentence.CommandText = this.queryBuilder.select("id ", "enrollment").ToString();
                    using (IDataReader reader = sentence.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            this.idList.Add((int)reader.GetValue(0));
                        }
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
                        setVehicleParameters(vehicle, sentence);

                        if (this.query != null)
                        {
                            updateVehicle(sentence);
                            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                            deleteWheelsDoors(sentence);
                            insertwheelsdoors(vehicle, sentence);
                        }
                        else
                        {
                            insertVehicle(sentence);
                            insertwheelsdoors(vehicle, sentence);
                        }
                    }
                    else
                    {
                        insertEnrollment(sentence);
                        selectIdEnrollment(sentence);
                        readEnrollmentId(sentence);
                        setVehicleParameters(vehicle, sentence);
                        insertVehicle(sentence);
                        Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
                        insertwheelsdoors(vehicle, sentence);
                    }
                }
            }
        }

        private static IDataParameter setParameter(IDbCommand sentence, string name, object thing)
        {
            IDataParameter parameter = sentence.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = thing;
            return parameter;
        }
        private static void setVehicleParameters(IVehicle vehicle, IDbCommand sentence)
        {
            sentence.Parameters.Add(setParameter(sentence, "@color", (int)vehicle.Color));
            sentence.Parameters.Add(setParameter(sentence, "@engineIsStarted", vehicle.Engine.IsStarted ? 1 : 0));
            sentence.Parameters.Add(setParameter(sentence, "@engineHorsePower", vehicle.Engine.HorsePower));
        }
        private static void inserObject(string query, IDbCommand sentence, string name, object param)
        {
            IDataParameter parameter = setParameter(sentence, name, param);
            sentence.Parameters.Add(parameter);
            sentence.CommandText = query;
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            sentence.Parameters.Remove(parameter);
        }
        
        private void insertEnrollment(IDbCommand sentence)
        {
            List<string> columnsValues = new List<string>();
            columnsValues.Add("serial");
            columnsValues.Add("number");

            sentence.CommandText = this.queryBuilder.insert("enrollment", columnsValues).ToString();
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private void insertVehicle(IDbCommand sentence)
        {
            List<string> columnsValues = new List<string>();
            columnsValues.Add("id");
            columnsValues.Add("color");
            columnsValues.Add("engineHorsePower");
            columnsValues.Add("engineIsStarted");
            sentence.CommandText = this.queryBuilder.insert("vehicle", columnsValues).ToString();
        }
        private void insertwheelsdoors(IVehicle vehicle, IDbCommand sentence)
        {
            foreach (IWheel wheel in vehicle.Wheels)
            {
                makeWheelDoor(sentence, wheel.Pressure, "pressure", "wheel");
            }
            foreach (IDoor door in vehicle.Doors)
            {
                makeWheelDoor(sentence, door.IsOpen, "isOpen", "door");
            }
        }
        private void makeWheelDoor(IDbCommand sentence, object parameter, string column, string table)
        {
            List<string> columnsValues = new List<string>();
            columnsValues.Add("id");
            columnsValues.Add(column);
            inserObject(this.queryBuilder.insert(table, columnsValues).ToString(), sentence, $"@{column}", parameter);
        }

        private object selectVehicle(IDbCommand sentence)
        {
            List<string> values = new List<string>();
            List<string> keys = new List<string>();
            IDictionary<List<string>, string> whereParams = new Dictionary<List<string>, string>();
            values.Add("@id");
            whereParams.Add(values, "id");
            keys.Add("=");
            sentence.CommandText = $"{this.queryBuilder.select("*", "vehicle")}{this.queryBuilder.where("vehicle", whereParams, keys)}";
            return sentence.ExecuteScalar();
        }
        private void selectIdEnrollment(IDbCommand sentence)
        {
            List<string> values = new List<string>();
            List<string> keys = new List<string>();
            IDictionary<List<string>, string> whereParams = new Dictionary<List<string>, string>();
            values.Add("@serial");
            whereParams.Add(values, "serial");
            keys.Add("=");
            values = new List<string>();
            values.Add("@number");
            whereParams.Add(values, "number");
            keys.Add("=");
            sentence.CommandText = $"{this.queryBuilder.select("id", "enrollment")}{this.queryBuilder.where("vehicle", whereParams, keys)}";
        }
        private static void readEnrollmentId(IDbCommand sentence)
        {
            using (IDataReader reader = sentence.ExecuteReader())
            {
                reader.Read();
                sentence.Parameters.Add(setParameter(sentence, $"@{reader.GetName(0)}", reader.GetValue(0)));
                reader.Close();
            }
        }
        private void updateVehicle(IDbCommand sentence)
        {
            List<string> conditions = new List<string>();
            List<List<string>> values = new List<List<string>>();
            List<string> value = new List<string>();
            List<string> keys = new List<string>();

            List<string> columnsValues = new List<string>();
            IDictionary<List<string>, string> whereParams = new Dictionary<List<string>, string>();
            columnsValues.Add("color");
            columnsValues.Add("engineHorsePower");
            columnsValues.Add("engineIsStarted");
            value.Add("@id");
            whereParams.Add(value, "id");
            keys.Add("=");
            sentence.CommandText = this.queryBuilder.update("vehicle", columnsValues, whereParams, keys).ToString();
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private StringBuilder deleteAllQuery()
        {
            IDictionary<List<string>, string> whereParams = new Dictionary<List<string>, string>();
            List<string> values = new List<string>();
            List<string> keys = new List<string>();
            values.Add("@id");
            whereParams.Add(values, "id");
            keys.Add("=");
            StringBuilder query = new StringBuilder();
            query.Insert(query.Length, $@"
                {this.queryBuilder.delete("wheel", whereParams, keys)};
                {this.queryBuilder.delete("door", whereParams, keys)};
                {this.queryBuilder.delete("vehicle", whereParams, keys)};");

            return query;
        }
        private void deleteWheelsDoors(IDbCommand sentence)
        {
            IDictionary<List<string>, string> whereParams = new Dictionary<List<string>, string>();
            List<string> values = new List<string>();
            List<string> keys = new List<string>();
            values.Add("@id");
            whereParams.Add(values, "id");
            keys.Add("=");

            sentence.CommandText = this.queryBuilder.delete("wheel", whereParams, keys).ToString();
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            sentence.CommandText = this.queryBuilder.delete("door", whereParams, keys).ToString();
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private class PrvVehicleQuery : IVehicleQuery
        {
            private QueryBuilder queryBuilder = new QueryBuilder();
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IEnrollmentProvider enrollmentProvider;
            private int id;
            private IDictionary<string, object> queryParameters;
            private IDictionary<List<string>, string> whereParams;
            private IDictionary<string, List<string>> tablesColumns;
            private IDictionary<string, string> joinConditions;
            private List<string> columns;
            private List<string> values;
            private List<string> keys;
            private List<WheelDto> wheelsDto;
            private List<DoorDto> doorsDto;
            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.enrollmentProvider = new DefaultEnrollmentProvider();
                this.queryParameters = new Dictionary<string, object>();
                this.values = new List<string>();
                this.keys = new List<string>();
                this.whereParams = new Dictionary<List<string>, string>();
                this.tablesColumns = new Dictionary<string, List<string>>();
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
                        sentence.CommandText = this.queryBuilder.select("serial, number ", "enrollment").ToString();
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
                this.values.Add("@color");
                this.keys.Add("=");
                this.whereParams.Add(this.values, "color");
                this.queryParameters.Add("@color", (int)color);
                return this;
            }
            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                this.values.Add("@engineIsStarted");
                this.keys.Add("=");
                this.whereParams.Add(this.values, "engineIsStarted");
                this.queryParameters.Add("@engineIsStarted", started);
                return this;
            }
            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                this.values.Add("@number");
                this.keys.Add("=");
                this.whereParams.Add(this.values, "number");
                this.values = new List<string>();
                this.queryParameters.Add("@number", enrollment.Number);
                this.values.Add("@serial");
                this.keys.Add("=");
                this.whereParams.Add(this.values, "serial");
                this.queryParameters.Add("@serial", enrollment.Serial);
                return this;
            }
            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.values.Add("@serial");
                this.keys.Add("=");
                this.whereParams.Add(this.values, "serial");
                return this;
            }
            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                this.values.Add("@engineHorsePower");
                this.keys.Add("=");
                this.whereParams.Add(this.values, "engineHorsePower");
                this.queryParameters.Add("@engineHorsePower", horsePower);
                return this;
            }
            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min > 0);
                Asserts.isTrue(max > 0);
                this.values.Add("@min");
                this.values.Add("@max");
                this.whereParams.Add(this.values, "engineHorsePower");
                this.keys.Add("BETWEEN");
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
                        this.columns.Add("serial");
                        this.columns.Add("number");
                        this.columns.Add("id");
                        this.tablesColumns.Add("enrollment", this.columns);
                        this.columns = new List<string>();
                        this.columns.Add("color");
                        this.columns.Add("engineIsStarted");
                        this.columns.Add("engineHorsePower");
                        this.tablesColumns.Add("vehicle", this.columns);
                        this.joinConditions.Add("id", "enrollmentId");
                        sentence.CommandText = $"{this.queryBuilder.complexSelect(this.tablesColumns, this.joinConditions)}{this.queryBuilder.where("enrollment", this.whereParams, this.keys)}";
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
                                    this.values = new List<string>();
                                    this.keys = new List<string>();
                                    this.whereParams = new Dictionary<List<string>, string>();

                                    sentence2.Parameters.Add(setParameter(sentence, "@id", this.id));

                                    this.values.Add("@id");
                                    this.keys.Add("=");
                                    this.whereParams.Add(this.values, "id");

                                    sentence2.CommandText = $"{this.queryBuilder.select("isOpen ", "door")}{this.queryBuilder.where("wheel", this.whereParams, this.keys)}";
                                    elements(sentence2, "door");
                                    sentence2.CommandText = $"{this.queryBuilder.select("pressure ", "wheel")}{this.queryBuilder.where("wheel", this.whereParams, this.keys)}";
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
            private static void copyParameters(IDbCommand sentence, IDbCommand sentence2)
            {
                if (sentence2.Parameters == null)
                {
                    foreach (IDbDataParameter parameter in sentence.Parameters)
                    {
                        sentence2.Parameters.Add(parameter);
                    }
                }
            }
            private static IDataParameter setParameter(IDbCommand sentence, string name, object thing)
            {
                IDataParameter parameter = sentence.CreateParameter();
                parameter.ParameterName = name;
                parameter.Value = thing;
                return parameter;
            }
            
        }
    }
}