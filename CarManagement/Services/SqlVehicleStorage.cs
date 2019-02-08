using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public int Count { get; }

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

        public IVehicle get(IEnrollment enrollment)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IVehicle> getAll()
        {
            const string QUERY_ENROLL_SKEL = "SELECT * FROM enrollment";
            const string QUERY_VEHICLE_SKEL = "SELECT * FROM vehicle WHERE enrollmentId=@id";
            const string QUERY_WHEEL_SKEL = "SELECT * FROM wheel WHERE vehicleId=@id";
            const string QUERY_DOOR_SKEL = "SELECT * FROM door WHERE vehicleId=@id";

            List<IVehicle> vehiclesToReturn = new List<IVehicle>();

            using (SqlConnection sqlDbConnection = new SqlConnection(this.connectionString))
            {
                SqlCommand querier = new SqlCommand(QUERY_ENROLL_SKEL, sqlDbConnection);
                sqlDbConnection.Open();


                using (SqlDataReader sqlReader = querier.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        EnrollmentDto enrollment = new EnrollmentDto();
                        enrollment.Number = (short)sqlReader["number"];
                        enrollment.Serial = sqlReader["serial"].ToString();

                        querier = new SqlCommand(QUERY_VEHICLE_SKEL, sqlDbConnection);
                        querier.Parameters.AddWithValue("@id", (int)sqlReader["id"]);
                        SqlDataReader dbVehicle = querier
                            .ExecuteReader();

                        if (dbVehicle.Read())
                        {
                            VehicleDto vehicle = new VehicleDto
                            {
                                Enrollment = enrollment,
                                Engine = new EngineDto
                                {
                                    HorsePower = Convert.ToInt32( dbVehicle["engineHorsePower"]),
                                    IsStarted = Convert.ToBoolean(dbVehicle["engineIsStarted"])
                                },
                                Color = (CarColor)Convert.ToInt32(dbVehicle["color"])
                            };

                            querier = new SqlCommand(QUERY_DOOR_SKEL, sqlDbConnection);
                            querier.Parameters.AddWithValue("@id", (int)sqlReader["id"]);

                            List<DoorDto> doors = new List<DoorDto>();

                            IDataReader doorReader = querier
                                .ExecuteReader();

                            while (doorReader.Read())
                            {
                                DoorDto door = new DoorDto
                                {
                                    IsOpen = Convert.ToBoolean(doorReader["isOpen"])
                                };
                                doors.Add(door);

                            }

                            doorReader.Close();

                            vehicle.Doors = doors.ToArray();

                            querier = new SqlCommand(QUERY_WHEEL_SKEL, sqlDbConnection);
                            querier.Parameters.AddWithValue("@id", (int)sqlReader["id"]);

                            List<WheelDto> wheels = new List<WheelDto>();

                            IDataReader wheelReader = querier
                                .ExecuteReader();

                            while (wheelReader.Read())
                            {
                                WheelDto wheel = new WheelDto
                                {
                                    Pressure = Convert.ToDouble(wheelReader["pressure"])
                                };
                                wheels.Add(wheel);

                            }

                            wheelReader.Close();

                            vehicle.Wheels = wheels.ToArray();

                            vehiclesToReturn.Add(this.vehicleBuilder.import(vehicle));

                            dbVehicle.Close();
                        }
                    }
                    sqlReader.Close();
                }

                sqlDbConnection.Close();

            }

            return vehiclesToReturn;
        }

        public void set(IVehicle vehicle)
        {
            throw new NotImplementedException();
        }
    }
}
