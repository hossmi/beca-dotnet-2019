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
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IVehicle get(IEnrollment enrollment)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IVehicle> getAll()
        {
            const int QUERY_TOP_NUM_ENTRIES = 32;
            const string QUERY_ENROLL_SKEL = "SELECT TOP @cuantity * FROM enrollment";
            const string QUERY_VEHICLE_SKEL = "SELECT (color, engineHorsePower, engineIsStarted) FROM vehicle WHERE enrollmentId=@id";
            const string QUERY_WHEEL_SKEL = "SELECT (isOpen) FROM wheel WHERE vehicleId=@id";
            const string QUERY_DOOR_SKEL = "SELECT (pressure) FROM door WHERE vehicleId=@id";

            List<IVehicle> vehiclesToReturn = new List<IVehicle>();

            IEnrollmentProvider EnrollProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(EnrollProvider);

            using (SqlConnection sqlDbConnection = new SqlConnection(this.connectionString))
            {
                SqlCommand querier = new SqlCommand(QUERY_ENROLL_SKEL, sqlDbConnection);
                sqlDbConnection.Open();
                querier.Parameters.AddWithValue("@cuantity", QUERY_TOP_NUM_ENTRIES);


                using (SqlDataReader sqlReader = querier.ExecuteReader())
                {
                    DataTable enrollTable = sqlReader.GetSchemaTable();

                    foreach (DataRow row in enrollTable.Rows)
                    {
                        EnrollmentDto enrollment = new EnrollmentDto();
                        enrollment.Number = row.Field<int>("number");
                        enrollment.Serial = row.Field<string>("serial");

                        querier = new SqlCommand(QUERY_VEHICLE_SKEL, sqlDbConnection);
                        querier.Parameters.AddWithValue("@id", row.Field<int>("id"));
                        DataRow dbVehicle = querier
                            .ExecuteReader()
                            .GetSchemaTable()
                            .Rows[0];

                        if (dbVehicle.IsNull(0) == false)
                        {
                            VehicleDto vehicle = new VehicleDto();
                            EngineDto engine = new EngineDto();

                            engine.HorsePower = dbVehicle.Field<int>("engineHorsePower");
                            engine.IsStarted = dbVehicle.Field<bool>("engineIsStarted");

                            vehicle.Color = dbVehicle.Field<CarColor>("color");
                            vehicle.Engine = engine;

                            querier = new SqlCommand(QUERY_DOOR_SKEL, sqlDbConnection);
                            querier.Parameters.AddWithValue("@id", row.Field<int>("id"));

                            List<DoorDto> doors = new List<DoorDto>();

                            DataTable doorTable = querier
                                .ExecuteReader()
                                .GetSchemaTable();

                            foreach (DataRow doorRow in doorTable.Rows)
                            {
                                DoorDto door = new DoorDto
                                {
                                    IsOpen = doorRow.Field<bool>("isOpen")
                                };
                                doors.Add(door);
                            }

                            vehicle.Doors = doors.ToArray();

                            querier = new SqlCommand(QUERY_WHEEL_SKEL, sqlDbConnection);
                            querier.Parameters.AddWithValue("@id", row.Field<int>("id"));

                            List<WheelDto> wheels = new List<WheelDto>();

                            DataTable wheelTable = querier
                                .ExecuteReader()
                                .GetSchemaTable();

                            foreach (DataRow wheelRow in wheelTable.Rows)
                            {
                                WheelDto wheel = new WheelDto
                                {
                                    Pressure = wheelRow.Field<double>("pressure")
                                };
                                wheels.Add(wheel);
                            }

                            vehicle.Wheels = wheels.ToArray();

                            vehiclesToReturn.Add(vehicleBuilder.import(vehicle));
                        }
                    }
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
