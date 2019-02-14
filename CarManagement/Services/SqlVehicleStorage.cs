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
        const string QUERY_VEHICLE_SKEL = "SELECT * FROM vehicle WHERE enrollmentId=@id";
        const string QUERY_WHEEL_SKEL = "SELECT * FROM wheel WHERE vehicleId=@id";
        const string QUERY_DOOR_SKEL = "SELECT * FROM door WHERE vehicleId=@id";

        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count
        {
            get
            {
                const string COUNT_QUERY_ENROLL = "SELECT COUNT(*) FROM enrollment";
                using (SqlConnection sqlDbConnection = new SqlConnection(this.connectionString))
                {
                    SqlCommand sqlOperation = new SqlCommand(COUNT_QUERY_ENROLL, sqlDbConnection);
                    sqlDbConnection.Open();
                    int enrollmentCount = (int)sqlOperation.ExecuteScalar();
                    sqlDbConnection.Close();
                    return enrollmentCount;
                }
            }
        }

        public void clear()
        {
            const string DELETE_ENROLL_SKEL = "DELETE FROM enrollment";
            const string DELETE_VEHICLE_SKEL = "DELETE FROM vehicle";
            const string DELETE_WHEEL_SKEL = "DELETE FROM wheel";
            const string DELETE_DOOR_SKEL = "DELETE FROM door";

            using (SqlConnection sqlDbConnection = new SqlConnection(this.connectionString))
            {
                SqlCommand deletionCommand = new SqlCommand(DELETE_DOOR_SKEL, sqlDbConnection);

                sqlDbConnection.Open();
                deletionCommand.ExecuteNonQuery();
                deletionCommand = new SqlCommand(DELETE_WHEEL_SKEL, sqlDbConnection);
                deletionCommand.ExecuteNonQuery();
                deletionCommand = new SqlCommand(DELETE_VEHICLE_SKEL, sqlDbConnection);
                deletionCommand.ExecuteNonQuery();
                deletionCommand = new SqlCommand(DELETE_ENROLL_SKEL, sqlDbConnection);
                deletionCommand.ExecuteNonQuery();

                sqlDbConnection.Close();
            }
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
            #region "SQL skels CONSTS"
            const string QUERY_ENROLL_SKEL = "SELECT * FROM enrollment " +
                "WHERE serial=@serial AND number=@number";
            const string QUERY_DOOR_SKEL = "SELECT id FROM door " +
                "WHERE vehicleId=@id";
            const string QUERY_WHEEL_SKEL = "SELECT id FROM wheel " +
                "WHERE  vehicleId=@id";

            const string UPDATE_VEHICLE_SKEL = "UPDATE vehicle" +
                " SET color=@color, engineIsStarted=@engineIsStarted, engineHorsePower=@engineHorsePower" +
                " WHERE enrollmentId=@id";
            const string UPDATE_DOOR_SKEL = "UPDATE door SET isOpen=@isOpen " +
                "WHERE vehicleId=@vehicleId AND id=@id";
            const string UPDATE_WHEEL_SKEL = "UPDATE wheel SET pressure=@pressure " +
                "WHERE vehicleId=@vehicleId AND id=@id";

            const string INSERT_ENROLL_SKEL = "INSERT INTO enrollment " +
                "(serial, number)" +
                " output INSERTED.ID VALUES " +
                "(@serial, @number)";
            const string INSERT_VEHICLE_SKEL = "INSERT INTO vehicle" +
                " (enrollmentId, color, engineHorsePower, engineIsStarted)" +
                " VALUES (@enrollmentId, @color, @engineHorsePower, @engineIsStarted)";
            const string INSERT_DOOR_SKEL = "INSERT INTO door " +
                "(vehicleId, isOpen) " +
                "VALUES (@vehicleId, @isOpen)";
            const string INSERT_WHEEL_SKEL = "INSERT INTO wheel " +
                "(vehicleId, pressure) " +
                "VALUES (@vehicleId, @pressure)";
            #endregion

            using (SqlConnection sqlDbConnection = new SqlConnection(this.connectionString))
            {
                SqlCommand sqlOperation = new SqlCommand(QUERY_ENROLL_SKEL, sqlDbConnection);
                sqlOperation.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                sqlOperation.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                sqlDbConnection.Open();

                using (SqlDataReader sqlReader = sqlOperation.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        sqlOperation = new SqlCommand(UPDATE_VEHICLE_SKEL, sqlDbConnection);
                        sqlOperation.Parameters.AddWithValue("@id", (int)sqlReader["id"]);
                        sqlOperation.Parameters.AddWithValue("@color", vehicle.Color);
                        sqlOperation.Parameters.AddWithValue("@engineHorsePower", vehicle.Engine.HorsePower);
                        sqlOperation.Parameters.AddWithValue("@engineIsStarted", vehicle.Engine.IsStarted);
                        sqlOperation.ExecuteNonQuery();

                        int i = 0;
                        sqlOperation = new SqlCommand(QUERY_DOOR_SKEL, sqlDbConnection);
                        sqlOperation.Parameters.AddWithValue("@id", (int)sqlReader["id"]);
                        using (SqlDataReader sqlDoorReader = sqlOperation.ExecuteReader())
                        {
                            while (sqlDoorReader.Read())
                            {
                                if(vehicle.Doors.Count() > i)
                                {
                                    sqlOperation = new SqlCommand(UPDATE_DOOR_SKEL, sqlDbConnection);
                                    sqlOperation.Parameters.AddWithValue("@vehicleId", (int)sqlReader["id"]);
                                    sqlOperation.Parameters.AddWithValue("@id", (int)sqlDoorReader["id"]);
                                    sqlOperation.Parameters.AddWithValue("@isOpen", vehicle.Doors[i].IsOpen);
                                    sqlOperation.ExecuteNonQuery();
                                }
                                else
                                {
                                    throw new NotImplementedException();
                                }
                                i++;
                            }
                        }

                        i = 0;
                        sqlOperation = new SqlCommand(QUERY_WHEEL_SKEL, sqlDbConnection);
                        sqlOperation.Parameters.AddWithValue("@id", (int)sqlReader["id"]);
                        using (SqlDataReader sqlWheelReader = sqlOperation.ExecuteReader())
                        {
                            while (sqlWheelReader.Read())
                            {
                                if (vehicle.Doors.Count() > i)
                                {
                                    sqlOperation = new SqlCommand(UPDATE_WHEEL_SKEL, sqlDbConnection);
                                    sqlOperation.Parameters.AddWithValue("@vehicleId", (int)sqlReader["id"]);
                                    sqlOperation.Parameters.AddWithValue("@id", (int)sqlWheelReader["id"]);
                                    sqlOperation.Parameters.AddWithValue("@pressure", vehicle.Wheels[i].Pressure);
                                    sqlOperation.ExecuteNonQuery();
                                }
                                else
                                {
                                    throw new NotImplementedException();
                                }
                                i++;
                            }
                        }
                    }
                    else
                    {
                        SqlCommand inserter = new SqlCommand(INSERT_ENROLL_SKEL, sqlDbConnection);
                        inserter.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                        inserter.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                        int databaseEnrollmentId = (int)inserter.ExecuteScalar();

                        inserter = new SqlCommand(INSERT_VEHICLE_SKEL, sqlDbConnection);
                        inserter.Parameters.AddWithValue("@enrollmentId", databaseEnrollmentId);
                        inserter.Parameters.AddWithValue("@color", vehicle.Color);
                        inserter.Parameters.AddWithValue("@engineHorsePower", vehicle.Engine.HorsePower);
                        inserter.Parameters.AddWithValue("@engineIsStarted", vehicle.Engine.IsStarted);
                        inserter.ExecuteNonQuery();

                        foreach (IDoor door in vehicle.Doors)
                        {
                            inserter = new SqlCommand(INSERT_DOOR_SKEL, sqlDbConnection);
                            inserter.Parameters.AddWithValue("@vehicleId", databaseEnrollmentId);
                            inserter.Parameters.AddWithValue("@isOpen", door.IsOpen);
                            inserter.ExecuteNonQuery();
                        }

                        foreach (IWheel wheel in vehicle.Wheels)
                        {
                            inserter = new SqlCommand(INSERT_WHEEL_SKEL, sqlDbConnection);
                            inserter.Parameters.AddWithValue("@vehicleId", databaseEnrollmentId);
                            inserter.Parameters.AddWithValue("@pressure", wheel.Pressure);
                            inserter.ExecuteNonQuery();
                        }
                    }
                }

            }
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            
            private Dictionary<string, object> arguments = new Dictionary<string, object>();

            #region "query CONST"
            const string SELECT_STANDART = "SELECT id, serial, number, color, engineIsStarted, engineHorsePower" +
                "  FROM vehicle, enrollment WHERE enrollmentId = id";

            const string AND = " AND ";

            const string COLOR_COND = "vehicle.color = @color";
            const string ENGINE_STARTED_COND = "vehicle.engineIsStarted = @isStarted";
            const string ENGINE_HORSE_COND = "vehicle.engineHorsePower = @horsePower";
            const string ENGINE_MINMAX_HORSE_COND = "vehicle.engineHorsePower >= @horsePower " +
                "AND vehicle.engineHorsePower <= @maxHorsePower";
            const string ENROLL_SERIAL_COND = "enrollment.serial = @serial";
            const string ENROLL_COND = "enrollment.serial = @serial AND enrollment.number = @number";
            #endregion

            private string specifiedQuery = SELECT_STANDART;

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor? color)
            {
                if (color != null)
                {
                    this.arguments.Add("@color", color);

                    this.specifiedQuery += AND + COLOR_COND;
                }

                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool? started)
            {
                if (started != null)
                {
                    this.arguments.Add("@isStarted", started);

                    this.specifiedQuery += AND + ENGINE_STARTED_COND;
                }

                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                if (enrollment != null)
                {
                    this.arguments.Add("@serial", enrollment.Serial);
                    this.arguments.Add("@number", enrollment.Number);

                    this.specifiedQuery += AND + ENROLL_COND;
                }

                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                if (serial != null)
                {
                    this.arguments.Add("@serial", serial);

                    this.specifiedQuery += AND + ENROLL_SERIAL_COND;
                }

                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int? horsePower)
            {
                if (horsePower != null)
                {
                    this.arguments.Add("@horsePower", horsePower);

                    this.specifiedQuery += AND + ENGINE_HORSE_COND;
                }

                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int? min, int? max)
            {
                if (min != null && max != null)
                {
                    this.arguments.Add("@horsePower", min);
                    this.arguments.Add("@maxHorsePower", max);

                    this.specifiedQuery += AND + ENGINE_MINMAX_HORSE_COND;
                }

                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }

            private IEnumerator<IVehicle> enumerate()
            {
                using (SqlConnection sqlDbConnection = new SqlConnection(this.connectionString))
                {
                    SqlCommand querier = new SqlCommand(this.specifiedQuery, sqlDbConnection);
                    foreach ( KeyValuePair<string,object> argEntry in this.arguments)
                    {
                        querier.Parameters.AddWithValue(argEntry.Key, argEntry.Value);
                    }

                    sqlDbConnection.Open();
                    using (SqlDataReader vehicleReader = querier.ExecuteReader())
                    {
                        while(vehicleReader.Read())
                        {
                            if (vehicleReader["color"] != null)
                            {
                                VehicleDto vehicleDto = new VehicleDto
                                {
                                    Enrollment = new EnrollmentDto
                                    {
                                        Serial = vehicleReader["serial"].ToString(),
                                        Number = Convert.ToInt32(vehicleReader["number"]),
                                    },
                                    Engine = new EngineDto
                                    {
                                        HorsePower = Convert.ToInt16(vehicleReader["engineHorsePower"]),
                                        IsStarted = Convert.ToBoolean(vehicleReader["engineIsStarted"]),
                                    },
                                    Color = (CarColor) Convert.ToInt32(vehicleReader["color"]),
                                };

                                vehicleDto.Wheels = queryArrayItemsOfId
                                    (
                                        (int)vehicleReader["id"],
                                        QUERY_WHEEL_SKEL,
                                        sqlDbConnection,
                                        dataReader => new WheelDto
                                        {
                                            Pressure = Convert.ToDouble(dataReader["pressure"]),
                                        }
                                    );

                                vehicleDto.Doors = queryArrayItemsOfId
                                    (
                                        (int)vehicleReader["id"],
                                        QUERY_DOOR_SKEL,
                                        sqlDbConnection,
                                        dataReader => new DoorDto
                                        {
                                            IsOpen = Convert.ToBoolean(dataReader["isOpen"]),
                                        }
                                    );

                                yield return this.vehicleBuilder.import(vehicleDto);
                            }
                        }
                    }
                    sqlDbConnection.Close();
                }
            }

            private static T[] queryArrayItemsOfId<T>(int masterId, string comand,
                SqlConnection sqlDbConnection, Func<SqlDataReader, T> func,
                string identifier = "@id")
            {
                SqlCommand querier;
                List<T> items = new List<T>();
                querier = new SqlCommand(comand, sqlDbConnection);
                querier.Parameters.AddWithValue(identifier, masterId);
                using (SqlDataReader sqlItemReader = querier.ExecuteReader())
                {
                    while (sqlItemReader.Read())
                    {
                        items.Add
                        (
                            func(sqlItemReader)
                        );
                    }
                }

                return items.ToArray();
            }

        }
    }
}
