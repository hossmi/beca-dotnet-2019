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
using ToolBox.Models;
using ToolBox.Services;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private const string DELETE_FROM_WHEEL_DOOR_VEHICLE_ENROLLMENT = @"
            DELETE FROM wheel
            WHERE vehicleId IN (SELECT enrollmentId AS vehicleId FROM vehicle)

            DELETE FROM door
            WHERE vehicleId IN (SELECT enrollmentId AS vehicleId FROM vehicle)

            DELETE FROM vehicle
            WHERE enrollmentid IN (SELECT id FROM enrollment)

            DELETE FROM enrollment";

        private const string SELECT_COUNT_VEHICLES = "SELECT COUNT(enrollmentId) FROM vehicle";

        private const string SELECT_VEHICLE_ID = @"
            SELECT		v.enrollmentId AS vehicleId
            FROM		vehicle v
            INNER JOIN	enrollment e ON v.enrollmentId = e.id
            WHERE		e.serial = @serial
            AND			e.number = @number";

        private const string SELECT_ENROLLMENT_ID = @"
            SELECT		id
            FROM		enrollment e
            WHERE		e.serial = @serial
            AND			e.number = @number";

        private const string INSERT_ENROLLMENT = @"
            INSERT enrollment (serial, number)
            VALUES (@serial, @number)";

        private const string INSERT_VEHICLE = @"
            INSERT INTO vehicle (enrollmentId, color, engineHorsePower, engineIsStarted)
            VALUES (@enrollmentId, @color, @engineHorsePower, @engineIsStarted)";

        private const string INSERT_WHEEL = @"
            INSERT INTO wheel (vehicleId, pressure)
            VALUES (@vehicleId, @pressure)";

        private const string INSERT_DOOR = @"
            INSERT INTO door (vehicleId, isOpen)
            VALUES (@vehicleId, @isOpen)";

        private const string UPDATE_VEHICLE = @"
            UPDATE vehicle
               SET color = @color
                  , engineHorsePower = @engineHorsePower
                  , engineIsStarted = @engineIsStarted
             WHERE enrollmentId = @enrollmentId";

        private const string DELETE_WHEELS = @"DELETE FROM wheel WHERE vehicleId = @vehicleId";
        private const string DELETE_DOORS = @"DELETE FROM door WHERE vehicleId = @vehicleId";

        private const string SELECT_VEHICLES = @"
            SELECT		v.enrollmentId AS vehicleId, v.color, v.engineHorsePower, v.engineIsStarted
            FROM		vehicle v
            INNER JOIN	enrollment e ON v.enrollmentId = e.id
            ";

        private static readonly string SELECT_WHEELS = @"SELECT vehicleId, pressure FROM wheel WHERE vehicleId = @vehicleId";
        private static readonly string SELECT_DOORS = @"SELECT vehicleId, isOpen FROM door WHERE vehicleId = @vehicleId";

        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private readonly DB<SqlConnection> db;

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
            this.db = new DB<SqlConnection>(connectionString);
        }

        public int Count
        {
            get
            {
                return (int)this.db.selectValue(SELECT_COUNT_VEHICLES);
            }
        }

        public void clear()
        {
            this.db.write(DELETE_FROM_WHEEL_DOOR_VEHICLE_ENROLLMENT);
        }

        public void Dispose()
        {
        }

        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }

        public void set(IVehicle vehicle)
        {
            this.db.transact(commandBuilder =>
            {
                IDbCommand command = commandBuilder
                    .setQuery(SELECT_VEHICLE_ID)
                    .setParameter("@serial", vehicle.Enrollment.Serial)
                    .setParameter("@number", vehicle.Enrollment.Number)
                    .build();

                int vehicleId = 0;

                using (IDataReader reader = command.ExecuteReader())
                    if (reader.Read())
                        vehicleId = (int)reader["vehicleId"];

                if (vehicleId > 0)
                    update(commandBuilder, vehicle, vehicleId);
                else
                    insert(commandBuilder, vehicle);

                return TransactionAction.Commit;
            });
        }

        private static void insert(ICommandBuilder commandBuilder, IVehicle vehicle)
        {
            int affectedRows, vehicleId;

            affectedRows = commandBuilder
                .setQuery(INSERT_ENROLLMENT)
                .setParameter("@serial", vehicle.Enrollment.Serial)
                .setParameter("@number", vehicle.Enrollment.Number)
                .build()
                .ExecuteNonQuery();

            Asserts.isTrue(affectedRows == 1);

            vehicleId = (int)commandBuilder
                .setQuery(SELECT_ENROLLMENT_ID)
                .setParameter("@serial", vehicle.Enrollment.Serial)
                .setParameter("@number", vehicle.Enrollment.Number)
                .build()
                .ExecuteScalar();

            Asserts.isTrue(vehicleId > 0);

            affectedRows = commandBuilder
                .setQuery(INSERT_VEHICLE)
                .setParameter("@enrollmentId", vehicleId)
                .setParameter("@color", vehicle.Color)
                .setParameter("@engineHorsePower", vehicle.Engine.HorsePower)
                .setParameter("@engineIsStarted", vehicle.Engine.IsStarted)
                .build()
                .ExecuteNonQuery();

            Asserts.isTrue(affectedRows == 1);

            foreach (IWheel wheel in vehicle.Wheels)
            {
                affectedRows = commandBuilder
                    .setQuery(INSERT_WHEEL)
                    .setParameter("@vehicleId", vehicleId)
                    .setParameter("@pressure", wheel.Pressure)
                    .build()
                    .ExecuteNonQuery();

                Asserts.isTrue(affectedRows == 1);
            }

            foreach (IDoor door in vehicle.Doors)
            {
                affectedRows = commandBuilder
                    .setQuery(INSERT_DOOR)
                    .setParameter("@vehicleId", vehicleId)
                    .setParameter("@isOpen", door.IsOpen)
                    .build()
                    .ExecuteNonQuery();

                Asserts.isTrue(affectedRows == 1);
            }
        }

        private static void update(ICommandBuilder commandBuilder, IVehicle vehicle, int vehicleId)
        {
            int affectedRows;

            affectedRows = commandBuilder
                .setQuery(UPDATE_VEHICLE)
                .setParameter("@enrollmentId", vehicleId)
                .setParameter("@color", vehicle.Color)
                .setParameter("@engineHorsePower", vehicle.Engine.HorsePower)
                .setParameter("@engineIsStarted", vehicle.Engine.IsStarted)
                .build()
                .ExecuteNonQuery();

            Asserts.isTrue(affectedRows == 1);

            commandBuilder
                .setQuery(DELETE_WHEELS)
                .setParameter("@vehicleId", vehicleId)
                .build()
                .ExecuteNonQuery();

            foreach (IWheel wheel in vehicle.Wheels)
            {
                affectedRows = commandBuilder
                    .setQuery(INSERT_WHEEL)
                    .setParameter("@vehicleId", vehicleId)
                    .setParameter("@pressure", wheel.Pressure)
                    .build()
                    .ExecuteNonQuery();

                Asserts.isTrue(affectedRows == 1);
            }

            commandBuilder
                .setQuery(DELETE_DOORS)
                .setParameter("@vehicleId", vehicleId)
                .build()
                .ExecuteNonQuery();

            foreach (IDoor door in vehicle.Doors)
            {
                affectedRows = commandBuilder
                    .setQuery(INSERT_DOOR)
                    .setParameter("@vehicleId", vehicleId)
                    .setParameter("@isOpen", door.IsOpen)
                    .build()
                    .ExecuteNonQuery();

                Asserts.isTrue(affectedRows == 1);
            }
        }

        private static IEnumerator<IVehicle> readAndBuildVehicles(string connectionString, string query, IDictionary<string, object> parameters, IVehicleBuilder vehicleBuilder)
        {
            using (IDbConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (IDbTransaction transaction = connection.BeginTransaction())
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;

                    command.CommandText = query;
                    command.setParameters(parameters);

                    using (IDataReader reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            int vehicleId = (int)reader["vehicleId"];

                            VehicleDto vehicleDto = buildVehicleDto(reader);
                            vehicleDto.Wheels = build(transaction, SELECT_WHEELS, vehicleId, buildWheel).ToArray();
                            vehicleDto.Doors = build(transaction, SELECT_DOORS, vehicleId, buildDoor).ToArray();

                            IVehicle vehicle = vehicleBuilder.import(vehicleDto);
                            yield return vehicle;
                        }

                    transaction.Commit();
                }

                connection.Close();
            }
        }

        private static WheelDto buildWheel(IDataRecord record)
        {
            return new WheelDto
            {
                Pressure = (float)record["pressure"],
            };
        }

        private static DoorDto buildDoor(IDataRecord record)
        {
            return new DoorDto
            {
                 IsOpen = Convert.ToBoolean(record["isOpen"]),
            };
        }

        private static VehicleDto buildVehicleDto(IDataReader reader)
        {
            return new VehicleDto
            {
                Color = (CarColor)(int)reader["color"],
                Engine = new EngineDto
                {
                    HorsePower = (short)reader["engineHorsePower"],
                    IsStarted = Convert.ToBoolean(reader["engineIsStarted"]),
                },
                Enrollment = new EnrollmentDto
                {
                    Serial = reader["serial"].ToString(),
                    Number = (short)reader["number"],
                },
            };
        }

        private static IEnumerable<T> build<T>(IDbTransaction transaction, string query, int vehicleId, Func<IDataRecord, T> buildDelegate) where T: class, new()
        {
            using (IDbCommand command = transaction.Connection.CreateCommand())
            {
                command.Transaction = transaction;

                command.CommandText = query;
                command.setParameter("@vehicleId", vehicleId);

                using (IDataReader reader = command.ExecuteReader())
                    while (reader.Read())
                        yield return buildDelegate(reader);
            }
        }

        private static string buildQuery(string query, IEnumerable<string> conditions)
        {
            return conditions
                .Aggregate(new StringBuilder(query).Append(" WHERE 1=1 "), (accum, condition) => accum.AppendLine(condition))
                .ToString();
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private const string AND_PREFIX = " AND ";
            private const string FILTER_BY_ENROLLMENT = "FILTER_BY_ENROLLMENT";
            private const string FILTER_BY_HORSEPOWER = "FILTER_BY_HORSEPOWER";
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IDictionary<string, string> conditions;
            private readonly IDictionary<string, object> parameters;

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.conditions = new Dictionary<string, string>();
                this.parameters = new Dictionary<string, object>();
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                string condition = nameof(whereColorIs);

                Asserts.isFalse(this.conditions.ContainsKey(condition));
                this.conditions[condition] = AND_PREFIX + " v.color = @color ";
                this.parameters["@color"] = color;

                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                string condition = nameof(whereEngineIsStarted);

                Asserts.isFalse(this.conditions.ContainsKey(condition));
                this.conditions[condition] = AND_PREFIX + " v.engineIsStarted = @engineIsStarted ";
                this.parameters["@engineIsStarted"] = started;

                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                string condition = nameof(FILTER_BY_ENROLLMENT);

                Asserts.isFalse(this.conditions.ContainsKey(condition));
                this.conditions[condition] = AND_PREFIX + " e.serial = @serial AND e.number = @number ";
                this.parameters["@serial"] = enrollment.Serial;
                this.parameters["@number"] = enrollment.Number;

                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                string condition = nameof(FILTER_BY_ENROLLMENT);

                Asserts.isFalse(this.conditions.ContainsKey(condition));
                this.conditions[condition] = AND_PREFIX + " e.serial = @serial ";
                this.parameters["@serial"] = serial;

                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                string condition = nameof(FILTER_BY_HORSEPOWER);

                Asserts.isFalse(this.conditions.ContainsKey(condition));
                this.conditions[condition] = AND_PREFIX + " v.engineHorsePower = @engineHorsePower ";
                this.parameters["@engineHorsePower"] = horsePower;

                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                string condition = nameof(FILTER_BY_HORSEPOWER);

                Asserts.isFalse(this.conditions.ContainsKey(condition));
                this.conditions[condition] = $"{AND_PREFIX} @engineHorsePowerMin <= v.engineHorsePower {AND_PREFIX} v.engineHorsePower <= @engineHorsePowerMax ";
                this.parameters["@engineHorsePowerMin"] = min;
                this.parameters["@engineHorsePowerMax"] = max;

                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }

            private IEnumerator<IVehicle> enumerate()
            {
                string query = buildQuery(SELECT_VEHICLES, this.conditions.Values);
                return readAndBuildVehicles(this.connectionString, query, this.parameters, this.vehicleBuilder);
            }

        }
    }
}
