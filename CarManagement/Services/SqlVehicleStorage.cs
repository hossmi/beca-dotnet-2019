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
                const string COUNT_QUERRY_ENROLL = "SELECT COUNT(*) FROM enrollment";
                using (SqlConnection sqlDbConnection = new SqlConnection(this.connectionString))
                {
                    SqlCommand sqlOperation = new SqlCommand(COUNT_QUERRY_ENROLL, sqlDbConnection);
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
        //    const string QUERY_ENROLL_SKEL = "SELECT * FROM enrollment WHERE serial=@serial AND number=@number";

        //    IVehicle queriedVehicleOrNull;

        //    using (SqlConnection sqlDbConnection = new SqlConnection(this.connectionString))
        //    {
        //        SqlCommand querier = new SqlCommand(QUERY_ENROLL_SKEL, sqlDbConnection);
        //        querier.Parameters.AddWithValue("@serial", enrollment.Serial);
        //        querier.Parameters.AddWithValue("@number", enrollment.Number);
        //        sqlDbConnection.Open();

        //        using (SqlDataReader sqlReader = querier.ExecuteReader())
        //        {
        //            if(sqlReader.Read())
        //            {
        //                querier = new SqlCommand(QUERY_VEHICLE_SKEL, sqlDbConnection);
        //                querier.Parameters.AddWithValue("@id", (int)sqlReader["id"]);
        //                SqlDataReader sqlVehicleReader = querier.ExecuteReader();

        //                if (sqlVehicleReader.Read())
        //                {

        //                    List<DoorDto> doorList = new List<DoorDto>();
        //                    querier = new SqlCommand(QUERY_DOOR_SKEL, sqlDbConnection);
        //                    querier.Parameters.AddWithValue("@id", (int)sqlReader["id"]);
        //                    using (SqlDataReader sqlDoorReader = querier.ExecuteReader())
        //                    {
        //                        while (sqlDoorReader.Read())
        //                        {
        //                            doorList.Add(new DoorDto { IsOpen = Convert.ToBoolean(sqlDoorReader["isOpen"]) });
        //                        }
        //                    }

        //                    List<WheelDto> wheelList = new List<WheelDto>();
        //                    querier = new SqlCommand(QUERY_WHEEL_SKEL, sqlDbConnection);
        //                    querier.Parameters.AddWithValue("@id", (int)sqlReader["id"]);
        //                    using (SqlDataReader sqlWheelReader = querier.ExecuteReader())
        //                    {
        //                        while (sqlWheelReader.Read())
        //                        {
        //                            wheelList.Add(new WheelDto { Pressure = Convert.ToDouble(sqlWheelReader["pressure"]) });
        //                        }
        //                    }

        //                    queriedVehicleOrNull = this.vehicleBuilder
        //                        .import(
        //                            new VehicleDto
        //                            {
        //                                Enrollment = new EnrollmentDto
        //                                {
        //                                    Serial = sqlReader["serial"].ToString(),
        //                                    Number = Convert.ToInt32(sqlReader["number"]),
        //                                },
        //                                Engine = new EngineDto
        //                                {
        //                                    IsStarted = Convert.ToBoolean(sqlVehicleReader["engineIsStarted"]),
        //                                    HorsePower = Convert.ToInt16(sqlVehicleReader["engineHorsePower"]),
        //                                },
        //                                Color = (CarColor)Convert.ToInt32(sqlVehicleReader["color"]),
        //                                Doors = doorList.ToArray(),
        //                                Wheels = wheelList.ToArray(),
        //                            }
        //                        );
        //                }
        //                else
        //                {
        //                    queriedVehicleOrNull = null;
        //                }
        //            }
        //            else
        //            {
        //                queriedVehicleOrNull = null;
        //            }
        //        }

        //        sqlDbConnection.Close();
        //    }
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
                                sqlOperation = new SqlCommand(UPDATE_DOOR_SKEL, sqlDbConnection);
                                sqlOperation.Parameters.AddWithValue("@vehicleId", (int)sqlReader["id"]);
                                sqlOperation.Parameters.AddWithValue("@id", (int)sqlDoorReader["id"]);
                                sqlOperation.Parameters.AddWithValue("@isOpen", vehicle.Doors[i].IsOpen);
                                sqlOperation.ExecuteNonQuery();
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
                                sqlOperation = new SqlCommand(UPDATE_WHEEL_SKEL, sqlDbConnection);
                                sqlOperation.Parameters.AddWithValue("@vehicleId", (int)sqlReader["id"]);
                                sqlOperation.Parameters.AddWithValue("@id", (int)sqlWheelReader["id"]);
                                sqlOperation.Parameters.AddWithValue("@pressure", vehicle.Wheels[i].Pressure);
                                sqlOperation.ExecuteNonQuery();
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

            private CarColor? color;
            private string enrollSerial;
            private int? enrollNumber;
            private bool? isStarted;
            private int? horsePower;
            private int? maxHorsePower;

            #region "query CONST"
            const string SELECT_STANDART = "SELECT id, serial, number, color, engineIsStarted, engineHorsePower" +
                "  FROM vehicle, enrollment WHERE enrollmentId = id";
            const string SELECT_WHEELS = "SELECT pressure FROM wheel WHERE vehicleId = @id";
            const string SELECT_DOORS = "SELECT isOpen FROM door WHERE vehicleId = @id";

            const string AND = " AND ";

            const string COLOR_COND = "vehicle.color = @color";
            const string ENGINE_STARTED_COND = "vehicle.engineIsStarted = @isStarted";
            const string ENGINE_HORSE_COND = "vehicle.engineHorsePower = @horsePower";
            const string ENGINE_MINMAX_HORSE_COND = "vehicle.engineHorsePower >= @minHorsePower " +
                "AND vehicle.engineHorsePower <= @maxHorsePower";
            const string ENROLL_SERIAL_COND = "enrollment.serial = @serial";
            const string ENROLL_COND = "enrollment.serial = @serial AND enrollment.number = @number";
            #endregion

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                if(this.color != null)
                {
                    Asserts.isTrue(false, "Color must be specified only once");
                }
                this.color = color;
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                if (this.isStarted != null)
                {
                    Asserts.isTrue(false, "isStarted must be specified only once");
                }
                this.isStarted = started;
                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                if (this.enrollSerial != null)
                {
                    Asserts.isTrue(false, "There is already an active filter with serial");
                }
                if (this.enrollNumber != null)
                {
                    Asserts.isTrue(false, "There is already an active filter with serial");
                }
                this.enrollNumber = enrollment.Number;
                this.enrollSerial = enrollment.Serial;
                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                if (this.enrollSerial != null)
                {
                    Asserts.isTrue(false, "There is already an active filter with serial");
                }
                this.enrollSerial =serial;
                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                if (this.horsePower != null)
                {
                    Asserts.isTrue(false, "There is already an active filter with horsePower");
                }
                this.horsePower = horsePower;
                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                if (this.horsePower != null)
                {
                    Asserts.isTrue(false, "There is already an active filter with horsePower");
                }
                if (this.maxHorsePower != null)
                {
                    Asserts.isTrue(false, "There is already an active filter with horsePower");
                }
                this.maxHorsePower = max;
                this.horsePower = min;
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }

            private IEnumerator<IVehicle> enumerate()
            {
                return hateIfTheyBringLove;
            }

        }
    }
}
