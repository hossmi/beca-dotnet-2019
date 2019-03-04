using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CarManagement.Core;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using ToolBox.Extensions.DbCommands;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private IDataParameter parameter;
        private List<string> idList;
        private object query;
        private int id;
        const string COUNT_VEHICLE = @"USE CarManagement
            SELECT count(enrollmentId) AS 'Count' FROM vehicle";

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
            this.idList = new List<string>();
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
                    sentence.CommandText = makeSelectDelete("SELECT id ", "enrollment");
                    using (IDataReader reader = sentence.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            this.idList.Add(reader.GetValue(0).ToString());
                        }
                        reader.Close();
                    }
                    foreach (string id in this.idList)
                    {
                        this.parameter = setParameter(sentence, "@id", id);
                        sentence.Parameters.Add(this.parameter);
                        sentence.CommandText = deleteAll();
                        sentence.ExecuteNonQuery();
                        sentence.Parameters.Remove(this.parameter);

                    }
                }

            }

        }
        private static string deleteAll()
        {
            string query = $"{makeSelectDelete("DELETE ", "door")}{makeSimpleWhere("vehicleId", "@id")};" +
                $"{makeSelectDelete("DELETE ", "wheel")}{makeSimpleWhere("vehicleId", "@id")};" +
                $"{makeSelectDelete("DELETE ", "vehicle")}{makeSimpleWhere("enrollmentId", "@id")};" +
                $"{makeSelectDelete("DELETE ", "enrollment")}{makeSimpleWhere("id", "@id")};";
            return query;
        }
        public void Dispose()
        {
            using (IDbConnection con = new SqlConnection(this.connectionString))
            {
                con.Close();
            }    
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
                    sentence.CommandText = makeSelectDelete("SELECT id ","enrollment") + "WHERE serial = @serial AND number = @number";

                    using (IDataReader reader = sentence.ExecuteReader())
                    {
                        reader.Read();
                        this.id = (int)reader["enrollmentId"];
                        reader.Close();
                    }
                    sentence.CommandText = $"{makeSelectDelete("SELECT * ", "vehicle")}{makeSimpleWhere("enrollmentId", "@id")};";
                    sentence.Parameters.Add(setParameter(sentence, "@id", this.id));
                    if (sentence.ExecuteScalar() != null)
                    {
                        sentence.CommandText = deleteAll();
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
                        selectVehicle(sentence);
                        this.query = sentence.ExecuteScalar();
                        setVehicleParameters(vehicle, sentence);

                        if (this.query != null)
                        {
                            updateVehicle(sentence);
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

        private static void insertEnrollment(IDbCommand sentence)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("serial", "@serial");
            dictionary.Add("number", "@number");
            sentence.CommandText = makeInsertUpdate("INSERT INTO ", "enrollment", dictionary, "VALUES ");
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private static void setVehicleParameters(IVehicle vehicle, IDbCommand sentence)
        {
            sentence.Parameters.Add(setParameter(sentence, "@color", (int)vehicle.Color));
            sentence.Parameters.Add(setParameter(sentence, "@engineIsStarted", vehicle.Engine.IsStarted ? 1 : 0));
            sentence.Parameters.Add(setParameter(sentence, "@engineHorsePower", vehicle.Engine.HorsePower));
        }
        private static void readEnrollmentId(IDbCommand sentence)
        {
            using (IDataReader reader = sentence.ExecuteReader())
            {
                //sentence.Parameters.Clear();
                reader.Read();
                sentence.Parameters.Add(setParameter(sentence, $"@{reader.GetName(0)}", reader.GetValue(0)));
                reader.Close();
            }
        }
        private static void updateVehicle(IDbCommand sentence)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("color", "@color");
            dictionary.Add("engineHorsePower", "@engineHorsePower");
            dictionary.Add("engineIsStarted", "@engineIsStarted");
            IDictionary<string, string> where = new Dictionary<string, string>();
            where.Add("@id", "enrollmentId");
            sentence.CommandText = createQuery(makeInsertUpdate("UPDATE ", "vehicle ", dictionary, "SET "), buildConditions(where, "vehicle"));
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private static void deleteWheelsDoors(IDbCommand sentence)
        {
            IDictionary<string, string> where = new Dictionary<string, string>();
            where.Add("@id", "vehicleId");
            sentence.CommandText = createQuery(makeSelectDelete("DELETE ", "wheel "), buildConditions(where, "vehicle"));
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            sentence.CommandText = createQuery(makeSelectDelete("DELETE ", "door "), buildConditions(where, "vehicle"));
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
        }
        private static void insertVehicle(IDbCommand sentence)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("enrollmentId", "@id");
            dictionary.Add("color", "@color");
            dictionary.Add("engineHorsePower", "@engineHorsePower");
            dictionary.Add("engineIsStarted", "@engineIsStarted");
            sentence.CommandText = makeInsertUpdate("INSERT INTO ", "vehicle", dictionary, "VALUES ");
        }
        private static IDictionary<string, string> selectVehicle(IDbCommand sentence)
        {
            IDictionary<string, string> where = new Dictionary<string, string>();
            where.Add("@id", "enrollmentId");
            sentence.CommandText = createQuery(makeSelectDelete("SELECT * ", "vehicle"), buildConditions(where, "vehicle"));
            return where;
        }
        private static void selectIdEnrollment(IDbCommand sentence)
        {
            IDictionary<string, string> where = new Dictionary<string, string>();
            where.Add("@serial", "serial");
            where.Add("@number", "number");
            sentence.CommandText = createQuery(makeSelectDelete("SELECT id ", "enrollment"), buildConditions(where, "erollment"));
        }
        private static void makeWheelDoor(IDbCommand sentence, object parameter, string column, string table)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("vehicleId", "@id");
            dictionary.Add(column, $"@{column}");
            inserObject(makeInsertUpdate("INSERT INTO ", table, dictionary, "VALUES "), sentence, $"@{column}", parameter);
        }
        private static void insertwheelsdoors(IVehicle vehicle, IDbCommand sentence)
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
        private static IDictionary<string, string> buildConditions(IDictionary<string, string> where, string table)
        {
            IDictionary<string, string> conditions = new Dictionary<string, string>();
            foreach (string value in where.Values)
            {
                if (value == "id" || value == "enrollmentId" || value == "vehicleId")
                {
                    conditions.Add($"@id", $"{value} = @id");
                }
                else 
                {
                    conditions.Add($"@{value}", $"{value} = @{value}");
                }
            }
            return conditions;
        }
        private static IDataParameter setParameter(IDbCommand sentence, string name, object thing)
        {
            IDataParameter parameter = sentence.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = thing;
            return parameter;
        }
        private static string createQuery(string query, IDictionary<string, string> conditionParts)
        {
            int counter = 0;
            foreach (string conditionPart in conditionParts.Values)
            {
                if (counter == 0)
                {
                    query += " WHERE " + conditionPart;
                }
                else
                {
                    query += " AND " + conditionPart;
                }
                counter++;
            }
            return query;
        }
        private static string makeSimpleWhere(string field, string condition)
        {
            return $" WHERE {field} = {condition}";
        }
        private static string makeElementString(List<string> elements)
        {
            int counter = 0;
            string query = "";
            foreach (string element in elements)
            {
                if (counter < elements.Count -1)
                {
                    query += element + ", ";
                }
                else  
                {
                    query += element;
                }
                counter++;
            }
            return query;
        }
        private static string makeSelectDelete(string command, string table)
        {
            return $"{command} FROM {table}";
        }
        private static string makeInsertUpdate(string type, string table, IDictionary<string, string> dataDictionary, string instruction)
        {
            List<string> columns, values;
            convertDataToList(dataDictionary, out columns, out values);
            if (type.Contains("UPDATE"))
            {
                return type + table + instruction + makeElementString(buildStringList(dataDictionary));
            }
            else
            {
                return $"{type}{table}({makeElementString(columns)}){instruction}({makeElementString(values)})";
            }
        }
        private static void convertDataToList(IDictionary<string, string> dataDictionary, out List<string> columns, out List<string> values)
        {
            columns = new List<string>();
            values = new List<string>();
            foreach (KeyValuePair<string, string> pair in dataDictionary)
            {
                values.Add(pair.Value);
                columns.Add(pair.Key);
            }
            /*foreach (string value in dataDictionary.Values)
            {
                values.Add(value);
            }
            foreach (string column in dataDictionary.Keys)
            {
                columns.Add(column);
            }*/
        }
        private static List<string> buildStringList(IDictionary<string, string> dictionary)
        {
            List<string> strinList = new List<string>();
            foreach (KeyValuePair<string, string> key in dictionary)
            {
                strinList.Add($"{key.Key} = {key.Value}");
            }
            return strinList;
        }
        IDictionary<string, string> buildDictionary(string name, string parameter)
        {
            IDictionary<string, string> conditionPart = new Dictionary<string, string>();
            conditionPart.Add(parameter, $"{name} = {parameter}");
            return conditionPart;
        }
        private static string makeSimpleQuery(string command, string table, string field, IDictionary<string, string> conditionParts, IDictionary<string, string> dataDictionary)
        {
            string query = "";
            if (command.Contains("SELECT") || command.Contains("DELETE"))
            {
                query = makeSelectDelete(command, table);
                query += createQuery(query, conditionParts);
            }
            else if (command.Contains("INSERT"))
            {
                query = makeInsertUpdate(" SET ", table, dataDictionary, command);
            }
            else if (command.Contains("UPDATE"))
            {
                query = makeInsertUpdate(" VALUES ", table, dataDictionary, command);
                query += createQuery(query, conditionParts);
            }
            return query;
        }
        private static void inserObject(string query, IDbCommand sentence, string name, object param)
        {
            IDataParameter parameter = setParameter(sentence, name, param);
            sentence.Parameters.Add(parameter);
            sentence.CommandText = query;
            Asserts.isTrue(sentence.ExecuteNonQuery() > 0);
            sentence.Parameters.Remove(parameter);
        }
        private static void makeElements(string type, IDbCommand sentence, object[] things)
        {
            foreach (dynamic thing in things)
            {
                if (type == "door")
                {
                    makeWheelDoor(sentence, thing.IsOpen, "isOpen", "door");
                }
                else if (type == "wheel")
                {
                    makeWheelDoor(sentence, thing.Pressure, "pressure", "wheel");
                }
            }
            /*if (type == "door")
            {
                foreach (IDoor door in things)
                {
                    wheeldoorMaker(sentence, door.IsOpen, "isOpen", "door");
                }
            }
            else if (type == "wheel")
            {
                foreach (IWheel wheel in things)
                {
                    wheeldoorMaker(sentence, wheel.Pressure, "pressure", "wheel");
                }
            }*/
        }
        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private VehicleBuilder vehicleBuilder2;
            private CarColor color;
            private bool engineIsStarted;
            private IEnrollment enrollment;
            private string enrollmentSerial;
            private int horsePower;
            private int min;
            private int max;
            private int id;
            private IDictionary<string, object> queryParameters;
            private IDictionary<string, string> queryParts;
            private IEnrollmentProvider enrollmentProvider;
            private EnrollmentDto enrollmentDto;
            private VehicleDto vehicleDto;
            private EngineDto engineDto;
            private List<WheelDto> wheelsDto;
            private List<DoorDto> doorsDto;
            private WheelDto wheelDto;
            private DoorDto doorDto;
            private IDictionary<string, object> dictionaryId;
            private IDictionary<string, string> dictionaryId2;
            private const string SELECT_VEHICLE_HEAD = @"
                SELECT e.serial, 
                    e.number, 
                    e.id, 
                    v.color, 
                    v.engineIsStarted, 
                    v.engineHorsePower 
                FROM enrollment e 
                INNER JOIN vehicle v ON e.id = v.enrollmentId ";

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.enrollmentProvider = new DefaultEnrollmentProvider();
                this.vehicleBuilder2 = new VehicleBuilder(this.enrollmentProvider);
                this.queryParts = new Dictionary<string, string>();
                this.queryParameters = new Dictionary<string, object>();
                this.vehicleDto = new VehicleDto();
                this.enrollmentDto = new EnrollmentDto();
                this.engineDto = new EngineDto();
                this.wheelsDto = new List<WheelDto>();
                this.doorsDto = new List<DoorDto>();
                this.enrollmentProvider = new DefaultEnrollmentProvider();
                this.dictionaryId = new Dictionary<string, object>();
                this.dictionaryId2 = new Dictionary<string, string>();

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
                addParametersDictionary("@color", "color", (int)color, this.queryParameters, this.queryParts);
                this.color = color;
                return this;
            }
            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                addParametersDictionary("@engineIsStarted", "engineIsStarted", started, this.queryParameters, this.queryParts);
                this.engineIsStarted = started;
                return this;
            }
            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                addParametersDictionary("@number", "number", enrollment.Number, this.queryParameters, this.queryParts);
                addParametersDictionary("@serial", "serial", enrollment.Serial, this.queryParameters, this.queryParts);
                this.enrollment = enrollment;
                return this;
            }
            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.enrollmentSerial = serial;
                addParametersDictionary("@serial", "serial", serial, this.queryParameters, this.queryParts);
                return this;
            }
            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                addParametersDictionary("@engineHorsePower", "engineHorsePower", horsePower, this.queryParameters, this.queryParts);
                this.horsePower = horsePower;
                return this;
            }
            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min > 0);
                Asserts.isTrue(max > 0);
                this.queryParameters.Add("@min", min);
                this.queryParameters.Add("@max", max);
                this.queryParts.Add("@engineHorsePower", "engineHorsePower BETWEEN @min AND @max");
                this.max = max;
                this.min = min;
                return this;
            }
            private static void addParametersDictionary(string key, string sqlCollumn, object parameter, IDictionary<string, object> queryParameters, IDictionary<string, string> queryParts)
            {
                string command = command = sqlCollumn + " = " + key;
                queryParameters.Add(key, parameter);
                queryParts.Add(key, command);
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
                        sentence.CommandText = createQuery(SELECT_VEHICLE_HEAD, this.queryParts);
                        DBCommandExtensions.setParameters(sentence, this.queryParameters);

                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.enrollmentDto = this.vehicleBuilder2.enrollmentDtoBuilder(reader.GetValue(0).ToString(), Convert.ToInt32(reader.GetValue(1)));
                                this.id = (int)reader.GetValue(2);
                                this.color = (CarColor)Enum.Parse(typeof(CarColor), reader.GetValue(3).ToString());
                                this.engineDto = this.vehicleBuilder2.engineDtoBuilder(Convert.ToBoolean(reader.GetValue(4)), Convert.ToInt32(reader.GetValue(5)));
                                CreateElement(con, sentence, "wheel");
                                CreateElement(con, sentence, "door");
                                yield return this.vehicleBuilder.import(this.vehicleBuilder2.vehicleDtoBuilder(this.enrollmentDto, this.engineDto, this.color, this.wheelsDto.ToArray(), this.doorsDto.ToArray()));
                            }
                            reader.Close();
                        }
                    }
                }
            }

            private void CreateElement(IDbConnection con, IDbCommand sentence, string type)
            {
                using (IDbCommand sentence2 = con.CreateCommand())
                {
                    copyParameters(sentence, sentence2);
                    sentence2.Parameters.Add(setParameter(sentence2, "@id", this.id));
                    if (type == "door")
                    {
                        sentence2.CommandText = makeSelectDelete("SELECT isOpen ", type) + addWhere("vehicleId", "@id");

                    }
                    else if (type == "wheel")
                    {
                        sentence2.CommandText = makeSelectDelete("SELECT pressure ", type) + addWhere("vehicleId", "@id");
                    }
                    extractElement(sentence2, type);
                }
            }
            private void extractElement(IDbCommand sentence2, string type)
            {
                using (IDataReader reader2 = sentence2.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        if (type == "wheel")
                        {
                            this.wheelDto = this.vehicleBuilder2.wheelDtoBuilder(Convert.ToDouble(reader2.GetValue(0)));
                            this.wheelsDto.Add(this.wheelDto);
                        }
                        else if (type == "door")
                        {
                            this.doorDto = this.vehicleBuilder2.doorDtoBuilder(Convert.ToBoolean(reader2.GetValue(0)));
                            this.doorsDto.Add(this.doorDto);
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

            private static string createQuery(string query, IDictionary<string, string> conditionParts)
            {
                int counter = 0;
                foreach (string conditionPart in conditionParts.Values)
                {
                    if (counter == 0)
                    {
                        query += " WHERE " + conditionPart;
                    }
                    else
                    {
                        query += " AND " + conditionPart;
                    }
                    counter++;
                }
                return query;
            }
            private static string addWhere(string field, string condition)
            {
                string where = $" WHERE {field} = {condition}";
                return where;
            }
            private static string elementString(string query, List<string> elements)
            {
                int counter = 0;
                foreach (string element in elements)
                {
                    if (counter < elements.Count)
                    {
                        query += element + ", ";
                    }
                    else
                    {
                        query += element + ")";
                    }
                    counter++;
                }
                return query;
            }
            private static string makeSelectDelete(string command, string table)
            {
                string select = $"{command} FROM {table}";
                return select;
            }
            private IEnumerable<IEnrollment> enumerateEnrollments()
            {
                using (IDbConnection con = new SqlConnection(this.connectionString))
                {
                    con.Open();
                    using (IDbCommand sentence = con.CreateCommand())
                    {
                        sentence.CommandText = makeSelectDelete("SELECT * ", "enrollment");
                        using (IDataReader reader = sentence.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                yield return this.enrollmentProvider.import(reader["serial"].ToString(), Convert.ToInt16(reader["number"]));
                            }
                        }
                    }
                }
                
            }
        }
    }
}